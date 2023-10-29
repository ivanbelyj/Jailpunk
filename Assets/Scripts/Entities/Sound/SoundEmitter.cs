using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(RadiusChecker))]
public class SoundEmitter : MonoBehaviour
{
    private static Dictionary<string, float> attenuationsByObstacleTypes;

    public float defaultSoundIntensity = 10f;

    [SerializeField]
    [Tooltip("Ajusts inverse square law of sound attenuation")]
    private float soundAttenuation = 1f;

    // [SerializeField]
    // private GameObject emitterGO;
    private Dictionary<int, SoundReceiver> receiversByIds;
    private RadiusChecker radiusChecker;

    private void Awake() {
        receiversByIds = new Dictionary<int, SoundReceiver>();
        // if (emitterGO == null)
        //     emitterGO = gameObject;
        radiusChecker = GetComponent<RadiusChecker>();
        radiusChecker.Radius = 100; // Todo: set correct minimal radius
        radiusChecker.NewInRadius += OnRadiusEnter;
        radiusChecker.OutOfRadius += OnRadiusExit;
    }

    private void OnRadiusEnter(List<GameObject> gameObjects) {
        foreach (var go in gameObjects) {
            SoundReceiver receiver = go.GetComponent<SoundReceiver>();
            if (receiver != null) {
                // Debug.Log("new receiver " + go.GetInstanceID());
                receiversByIds.Add(go.GetInstanceID(), receiver);
            }
        }
    }
    private void OnRadiusExit(List<GameObject> gameObjects) {
        foreach (var go in gameObjects) {
            if (go.GetComponent<SoundReceiver>() != null) {
                // Debug.Log("remove receiver " + go.GetInstanceID());
                receiversByIds.Remove(go.GetInstanceID());
            }
        }
    }

    public void Emit(SoundData soundData, float? soundIntensity = null) {
        if (soundIntensity == null) {
            soundIntensity = defaultSoundIntensity;
        }

        Vector3 emitterPos = transform.position;

        foreach (SoundReceiver sr in receiversByIds.Values) {
            Vector3 receiverPos = sr.transform.position;
            float distance = Vector3.Distance(receiverPos, emitterPos);
            float intensity = soundIntensity.Value;

            // Inverse square law
            intensity -= soundAttenuation * distance * distance;
            intensity -= GetWallAttenuation(emitterPos, receiverPos);
            if (intensity < sr.soundThreshold || intensity <= 0f) 
                continue;
            sr.Receive(intensity, emitterPos, soundData);
        }
    }

    private float GetWallAttenuation(Vector3 emitterPos, Vector3 receiverPos) {
        float attenuation = 0f;
        Vector3 dir = (receiverPos - emitterPos).normalized;
        float distance = dir.magnitude;
        RaycastHit[] hits = Physics.RaycastAll(new Ray(emitterPos, dir), distance);
        for (int i = 0; i < hits.Length; i++) {
            string tag = hits[i].collider.gameObject.tag;
            if (attenuationsByObstacleTypes.ContainsKey(tag)) {
                attenuation += attenuationsByObstacleTypes[tag];
            }
        }
        return attenuation;
    }
}
