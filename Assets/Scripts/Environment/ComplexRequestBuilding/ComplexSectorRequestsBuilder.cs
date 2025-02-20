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
        public SectorInfo sectorInfo;
    }

    private List<GroupBuildingConfig> groups = new();
    private List<SectorBuildingConfig> sectors = new();

    private int lastGroupId = 0;
    private int lastSectorId = 0;
    private int lastZoneId = 0;

    public ComplexSectorRequestsBuilder AddGroup(bool alienateAll = false) {
        groups.Add(new() {
            alienateAll = alienateAll,
            allocatableAreaGroup = new() {
                Id = ++lastGroupId
            }
        });

        return this;
    }

    public ComplexSectorRequestsBuilder AddSector(
        AreaIndividualAccessibility? individualAccessibility = null,
        NecessityDegree necessityDegree = NecessityDegree.Desirable)
    {
        var areaGroupId = groups.Last().allocatableAreaGroup.Id;
        sectors.Add(new() {
            sectorInfo = new() {
                SectorRequest = new() {
                    AreaAllocationRequest = new(++lastSectorId, areaGroupId) {
                        UseIndividualAccessibility = individualAccessibility.HasValue,
                        IndividualAccessibility = individualAccessibility ?? default,
                        Necessity = necessityDegree
                    }
                }
            }
        });
        return this;
    }

    public ComplexSectorRequestsBuilder AddZones(int count) {
        const int SingleZoneGroupId = 1;

        var sector = sectors.Last();

        sector.sectorInfo.SectorRequest.ZoneGroups.Add(new() {
            Id = SingleZoneGroupId
        });
        sector.sectorInfo.Zones.AddRange(CreateZones(count, SingleZoneGroupId));

        return this;
    }

    public (List<SectorInfo>, List<AllocatableAreaGroup>) Build() {
        return (BuildSectors(), BuildGroups());
    }

    private List<ZoneInfo> CreateZones(int count, int areaGroupId) {
        var result = new List<ZoneInfo>();
        for (int i = 0; i < count; i++) {
            result.Add(CreateZoneInfo(areaGroupId));
        }
        return result;
    }

    private ZoneInfo CreateZoneInfo(int areaGroupId) {
        return new ZoneInfo() {
            ZoneRequest = new() {
                AreaAllocationRequest = new(++lastZoneId, areaGroupId) {
                    UseIndividualAccessibility = false,
                    Necessity = NecessityDegree.Desirable,
                }
            }
        };
    }

    private List<SectorInfo> BuildSectors() {
        return sectors.Select(x => x.sectorInfo).ToList();
    }

    private List<AllocatableAreaGroup> BuildGroups() {
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
