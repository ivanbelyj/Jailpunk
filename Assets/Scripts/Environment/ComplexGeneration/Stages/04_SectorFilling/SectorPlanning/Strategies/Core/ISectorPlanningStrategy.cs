using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Strategies we could need
// 1. Different types of Glade. Useful for a complex with safe zone
// 2. Sector with linear or tree-like passage through rooms. Relevant for sectors
// that have not too many neighbor sectors
// 3. Sector with central zone. 1 central and 4 neighbor zones in a simple case.
// Relevant for Glade or for Control sector and maybe for some sectors with
// specific purpose

// Maybe we need sectors with some main region (an open staff base, for example).
// Related feature - use not just react areas, but rects with one excluded rects

public interface ISectorPlanningStrategy
{
    int Id { get; }
    List<SchemeArea> PlanSector(GeneratedSectorInfo sector, GenerationContext context);
}
