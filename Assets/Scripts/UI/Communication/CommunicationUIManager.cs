using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationUIManager : MonoBehaviour
{
    [SerializeField]
    private MessagesPanel messagesPanel;
    public MessagesPanel MessagesPanel => messagesPanel;

    [SerializeField]
    private CommunicationPanel communicationPanel;
    public CommunicationPanel CommunicationPanel => communicationPanel;

    // [SerializeField]
    // private RectTransform panel;
    // [SerializeField]
    // private float animationSpeed = 1.0f;
    // private bool isVisible = true;

    public void ToggleUI()
    {
        Debug.Log("Toggle UI");
        // Todo: hide all but messages
        // isVisible = !isVisible;
        // StartCoroutine(AnimatePanel());
    }

    // private IEnumerator AnimatePanel()
    // {
    //     float startX = panel.anchoredPosition.x;
    //     float targetX = isVisible ? 0 : panel.sizeDelta.x;

    //     float startTime = Time.time;
    //     while (Time.time - startTime <= 1.0f / animationSpeed)
    //     {
    //         float t = (Time.time - startTime) * animationSpeed;
    //         panel.anchoredPosition = new Vector2(Mathf.Lerp(startX, targetX, t), 0);
    //         yield return null;
    //     }

    //     panel.anchoredPosition = new Vector2(targetX, 0);
    // }
}
