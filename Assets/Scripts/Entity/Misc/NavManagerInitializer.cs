using UnityEngine;
using TigerForge;

[RequireComponent(typeof(NavManager))]
public class NavManagerInitializer : MonoBehaviour
{
    private void Awake()
    {
        EventManager.StartListening(EventConstants.GridReady, InitializeNavManager);
    }

    private void InitializeNavManager() {
        var eventData = (GridInstantiatedEventData)EventManager.GetData(
            EventConstants.GridReady);
        GetComponent<NavManager>().Initialize(
            eventData.InstantiatedComplexData.VisionObstacleTilemap);
    }
}
