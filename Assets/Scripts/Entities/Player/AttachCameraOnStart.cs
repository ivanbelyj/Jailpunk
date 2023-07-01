using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

/// <summary>
/// Attaches CinemachineVirtualCamera with defined name
/// to GameObject on local machine
/// </summary>
public class AttachCameraOnStart : NetworkBehaviour
{
    [SerializeField]
    private string cameraName = "PlayerVirtualCamera";
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        CinemachineVirtualCamera playerVirtualCamera = GameObject
            .Find(cameraName).GetComponent<CinemachineVirtualCamera>();

        playerVirtualCamera.Follow = transform;
    }
}
