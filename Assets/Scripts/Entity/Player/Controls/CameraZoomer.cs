using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoomer : MonoBehaviour
{
    public float scrollZoomSpeed = 2f;
    public float minScrollOrthographicSize = 3f;
    public float maxScrollOrthographicSize = 10f;
    public float defaultOrthographicSize = 5f;
    public float scrollLerpSpeed = 0.1f; 

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private float targetOrthographicSize;

    private Coroutine zoomOutCoroutine;

    private void Awake()
    {
        targetOrthographicSize = virtualCamera.m_Lens.OrthographicSize;
    }

    private void LateUpdate()
    {
        if (zoomOutCoroutine == null) {
            ZoomByScroll();
        }
    }

    private void ZoomByScroll() {
        float currentScroll = Mouse.current.scroll.ReadValue().y;
        
        targetOrthographicSize -= currentScroll * scrollZoomSpeed * Time.deltaTime;
        
        targetOrthographicSize = Mathf.Clamp(
            targetOrthographicSize,
            minScrollOrthographicSize,
            maxScrollOrthographicSize);
        
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
            virtualCamera.m_Lens.OrthographicSize,
            targetOrthographicSize,
            scrollLerpSpeed);
    }

    public void ResetZoomOut() {
        StopZoomOut();
        zoomOutCoroutine = null;
        virtualCamera.m_Lens.OrthographicSize = defaultOrthographicSize;
    }

    private void StopZoomOut() {
        if (zoomOutCoroutine != null)
        {
            StopCoroutine(zoomOutCoroutine);
        }
    }

    public void ZoomOut(float zoomOutTargetSize, float zoomOutSpeed)
    {
        StopZoomOut();

        var startSize = virtualCamera.m_Lens.OrthographicSize;
        var duration = Mathf.Abs(zoomOutTargetSize - startSize) / zoomOutSpeed;
        zoomOutCoroutine = StartCoroutine(SmoothTransitionUtils.SmoothTransition(
            startSize,
            zoomOutTargetSize,
            duration,
            newValue => {
                virtualCamera.m_Lens.OrthographicSize = newValue;
            }
        ));
    }
}
