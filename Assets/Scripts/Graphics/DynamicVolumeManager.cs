using Mirror;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Manages dynamically instantiating postprocessing volumes, applied
/// for local player effects
/// </summary>
public class DynamicVolumeManager : NetworkBehaviour
{
    public Volume Volume { get; private set; }
    public VolumeProfile VolumeProfile { get; private set; }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log("DynamicVolumeManager on start local player");
        InstantiateVolume();
    }

    public override void OnStopLocalPlayer()
    {
        base.OnStopLocalPlayer();
        DestroyVolume();
    }

    private void InstantiateVolume() {
        GameObject volumeGameObject = new("DynamicVolume");
        Volume = volumeGameObject.AddComponent<Volume>();
        Volume.isGlobal = true;
        VolumeProfile = ScriptableObject.CreateInstance<VolumeProfile>();
        Volume.profile = VolumeProfile;
    }

    private void DestroyVolume() {
        Destroy(Volume.gameObject);
    }

    public TVolumeComponent GetVolumeComponent<TVolumeComponent>()
        where TVolumeComponent : VolumeComponent {
        if (!VolumeProfile.TryGet(out TVolumeComponent volumeComponent))
        {
            volumeComponent = VolumeProfile.Add<TVolumeComponent>(false);
        }
        return volumeComponent;
    }
}
