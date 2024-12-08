using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RequestedSectorsAllocation : GenerationStage
{
    private Dictionary<int, SectorGroup> sectorGroupsById;
    private Dictionary<int, List<RequestedSectorInfo>> requestedSectorsByGroupId;
    private Dictionary<int, RequestedSectorInfo> centersByGroupId;

    private HashSet<RequestedSectorInfo> allocatedRequestedSectors;
    private HashSet<int> occupiedGeneratedSectorIds;
    
    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        Initialize(context);
        
        AllocateCenters(context);
        AllocateSectorsNearCenter(context);

        HandleDebugAndVerifyResult(context);

        return context;
    }

    #region Debug
    private void HandleDebugAndVerifyResult(GenerationContext context) {
        AddSectorDebugColors(context);
    }

    private void AddSectorDebugColors(GenerationContext context) {
        var assignedGeneratedSectorIds = new HashSet<int>();
        foreach (var requestedSector in context.Request.RequestedSectors) {
            if (requestedSector.GeneratedSectorId != null) {
                var generatedSectorId = requestedSector.GeneratedSectorId.Value;
                MazeGenerator.AddDebugSectorColor(generatedSectorId, Color.red);

                if (assignedGeneratedSectorIds.Contains(generatedSectorId)) {
                    Debug.LogError(
                        "The same generated sector cannot be assigned to multiple " +
                        "requested sectors."
                    );
                } else {
                    assignedGeneratedSectorIds.Add(generatedSectorId);
                }
            } else {
                if (requestedSector.Necessity == NecessityDegree.Required) {
                    Debug.LogError(
                        "requestedSector.GeneratedSectorId must not be null " +
                        "after generation.");
                }
            }
        }

        Debug.Log(
            $"Assigned generated sector ids: "
            + string.Join(", ", assignedGeneratedSectorIds));

        Debug.Log("Distance between 5 and 7: " + GetDistance(context.PossibleConnectivity, 5, 7));
    }
    #endregion

    #region Initialize
    private void Initialize(GenerationContext context) {
        sectorGroupsById = context.Request.SectorGroups.ToDictionary(x => x.Id, x => x);
        requestedSectorsByGroupId = GetRequestedSectorsByGroupId(context);
        centersByGroupId = GetCentersByGroupId();

        allocatedRequestedSectors = new();
        occupiedGeneratedSectorIds = new();
    }

    private Dictionary<int, List<RequestedSectorInfo>> GetRequestedSectorsByGroupId(GenerationContext context)
    {
        return context.Request.RequestedSectors
            .GroupBy(sector => sector.SectorGroupId)
            .ToDictionary(group => group.Key, group => group.ToList());
    }

    private Dictionary<int, RequestedSectorInfo> GetCentersByGroupId() {
        return requestedSectorsByGroupId
            .ToDictionary(x => x.Key, x => GetGroupCenter(x.Value));
    }

    private RequestedSectorInfo GetGroupCenter(List<RequestedSectorInfo> groupSectors) {
        var maxGroupNecessity = groupSectors.Max(x => x.Necessity);
        return groupSectors
            .Where(x => x.Necessity == maxGroupNecessity)
            .Aggregate(
                (x, y) => CompareIndividualAccessibilities(
                    x.IndividualAccessibility, y.IndividualAccessibility) ? x : y);
    }
    
    private bool CompareIndividualAccessibilities(
        SectorIndividualAccessibility value1,
        SectorIndividualAccessibility value2) {
        return (value1 - value2) > 0;
    }
    #endregion
    
    #region Allocate Central Sectors
    private void AllocateCenters(GenerationContext context) {
        foreach (var (groupId, center) in centersByGroupId) {
            AllocateCentralSector(
                context.PossibleConnectivity,
                center,
                sectorGroupsById[groupId]);
        }
    }

    private void AllocateCentralSector(
        Graph<int> sectorsConnectivity,
        RequestedSectorInfo requestedSector,
        SectorGroup sectorGroup) 
    {
        var allocatedAlienatedCenterIds = GetAllocatedAlienatedSectorIds(sectorGroup);
        // Debug message
        // Debug.Log("alienated sector ids (allocated): " + string.Join(", ", allocatedAlienatedCenterIds));

        var groupCenter = centersByGroupId[sectorGroup.Id];
        var generatedSectorIdsByMaxDistance = OrderSectorIdsByMaxDistance(
            OrderByRequirementsMatch(
                requestedSector,
                sectorsConnectivity.Nodes.Select(x => x.Value),
                sectorsConnectivity,
                false),
            sectorsConnectivity,
            allocatedAlienatedCenterIds);
        
        // Debug message
        // if (allocatedAlienatedCenterIds.Any())
        //     Debug.Log(string.Join(", ",
        //         generatedSectorIdsByMaxDistance.Select(
        //             x => GetDistance(sectorsConnectivity,
        //                 allocatedAlienatedCenterIds.First(), x))));
        // else
        //     Debug.LogWarning("NO ALLOCATED REQESTED SECTORS");

        GetFirstAndOccupy(
            requestedSector,
            generatedSectorIdsByMaxDistance); 
    }

    /// <summary>
    /// Allocated alienated sector ids for the given sector group
    /// </summary>
    private IEnumerable<int> GetAllocatedAlienatedSectorIds(
        SectorGroup sectorGroup) =>
        sectorGroup.AlienatedGroupIds
            .SelectMany(groupId => requestedSectorsByGroupId[groupId])
            .Where(requestedSector => allocatedRequestedSectors.Contains(requestedSector))
            .Select(x => x.GeneratedSectorId.Value);

    private IEnumerable<int> OrderSectorIdsByMaxDistance(
        IEnumerable<int> sectorIds,
        Graph<int> sectorConnectivity,
        IEnumerable<int> allocatedAlienatedSectorIds)
    {
        // If allocatedAlienatedSectorIds is empty, we don't consider distances
        if (!allocatedAlienatedSectorIds.Any()) {
            return sectorIds;
        }
        return sectorIds
            .OrderByDescending(generatedSectorId =>
                allocatedAlienatedSectorIds
                    .Select(alienatedId => GetDistance(
                        sectorConnectivity,
                        generatedSectorId,
                        alienatedId)
                        ?? throw new System.NotSupportedException(
                            "Isolated sectors are not fully supported."))
                    .Min());
    }
    #endregion

    #region Allocate Near Center
    private void AllocateSectorsNearCenter(GenerationContext context) {
        foreach (var (groupId, groupSectors) in requestedSectorsByGroupId) {
            foreach (var requestedSector in groupSectors) {
                // Central sectors were allocated previously
                if (!allocatedRequestedSectors.Contains(requestedSector)) {
                    AllocateNearCenter(context.PossibleConnectivity, requestedSector);
                }
            }
        }
    }

    private void AllocateNearCenter(
        Graph<int> sectorConnectivity,
        RequestedSectorInfo requestedSector)
    {
        var centralSectorId = centersByGroupId[requestedSector.SectorGroupId]
            .GeneratedSectorId
            .Value;
        var neighborSectorIds = GetNodeByValue(sectorConnectivity, centralSectorId)
            .ConnectedNodes
            .Select(x => x.Value);

        var prioritizedSectorIds = OrderByRequirementsMatch(
            requestedSector,
            neighborSectorIds,
            sectorConnectivity);
        
        GetFirstAndOccupy(requestedSector, prioritizedSectorIds);
    }
    #endregion

    #region Common Allocation Logic
    private void GetFirstAndOccupy(
        RequestedSectorInfo requestedSector,
        IEnumerable<int> sectorIds) {
        if (!sectorIds.Any()) {
            if (requestedSector.Necessity == NecessityDegree.Required) {
                throw new MazeGenerationException("Required sector was not allocated");
            } else {
                return;
            }
        }
        
        Occupy(requestedSector, sectorIds.First());
    }

    private void Occupy(
        RequestedSectorInfo requestedSector,
        int sectorId) {
        if (occupiedGeneratedSectorIds.Contains(sectorId)) {
            throw new MazeGenerationException(
                "Cannot assign the same generated sector to multiple requested sectors");
        }
        requestedSector.AssignGeneratedSectorId(sectorId);
        allocatedRequestedSectors.Add(requestedSector);
        occupiedGeneratedSectorIds.Add(sectorId);
    }

    /// <summary>
    /// Orders actually generated sector ids to the base sector-level requirements
    /// of the requested sector (i.e. alienated group requirements aren't considered)
    /// and filters sectors by matching and occupancy
    /// </summary>
    private IEnumerable<int> OrderByRequirementsMatch(
        RequestedSectorInfo requestedSector,
        IEnumerable<int> sectorIds,
        Graph<int> sectorsConnectivity,
        bool includeNotMatching = true
    ) {
        return sectorIds
            .Select(sectorId =>
                new {
                    SectorId = sectorId,
                    Match = requestedSector.UseIndividualAccessibility ?
                        EstimateIndividualAccessibilityMatching(
                            requestedSector,
                            GetNodeByValue(sectorsConnectivity, sectorId).ConnectedNodes.Count)
                        : Random.Range(-1, 2)
                }
            )
            .Where(x => includeNotMatching || x.Match >= 0)
            .Where(x => !occupiedGeneratedSectorIds.Contains(x.SectorId))
            .OrderBy(x => x.Match)
            .ThenBy(x => Random.Range(-1, 2))
            .Select(x => x.SectorId);
    }

    private int EstimateIndividualAccessibilityMatching(
        RequestedSectorInfo requestedSector,
        int neighborsCount
    ) {
        return requestedSector.IndividualAccessibility switch
        {
            SectorIndividualAccessibility.High => neighborsCount - 4,
            SectorIndividualAccessibility.Medium => -Mathf.Abs(3 - neighborsCount) + 1,
            SectorIndividualAccessibility.Low => -Mathf.Abs(2 - neighborsCount) + 1,
            SectorIndividualAccessibility.Isolated => -neighborsCount,
            _ => throw new System.ArgumentException(
                $"Unknown {nameof(SectorIndividualAccessibility)} value"),
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
