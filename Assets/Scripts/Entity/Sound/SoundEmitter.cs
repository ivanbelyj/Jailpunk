using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// [RequireComponent(typeof(RadiusChecker))]
public class SoundEmitter : MonoBehaviour, ISoundEmitter
{
    private static Dictionary<string, float> attenuationsByObstacleTypes =
        new Dictionary<string, float>();

    private float soundIntensity = 10f;
    public float SoundIntensity {
        get => soundIntensity;
        set {
            soundIntensity = value;
            SetRadiusByIntensity();
        }
    }

    [SerializeField]
    [Tooltip("Ajusts inverse square law of sound attenuation")]
    private float soundAttenuation = 1f;

    // [SerializeField]
    // private GameObject emitterGO;
    private Dictionary<int, SoundReceiver> receiversByIds;
    [SerializeField]
    private RadiusChecker radiusChecker;
    public RadiusChecker RadiusChecker => radiusChecker;

    private void Awake() {
        receiversByIds = new Dictionary<int, SoundReceiver>();
        // if (emitterGO == null)
        //     emitterGO = gameObject;
        // radiusChecker = GetComponent<RadiusChecker>();
        SetRadiusByIntensity();
        radiusChecker.NewInRadius += OnRadiusEnter;
        radiusChecker.OutOfRadius += OnRadiusExit;
    }

    public void Emit(SoundData soundData) {
        Vector3 emitterPos = transform.position;
        GetReceiversAndIntensities().ForEach(x
            => {
                var receiver = x.Item1;
                var intensity = x.Item2;
                if (intensity >= receiver.soundThreshold)
                    receiver.Receive(intensity, emitterPos, soundData);
            });
    }

    /// <summary>
    /// Gets all receivers that can receive sound from current
    /// emitter configuration.
    /// Ignores individual receiver's sound threshold
    /// </summary>
    /// <returns></returns>
    public List<(SoundReceiver, float)> GetReceiversAndIntensities() {
        var res = new List<(SoundReceiver, float)>();
        Vector2 emitterPos = transform.position;

        foreach (SoundReceiver sr in receiversByIds.Values) {
            Vector2 receiverPos = sr.transform.position;
            float distance = Vector2.Distance(receiverPos, emitterPos);
            float intensity = soundIntensity;

            // Inverse square law
            intensity -= soundAttenuation * distance * distance;
            intensity -= GetWallAttenuation(emitterPos, receiverPos);
            if (intensity <= 0f) 
                continue;
            res.Add((sr, intensity));
        }
        return res;
    }

    private void SetRadiusByIntensity() {
        radiusChecker.Radius = Mathf.Sqrt(soundIntensity / soundAttenuation);
    }

    private void OnRadiusEnter(List<GameObject> gameObjects) {
        int receiversCounter = 0;
        foreach (var go in gameObjects) {
            SoundReceiver receiver = go.GetComponent<SoundReceiver>();
            if (receiver != null) {
                receiversCounter++;
                // Debug.Log("new receiver " + go.GetInstanceID());
                receiversByIds.Add(go.GetInstanceID(), receiver);
            }
        }
        // Debug.Log("Radius enter! " + receiversCounter + " new receivers");
    }
    private void OnRadiusExit(List<GameObject> gameObjects) {
        int receiversCounter = 0;
        foreach (var go in gameObjects) {
            if (go.GetComponent<SoundReceiver>() != null) {
                receiversCounter++;
                // Debug.Log("remove receiver " + go.GetInstanceID());
                receiversByIds.Remove(go.GetInstanceID());
            }
        }
        // Debug.Log("Radius exit! " + receiversCounter + " exited receivers");
    }

    private float GetWallAttenuation(Vector2 emitterPos, Vector2 receiverPos) {
        float attenuation = 0f;
        Vector2 dir = (receiverPos - emitterPos).normalized;
        float distance = dir.magnitude;
        RaycastHit2D[] hits = Physics2D.RaycastAll(emitterPos, dir, distance);
        for (int i = 0; i < hits.Length; i++) {
            string tag = hits[i].collider.gameObject.tag;
            if (attenuationsByObstacleTypes.ContainsKey(tag)) {
                attenuation += attenuationsByObstacleTypes[tag];
            }
        }
        return attenuation;
    }
}
