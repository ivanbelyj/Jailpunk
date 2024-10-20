using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DeathScreen : MonoBehaviour
{
    private DynamicVolumeManager volumeManager;

    [SerializeField]
    private CanvasGroup deathPanel;

    [SerializeField]
    private string deathPanelTag = "DeathPanel";

    [SerializeField]
    private Color deathScreenColorFilter = Color.red;

    [SerializeField]
    private float transitionDuration = 1f;

    [SerializeField]
    private float panelFadeInDuration = 1f;

    [SerializeField]
    private float panelFadeInDelay = 1f;

    [SerializeField]
    private float zoomOutTargetSize = 20f;

    [SerializeField]
    private float zoomOutSpeed = 0.5f;

    [SerializeField]
    private PlayerCameraManager playerCameraManager;

    private void Awake()
    {
        volumeManager = FindObjectOfType<DynamicVolumeManager>();
        deathPanel = GameObject.FindWithTag(deathPanelTag).GetComponent<CanvasGroup>();
        if (deathPanel == null) {
            Debug.LogError("Death panel not found by tag: " + deathPanelTag);
        }
        
        DisableDeathPanel();
    }

    private void DisableDeathPanel() {
        if (deathPanel != null)
        {
            deathPanel.alpha = 0f;
            deathPanel.interactable = false;
            deathPanel.blocksRaycasts = false;
        }
    }

    public void Die()
    {
        StartCoroutine(TransitionToDeathColor());
        StartCoroutine(FadeInDeathPanel());
        ZoomOut();
    }

    private void ZoomOut() {
        var cameraZoomer = playerCameraManager.PlayerVirtualCamera.GetComponent<CameraZoomer>();
        cameraZoomer?.ZoomOut(zoomOutTargetSize, zoomOutSpeed);
    }

    protected IEnumerator TransitionToDeathColor()
    {
        var colorAdjustments = volumeManager.GetVolumeComponent<ColorAdjustments>();
        colorAdjustments.colorFilter.overrideState = true;

        Color initialColor = colorAdjustments.colorFilter.value;

        yield return SmoothTransitionUtils.SmoothTransition(0, 1, transitionDuration,
            (newValue) => {
                colorAdjustments.colorFilter.value =
                    Color.Lerp(initialColor, deathScreenColorFilter, newValue);
            });
    }

    protected IEnumerator FadeInDeathPanel()
    {
        if (deathPanel == null) yield break;
        yield return new WaitForSeconds(panelFadeInDelay);

        deathPanel.interactable = true;
        deathPanel.blocksRaycasts = true;

        float startAlpha = deathPanel.alpha;

        yield return SmoothTransitionUtils.SmoothTransition(
            startAlpha,
            1f,
            panelFadeInDuration,
            (newValue) => {
                deathPanel.alpha = newValue;
            });

        deathPanel.alpha = 1f;
    }
}
