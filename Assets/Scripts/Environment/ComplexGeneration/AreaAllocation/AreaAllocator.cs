using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaAllocationResult {
    public Dictionary<int, int> GeneratedAreaIdsByAllocationRequestId { get; set; }
}

public class AreaAllocator
{
    private readonly IEnumerable<IAreaAllocatorAreaFilter> areaFilters;

    // Initial
    private IEnumerable<AreaAllocationRequest> requestedAreas;
    private IEnumerable<AllocatableAreaGroup> areaGroups;
    private Graph<int> areaPossibleConnectivity;

    // Initialized
    private Dictionary<int, AllocatableAreaGroup> areaGroupsById;
    private Dictionary<int, List<AreaAllocationRequest>> requestedAreasByGroupId;
    private Dictionary<int, AreaAllocationRequest> centersByGroupId;

    private HashSet<AreaAllocationRequest> allocatedRequestedAreas;
    private HashSet<int> occupiedGeneratedAreaIds;

    private Dictionary<int, int> generatedAreaIdsByAllocationRequestId = new();

    public AreaAllocator(params IAreaAllocatorAreaFilter[] areaFilters)
    {
        this.areaFilters = areaFilters;
    }
    
    public AreaAllocationResult AllocateAreas(
        IEnumerable<AreaAllocationRequest> requestedAreas,
        IEnumerable<AllocatableAreaGroup> areaGroups,
        Graph<int> areaPossibleConnectivity)
    {
        this.requestedAreas = requestedAreas;
        this.areaGroups = areaGroups;
        this.areaPossibleConnectivity = areaPossibleConnectivity;

        Initialize();
        
        AllocateCenters();
        AllocateAreasNearCenter();

        return new() {
            GeneratedAreaIdsByAllocationRequestId = generatedAreaIdsByAllocationRequestId
        };
    }

    #region Debug
    public void DebugVerifyResult() {
        var assignedGeneratedAreaIds = new HashSet<int>();
        foreach (var requestedArea in requestedAreas) {
            if (GetGeneratedAreaId(requestedArea) != null) {
                var generatedAreaId = GetGeneratedAreaId(requestedArea).Value;

                if (assignedGeneratedAreaIds.Contains(generatedAreaId)) {
                    Debug.LogError(
                        "The same generated Area cannot be assigned to multiple " +
                        "requested Areas."
                    );
                } else {
                    assignedGeneratedAreaIds.Add(generatedAreaId);
                }
            } else {
                if (requestedArea.Necessity == NecessityDegree.Required) {
                    Debug.LogError(
                        "requestedArea.GeneratedAreaId must not be null " +
                        "after generation.");
                }
            }
        }

        Debug.Log(
            $"Assigned generated Area ids: "
            + string.Join(", ", assignedGeneratedAreaIds));
    }
    #endregion

    #region Initialize
    private void Initialize() {
        areaGroupsById = areaGroups.ToDictionary(x => x.Id, x => x);
        requestedAreasByGroupId = GetRequestedAreasByGroupId();
        centersByGroupId = GetCentersByGroupId();

        allocatedRequestedAreas = new();
        occupiedGeneratedAreaIds = new();
    }

    private Dictionary<int, List<AreaAllocationRequest>> GetRequestedAreasByGroupId()
    {
        return requestedAreas
            .GroupBy(area => area.AreaGroupId)
            .ToDictionary(group => group.Key, group => group.ToList());
    }

    private int? GetGeneratedAreaId(AreaAllocationRequest allocationRequest) {
        generatedAreaIdsByAllocationRequestId.TryGetValue(
            allocationRequest.Id,
            out var result);
        return result;
    }

    private void SetGeneratedAreaId(AreaAllocationRequest allocationRequest, int value) {
        generatedAreaIdsByAllocationRequestId.Add(allocationRequest.Id, value);
    }

    private Dictionary<int, AreaAllocationRequest> GetCentersByGroupId() {
        return requestedAreasByGroupId
            .ToDictionary(x => x.Key, x => GetGroupCenter(x.Value));
    }

    private AreaAllocationRequest GetGroupCenter(List<AreaAllocationRequest> groupAreas) {
        var maxGroupNecessity = groupAreas.Max(x => x.Necessity);
        return groupAreas
            .Where(x => x.Necessity == maxGroupNecessity)
            .Aggregate(
                (x, y) => CompareIndividualAccessibilities(
                    x.IndividualAccessibility, y.IndividualAccessibility) ? x : y);
    }
    
    private bool CompareIndividualAccessibilities(
        AreaIndividualAccessibility value1,
        AreaIndividualAccessibility value2) {
        return (value1 - value2) > 0;
    }
    #endregion
    
    #region Allocate Central Areas
    private void AllocateCenters() {
        foreach (var (groupId, center) in centersByGroupId) {
            AllocateCentralArea(
                areaPossibleConnectivity,
                center,
                areaGroupsById[groupId]);
        }
    }

    private void AllocateCentralArea(
        Graph<int> AreasConnectivity,
        AreaAllocationRequest requestedArea,
        AllocatableAreaGroup areaGroup) 
    {
        var allocatedAlienatedCenterIds = GetAllocatedAlienatedAreaIds(areaGroup);

        var groupCenter = centersByGroupId[areaGroup.Id];
        var generatedAreaIdsByMaxDistance = OrderAreaIdsByMaxDistance(
            OrderByRequirementsMatch(
                requestedArea,
                AreasConnectivity.Nodes.Select(x => x.Value),
                AreasConnectivity,
                false),
            AreasConnectivity,
            allocatedAlienatedCenterIds);

        GetFirstAndOccupy(
            requestedArea,
            generatedAreaIdsByMaxDistance); 
    }

    /// <summary>
    /// Allocated alienated Area ids for the given Area group
    /// </summary>
    private IEnumerable<int> GetAllocatedAlienatedAreaIds(
        AllocatableAreaGroup areaGroup) =>
        areaGroup.AlienatedGroupIds
            .SelectMany(groupId => requestedAreasByGroupId[groupId])
            .Where(requestedArea => allocatedRequestedAreas.Contains(requestedArea))
            .Select(x => GetGeneratedAreaId(x).Value);

    private IEnumerable<int> OrderAreaIdsByMaxDistance(
        IEnumerable<int> AreaIds,
        Graph<int> AreaConnectivity,
        IEnumerable<int> allocatedAlienatedAreaIds)
    {
        // If allocatedAlienatedAreaIds is empty, we don't consider distances
        if (!allocatedAlienatedAreaIds.Any()) {
            return AreaIds;
        }
        return AreaIds
            .OrderByDescending(generatedAreaId =>
                allocatedAlienatedAreaIds
                    .Select(alienatedId => GetDistance(
                        AreaConnectivity,
                        generatedAreaId,
                        alienatedId)
                        ?? -1)
                        // ?? throw new System.NotSupportedException(
                        //     "Isolated Areas are not fully supported."))
                    .Min());
    }
    #endregion

    #region Allocate Near Center
    private void AllocateAreasNearCenter() {
        foreach (var (groupId, groupAreas) in requestedAreasByGroupId) {
            foreach (var requestedArea in groupAreas) {
                // Central Areas were allocated previously
                if (!allocatedRequestedAreas.Contains(requestedArea)) {
                    AllocateNearCenter(areaPossibleConnectivity, requestedArea);
                }
            }
        }
    }

    private void AllocateNearCenter(
        Graph<int> AreaConnectivity,
        AreaAllocationRequest requestedArea)
    {
        var centralAreaId = GetGeneratedAreaId(centersByGroupId[requestedArea.AreaGroupId])
            .Value;
        var neighborAreaIds = GetNodeByValue(AreaConnectivity, centralAreaId)
            .ConnectedNodes
            .Select(x => x.Value);

        var prioritizedAreaIds = OrderByRequirementsMatch(
            requestedArea,
            neighborAreaIds,
            AreaConnectivity);
        
        GetFirstAndOccupy(requestedArea, prioritizedAreaIds);
    }
    #endregion

    #region Common Allocation Logic
    private void GetFirstAndOccupy(
        AreaAllocationRequest requestedArea,
        IEnumerable<int> areaIds) {
        if (!areaIds.Any()) {
            if (requestedArea.Necessity == NecessityDegree.Required) {
                throw new ComplexGenerationException("Required Area was not allocated");
            } else {
                return;
            }
        }
        
        Occupy(requestedArea, areaIds.First());
    }

    private void Occupy(
        AreaAllocationRequest requestedArea,
        int areaId)
    {
        if (occupiedGeneratedAreaIds.Contains(areaId)) {
            throw new ComplexGenerationException(
                "Cannot assign the same generated Area to multiple requested Areas");
        }
        SetGeneratedAreaId(requestedArea, areaId);
        allocatedRequestedAreas.Add(requestedArea);
        occupiedGeneratedAreaIds.Add(areaId);
    }

    /// <summary>
    /// Orders actually generated Area ids to the base Area-level requirements
    /// of the requested Area (i.e. alienated group requirements aren't considered)
    /// and filters Areas by matching and occupancy
    /// </summary>
    private IEnumerable<int> OrderByRequirementsMatch(
        AreaAllocationRequest requestedArea,
        IEnumerable<int> areaIds,
        Graph<int> areaConnectivity,
        bool includeNotMatching = true)
    {
        return areaIds
            .Select(AreaId =>
                new {
                    AreaId,
                    Match = requestedArea.UseIndividualAccessibility ?
                        EstimateIndividualAccessibilityMatching(
                            requestedArea,
                            GetNodeByValue(areaConnectivity, AreaId).ConnectedNodes.Count)
                        : Random.Range(-1, 2)
                }
            )
            .Where(x => areaFilters.All(filter => filter.ShouldAllocate(x.AreaId)))
            .Where(x => includeNotMatching || x.Match >= 0)
            .Where(x => !occupiedGeneratedAreaIds.Contains(x.AreaId))
            .OrderBy(x => x.Match)
            .ThenBy(x => Random.Range(-1, 2))
            .Select(x => x.AreaId);
    }

    private int EstimateIndividualAccessibilityMatching(
        AreaAllocationRequest requestedArea,
        int neighborsCount)
    {
        return requestedArea.IndividualAccessibility switch
        {
            AreaIndividualAccessibility.High => neighborsCount - 4,
            AreaIndividualAccessibility.Medium => -Mathf.Abs(3 - neighborsCount) + 1,
            AreaIndividualAccessibility.Low => -Mathf.Abs(2 - neighborsCount) + 1,
            AreaIndividualAccessibility.Isolated => -neighborsCount,
            _ => throw new System.ArgumentException(
                $"Unknown {nameof(AreaIndividualAccessibility)} value"),
        };
    }
    #endregion

    #region Graph
    private Node<int> GetNodeByValue(Graph<int> graph, int value) {
        return graph.Nodes.First(node => node.Value == value);
    }

    /// <summary>
    /// Calculates distance between two nodes using BFS.
    /// Null if there is no way
    /// </summary>
    private int? GetDistance(Graph<int> graph, int startId, int targetId)
    {
        var queue = new Queue<int>();
        var visited = new HashSet<int>();
        var distances = new Dictionary<int, int> { { startId, 0 } };

        queue.Enqueue(startId);
        visited.Add(startId);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == targetId)
                return distances[current];

            // Improvement: Node search is not efficient now
            foreach (var neighbor in GetNodeByValue(graph, current).ConnectedNodes.Select(x => x.Value))
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    distances[neighbor] = distances[current] + 1;
                }
            }
        }

        return null;
    }
    #endregion
}
