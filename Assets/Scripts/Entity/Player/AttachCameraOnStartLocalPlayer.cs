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

    [SerializeField]
    private Vector3 cameraOffset;

    public event Action<CinemachineVirtualCamera> CameraAttached;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        CinemachineVirtualCamera playerVirtualCamera = GetCamera();

        playerVirtualCamera.Follow = InstantiateChildToFollow();

        CameraAttached?.Invoke(playerVirtualCamera);
    }

    private CinemachineVirtualCamera GetCamera() => 
        GameObject
            .Find(cameraName)
            .GetComponent<CinemachineVirtualCamera>();

    private Transform InstantiateChildToFollow() {
        GameObject childObject = new GameObject("CameraFollow");
        childObject.transform.SetParent(transform, false);
        childObject.transform.localPosition = cameraOffset;
        return childObject.transform;
    }
}
