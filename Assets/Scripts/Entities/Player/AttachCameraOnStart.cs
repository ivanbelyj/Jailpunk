using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

/// <summary>
/// Attaches CinemachineVirtualCamera with defined tag
/// to GameObject on local machine
/// </summary>
public class AttachCameraOnStart : NetworkBehaviour
{
    [SerializeField]
    private string cameraTag = "PlayerVirtualCamera";
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        CinemachineVirtualCamera playerVirtualCamera = GameObject
            .FindWithTag(cameraTag).GetComponent<CinemachineVirtualCamera>();

        playerVirtualCamera.Follow = transform;
    }
}
