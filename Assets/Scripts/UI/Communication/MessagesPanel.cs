using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// Panel displaying different game messages (sounds including dialogs first of all)
/// </summary>
public class MessagesPanel : MonoBehaviour
{
    // [SerializeField]
    // private GameObject messageItemPrefab;
    [SerializeField]
    private VerticalLayoutGroup messageItemsParent;

    // Todo: normal adding messages
    [SerializeField]
    private GameObject textPrefab;

    [SerializeField]
    private ScrollRect scrollRect;

    public void AddSoundReceiver(SoundReceiver soundReceiver) {
        soundReceiver.SoundReceived += OnSoundReceived;
    }

    private void OnSoundReceived(object sender, SoundReceiver.SoundReceivedEventArgs args) {
        // var newMessageItem = Instantiate(messageItemPrefab).GetComponent<MessageItem>();
        // newMessageItem.transform.SetParent(messageItemsParent);

        // Todo: normal setting of text
        // newMessageItem.Set(args.SoundData.ToDisplayText());

        string messageText = args.SoundData.ToDisplayText();
        // text.text = messageText;
        var go = Instantiate(textPrefab);
        go.GetComponent<TextMeshProUGUI>().text = messageText;
        go.transform.SetParent(messageItemsParent.transform);

        // Todo: move into another place?
        ScrollToBottom();
    }

    private void ScrollToBottom() {
        Canvas.ForceUpdateCanvases();

        messageItemsParent.CalculateLayoutInputVertical() ;
        messageItemsParent.GetComponent<ContentSizeFitter>().SetLayoutVertical() ;

        scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical() ;
        scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical() ;

        scrollRect.verticalNormalizedPosition = 0 ;
    }
}
