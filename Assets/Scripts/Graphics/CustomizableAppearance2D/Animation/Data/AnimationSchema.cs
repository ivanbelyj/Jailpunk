using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct AnimationSchema
{
    [SerializeField]
    private AnimationStateSchema[] stateSchemas;
    private Dictionary<string, AnimationStateSchema> schemasByState;

    public AnimationStateSchema GetStateSchema(string state) {
        schemasByState ??= stateSchemas.ToDictionary(x => x.state, x => x);
        return schemasByState[state];
    }

    [SerializeField]
    private AnimatedOrigin[] origins;
    private Dictionary<string, AnimatedOrigin> originsByName;

    public AnimatedOrigin GetOriginByName(string name) {
        originsByName ??= origins.ToDictionary(x => x.name, x => x);
        return originsByName[name];
    }
}
