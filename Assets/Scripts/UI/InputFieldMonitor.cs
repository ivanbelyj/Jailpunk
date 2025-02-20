using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Monitors current input field and blocks player's keyboard controls
/// </summary>
public class InputFieldMonitor : MonoBehaviour
{
    private PlayerControls playerControls;

    private void Update()
    {
        if (playerControls == null)
        {
            playerControls = FindAnyObjectByType<PlayerControls>();

            if (playerControls == null)
            {
                return;
            }
        }

        UpdatePlayerControlsInputMode();
        UpdatePlayerControlsMouseBlocking();
    }

    private void UpdatePlayerControlsInputMode() {
        if (EventSystem.current.currentSelectedGameObject == null
            && playerControls.IsTextInputMode)
        {
            playerControls.IsTextInputMode = false;
        }
        else if (EventSystem.current.currentSelectedGameObject != null
            && !playerControls.IsTextInputMode) {
            var selectedGO = EventSystem.current.currentSelectedGameObject;

            if (selectedGO.GetComponent<TMP_InputField>() != null)
            {
                playerControls.IsTextInputMode = true;
            }
        }
    }

    private void UpdatePlayerControlsMouseBlocking() {
        if (EventSystem.current.currentSelectedGameObject == null
            && playerControls.IsMouseInputDisabled)
        {
            playerControls.IsMouseInputDisabled = false;
        }
        else if (EventSystem.current.currentSelectedGameObject != null
            && !playerControls.IsMouseInputDisabled)
        {
            playerControls.IsMouseInputDisabled = true;
        }
    }
}
