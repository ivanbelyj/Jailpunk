using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

[Serializable]
public class AnimatedOrigin
{
    public string name;
    public bool ignoreState;
    public bool ignoreAngle;

    [SerializeField]
    private AnimatedOriginItem[] items;

    private Dictionary<(string state, int angle), AnimatedOriginItem> itemsDictionary;

    /// <param name="state">Null for origins ignoring state</param>
    /// <param name="angle">Null for origins ingoring angle</param>
    public AnimatedOriginItem GetItem(string state = null, int angle = 0) {
        if (itemsDictionary == null) {
            SetItemsDictionary();   
        }

        var key = GetKey(state, angle);

        if (itemsDictionary.TryGetValue(key, out var item)) {
            return item;
        } else {
            // Debug.LogWarning($"Appearance animated origin not found. "
            //     + $"state: {state}, angle: {angle}");
            return null;
        }
    }

    private void SetItemsDictionary() {
        itemsDictionary = items.ToDictionary(
            x => GetKey(x.state, x.angle),
            x => x);
    }

    private (string, int) GetKey(string state, int angle) {
        if (ignoreState) {
            state = null;
        }
        if (ignoreAngle) {
            angle = 0;
        }
        return (state, angle);
    }   
}
