using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(DeathScreen))]
public class Player : NetworkBehaviour
{
    [SerializeField]
    private DestroyableLifecycle destroyableLifecycle;

    private DynamicVolumeManager volumeManager;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        destroyableLifecycle.EntityDestroyed += OnDeath;
        volumeManager = FindObjectOfType<DynamicVolumeManager>();
    }

    private void OnDeath() {
        if (isLocalPlayer) {
            GetComponent<DeathScreen>().Die();
        }
    }
}
