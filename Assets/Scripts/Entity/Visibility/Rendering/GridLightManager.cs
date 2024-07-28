using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LightSourceEventHandler(
    object sender,
    GridLightSource lightSource
);

public class GridLightManager : MonoBehaviour
{
    public event LightSourceEventHandler LightSourceAdded;
    public event LightSourceEventHandler LightSourceRemoved;

    public void AddLightSource(GridLightSource lightSource) {
        LightSourceAdded?.Invoke(this, lightSource);
    }

    public void RemoveLightSource(GridLightSource lightSource) {
        LightSourceRemoved?.Invoke(this, lightSource);
    }
}
