using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// Panel displaying different game messages (sounds including dialogs first of all)
/// </summary>
public class MessagesPanel : MonoBehaviour
{
    [SerializeField]
    private VerticalLayoutGroup messageItemsParent;

    // Todo: normal adding message items
    [SerializeField]
    private GameObject textPrefab;

    [SerializeField]
    private ScrollRect scrollRect;

    private Person subjectPerson;

    public void SetSubjectPerson(Person person) {
        subjectPerson = person;
    }

    public void AddSoundReceiver(SoundReceiver soundReceiver) {
        soundReceiver.SoundReceived += OnSoundReceived;
    }    

    private void OnSoundReceived(object sender,
        SoundReceiver.SoundReceivedEventArgs args) {
        // Todo: normal message items, not only text

        string messageText = "";
        if (args.SoundData is SpeechSoundData speech) {
            messageText += subjectPerson.GetKnownCharacterName(speech.SpeakerId)
                + ": ";
        }
        
        messageText += args.SoundData.ToDisplayText();
        AddMessage(messageText);
        

        // Todo: move into another place?
        ScrollToBottom();
    }

    private void AddMessage(string messageText) {
        var go = Instantiate(textPrefab);
        go.GetComponent<TextMeshProUGUI>().text = messageText;
        go.transform.SetParent(messageItemsParent.transform);
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
