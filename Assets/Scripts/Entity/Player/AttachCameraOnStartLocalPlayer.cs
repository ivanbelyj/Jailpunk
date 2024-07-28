using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
using System;

/// <summary>
/// Attaches CinemachineVirtualCamera with defined name
/// to GameObject on local machine
/// </summary>
public class AttachCameraOnStartLocalPlayer : NetworkBehaviour
{
    [SerializeField]
    private string cameraName = "PlayerVirtualCamera";

    public event Action<CinemachineVirtualCamera> CameraAttached;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        CinemachineVirtualCamera playerVirtualCamera = GameObject
            .Find(cameraName)
            .GetComponent<CinemachineVirtualCamera>();

        playerVirtualCamera.Follow = transform;

        CameraAttached?.Invoke(playerVirtualCamera);
    }
}
