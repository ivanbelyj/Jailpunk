using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceSortingOrderHelper
{
    private const int AngleTopRight = 45;
    private const int AngleBottomRight = 135;
    private const int AngleBottomLeft = 225;
    private const int AngleTopLeft = 315;

    private readonly AppearanceElementSchema elementSchema;

    public AppearanceSortingOrderHelper(AppearanceElementSchema elementSchema)
    {
        this.elementSchema = elementSchema;
    }

    public int GetSortingOrder(int angle) {
        // Temporary fix )
        return angle switch {
            0 => elementSchema.orderWhenVerticalAngle
                * (elementSchema.ignoreInvertForTopAngle ? 1 : -1),
            180 => elementSchema.orderWhenVerticalAngle,
            _ => elementSchema.orderWhenHorizontalAngle
        };

        // Todo: possibly has a bug
        // bool isInTopQuarter = IsInArc(angle, AngleTopLeft, AngleTopRight);
        // bool isInBottomQuarter = IsInArc(angle, AngleBottomRight, AngleBottomLeft);

        // bool shouldInvert = isInTopQuarter;

        // int sortingOrder = isInTopQuarter || isInBottomQuarter
        //     ? elementSchema.orderWhenVerticalAngle
        //     : elementSchema.orderWhenHorizontalAngle;
        
        // return shouldInvert ? -sortingOrder : sortingOrder;
    }

    private static int To360(int angle)
        => (angle < 0 ? angle + 360 : angle) % 360;

    private static bool IsInArc(int angle, int arcStart, int arcEnd) {
        angle = To360(angle);
        arcStart = To360(arcStart);
        arcEnd = To360(arcEnd);

        if (arcStart > arcEnd) {
            return IsInArc(angle, arcStart, 359) && IsInArc(angle, 0, arcEnd);
        }
        return angle >= arcStart && angle <= arcEnd;
    }
}
