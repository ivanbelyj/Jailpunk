using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Appearance Schema",
    menuName = "Customizable Appearance 2D/Appearance Schema",
    order = 52)]
public class AppearanceSchema : ScriptableObject
{
    public bool useFlipped90For270 = true;
    public AnimationSchema animationSchema;

    #region Stop Frames
    [SerializeField]
    private StopFramesSet[] stopFrameSets;
    private Dictionary<string, StopFramesSet> stopFrameSetsByName;
    public StopFramesSet GetStopFramesSetByName(string name) {
        stopFrameSetsByName ??= stopFrameSets.ToDictionary(
            x => x.name,
            x => x);
        return stopFrameSetsByName[name];
    }
    #endregion


    #region Element schemas
    [SerializeField]
    private AppearanceElementSchema[] elements;
    public AppearanceElementSchema[] Elements => elements;
    private Dictionary<string, AppearanceElementSchema> elementsByName;

    public AppearanceElementSchema GetElementSchemaByName(string name) {
        elementsByName ??= elements.ToDictionary(x => x.name, x => x);
        return elementsByName[name];
    }
    #endregion
}
