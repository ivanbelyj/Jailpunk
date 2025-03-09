using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComplexSectorRequestsBuilder
{
    class GroupBuildingConfig {
        public bool alienateAll = false;

        public AllocatableAreaGroup allocatableAreaGroup;
    }

    class SectorBuildingConfig {
        public SectorRequest sectorRequest;
        public List<GroupBuildingConfig> zoneGroups = new();
    }

    private List<GroupBuildingConfig> sectorGroups = new();
    private List<SectorBuildingConfig> sectors = new();

    private int lastGroupId = 0;
    private int lastSectorId = 0;
    private int lastZoneId = 0;

    public ComplexSectorRequestsBuilder AddSectorGroup(bool alienateAll = false) {
        AddGroup(sectorGroups, alienateAll);

        return this;
    }

    public ComplexSectorRequestsBuilder AddSector(
        AreaIndividualAccessibility? individualAccessibility = null,
        NecessityDegree necessityDegree = NecessityDegree.Desirable)
    {
        if (sectorGroups.Count == 0) {
            AddSectorGroup();
        }
        var areaGroupId = sectorGroups.Last().allocatableAreaGroup.Id;
        sectors.Add(new() {
            sectorRequest = new() {
                // TODO: implement
                SectorPlanningInfo = new() {
                    SectorGenerationSchemaId = "RoomsAndCorridorsSector"
                },
                AreaAllocationRequest = new(++lastSectorId, areaGroupId) {
                    UseIndividualAccessibility = individualAccessibility.HasValue,
                    IndividualAccessibility = individualAccessibility ?? default,
                    Necessity = necessityDegree
                }
            }
        });
        return this;
    }

    public ComplexSectorRequestsBuilder AddZoneGroup(bool alienateAll = false) {
        var lastSector = sectors.Last();
        AddGroup(lastSector.zoneGroups, alienateAll);

        return this;
    }

    public ComplexSectorRequestsBuilder AddZones(int count) {
        var lastSector = sectors.Last();

        if (lastSector.zoneGroups.Count == 0) {
            AddZoneGroup(false);
        }

        var areaGroupId = lastSector.zoneGroups.Last().allocatableAreaGroup.Id;

        lastSector.sectorRequest.Zones.AddRange(CreateZones(count, areaGroupId));

        return this;
    }

    public (List<SectorRequest>, List<AllocatableAreaGroup>) Build() {
        return (BuildSectors(), BuildGroups(sectorGroups));
    }

    private void AddGroup(
        List<GroupBuildingConfig> sectorGroups,
        bool alienateAll = false)
    {
        sectorGroups.Add(new() {
            alienateAll = alienateAll,
            allocatableAreaGroup = new() {
                Id = ++lastGroupId
            }
        });
    }

    private List<ZoneRequest> CreateZones(int count, int areaGroupId) {
        var result = new List<ZoneRequest>();
        for (int i = 0; i < count; i++) {
            result.Add(CreateZoneInfo(areaGroupId));
        }
        return result;
    }

    private ZoneRequest CreateZoneInfo(int areaGroupId) {
        return new() {
            AreaAllocationRequest = new(++lastZoneId, areaGroupId) {
                UseIndividualAccessibility = false,
                Necessity = NecessityDegree.Desirable,
            },
            // TODO: implement
            ZoneFillingInfo = new() {
                ZoneGenerationSchemaId = "EmptyZone"
            }
        };
    }

    private List<SectorRequest> BuildSectors() {
        return sectors
            .Select(x => {
                x.sectorRequest.ZoneGroups = BuildGroups(x.zoneGroups);
                return x.sectorRequest;
            })
            .ToList();
    }

    private List<AllocatableAreaGroup> BuildGroups(List<GroupBuildingConfig> groups) {
        return groups.Select(x => {
            if (x.alienateAll) {
                x.allocatableAreaGroup.AlienatedGroupIds =
                    groups
                        .Select(y => y.allocatableAreaGroup.Id)
                        .Where(id => id != x.allocatableAreaGroup.Id)
                        .ToArray();
            }
            return x.allocatableAreaGroup;
        }).ToList();
    }
}
