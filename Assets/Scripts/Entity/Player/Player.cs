using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(DeathScreen))]
public class Player : NetworkBehaviour
{
    [SerializeField]
    private DestroyableLifecycle destroyableLifecycle;

    [SerializeField]
    [Tooltip("Child gameObject with components specific to local player only")]
    private GameObject localPlayer;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        destroyableLifecycle.EntityDestroyed += OnDeath;
    }

    private void Start() {
        if (!isLocalPlayer) {
            Destroy(localPlayer);
        }
    }

    private void OnDeath() {
        if (isLocalPlayer) {
            GetComponent<DeathScreen>().Die();
        }
    }
}
