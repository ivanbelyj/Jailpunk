using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoomer : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float minOrthographicSize = 3f;
    public float maxOrthographicSize = 10f;
    public float lerpSpeed = 0.1f; 

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private float targetOrthographicSize;

    private void Awake()
    {
        targetOrthographicSize = virtualCamera.m_Lens.OrthographicSize;
    }

    private void LateUpdate()
    {
        float currentScroll = Mouse.current.scroll.ReadValue().y;
        
        targetOrthographicSize -= currentScroll * zoomSpeed * Time.deltaTime;
        
        targetOrthographicSize = Mathf.Clamp(
            targetOrthographicSize,
            minOrthographicSize,
            maxOrthographicSize);
        
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
            virtualCamera.m_Lens.OrthographicSize,
            targetOrthographicSize,
            lerpSpeed);
    }
}
