using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
using System;

public class PlayerCameraManager : NetworkBehaviour
{
    [SerializeField]
    private string cameraName = "PlayerVirtualCamera";

    [SerializeField]
    private Vector3 cameraOffset;

    public CinemachineVirtualCamera PlayerVirtualCamera { get; private set; }

    public event Action<CinemachineVirtualCamera> CameraAttached;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        PlayerVirtualCamera = GetCamera();
        PlayerVirtualCamera.Follow = InstantiateChildToFollow();
        PlayerVirtualCamera.GetComponent<CameraZoomer>().ResetZoomOut();

        CameraAttached?.Invoke(PlayerVirtualCamera);
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
