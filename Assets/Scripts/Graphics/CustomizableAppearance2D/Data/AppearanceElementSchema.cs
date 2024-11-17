using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AppearanceElementSchema
{
    public string name;

    [Tooltip(
        "Set to true for appearance elements that have no special sprites drawn "
        + "for different states")]
    public bool ignoreState;

    [Tooltip(
        "Set to true for appearance elements that have no special sprites drawn "
        + "for different angles")]
    public bool ignoreAngle;

    [Tooltip("Animate element using origin configuration")]
    public bool useOrigin;

    [Tooltip("Define if " + nameof(useOrigin) + " is set")]
    public string originName;

    [Tooltip("Elements with higher values are displayed above elements " +
        "with lower values. Applied to element for angles " +
        "betwen [135; 225], applied and inverted for [-45; 45] (all inclusive)")]
    public int orderWhenVerticalAngle;

    [Tooltip("Ignore invert sorting order for [-45; 45]")]
    public bool ignoreInvertForTopAngle;

    [Tooltip("Elements with higher values are displayed above elements " +
        "with lower values. Applied to element for angles " +
        "betwen (45; 135), applied and inverted for (225; 315) (all exclusive)")]
    public int orderWhenHorizontalAngle;

    [Tooltip("Use defined stop-frames of walk instead of idle animation")]
    public bool useStopFrames;

    [Tooltip("Define if " + nameof(useStopFrames) + " is set")]
    public string stopFramesSet;
}
