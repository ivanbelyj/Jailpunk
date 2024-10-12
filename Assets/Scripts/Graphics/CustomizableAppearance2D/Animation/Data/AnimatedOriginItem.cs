using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimatedOriginItem
{
    [Tooltip(
        "Config will be used for the specified state only. "
        + " Can be ignored if the animated origin is configured in this way")]
    public string state;

    [Tooltip(
        "Config will be used for the specified angle only. "
        + " Can be ignored if the animated origin is configured in this way")]
    public int angle;

    public Vector2Int baseOffset;

    [Tooltip("Additional offsets accordingly to frames")]
    [SerializeField]
    private Vector2Int[] offsetsPerFrame;

    public Vector2Int GetOffset(int frame) {
        return offsetsPerFrame.Length == 0 ?
            baseOffset
            : offsetsPerFrame[frame] + baseOffset;
    }
}
