using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommunicationPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject choiceItemPrefab;

    [SerializeField]
    private Transform choiceItemsParent;
    private Person subject;
    private Speaker speaker;

    /// <summary>
    /// UI items representing choices
    /// </summary>
    private List<ChoiceItem> choiceItems;

    // [SerializeField]
    // private TMP_Dropdown speechVolumeDropdown;

    [SerializeField]
    private TMP_Dropdown recipientDropdown;

    private List<CharacterId> availableRecipients;
    private CharacterId selectedRecipient;

    private ChoicesProvider choicesProvider;

    public void SetSubjectPerson(Person subject, Speaker speaker) {
        this.subject = subject;
        this.speaker = speaker;
        choicesProvider = new ChoicesProvider(this.subject, speaker);
    }

    private CharacterId GetRecipientByDropdownIndex(int index) {
        return index == 0 ? CharacterId.NoIdentity
            : availableRecipients[index - 1];
    }

    private int GetRecipientDropdownIndex(CharacterId recipient) {
        if (recipient.IsNoIdentity)
            return 0;
        int indexOfRecipient = availableRecipients.IndexOf(recipient);
        if (indexOfRecipient == -1)
            return indexOfRecipient;
        return indexOfRecipient + 1;
    }

    public void OnRecipientSelected(int newIndex) {
        // Debug.Log($"Selected item {newIndex}. there are {availableRecipients.Count} recipients available");
        SetRecipientId(GetRecipientByDropdownIndex(newIndex));
    }

    public void OnSpeechVolumeSelected(int newIndex) {
        SpeechVolume volume = (SpeechVolume)newIndex;
        speaker.SetSpeechVolume(volume);
    }

    public void SetRecipientId(CharacterId recipientId) {
        selectedRecipient = recipientId;
        UpdatePanel();
    }

    public void SetAvailableRecipientIds(List<CharacterId> recipients) {
        recipientDropdown.ClearOptions();
        var options = new List<TMP_Dropdown.OptionData>() {
            new TMP_Dropdown.OptionData() {
                text = "Everybody"
            }
        };
        options.AddRange(recipients
            .Select(recipientId => new TMP_Dropdown.OptionData() {
                text = $"{subject.GetKnownCharacterName(recipientId)}"
            }));
        recipientDropdown.AddOptions(options);

        this.availableRecipients = recipients;

        // If selected recipient is not actiual, change to "Everybody"
        if (GetRecipientDropdownIndex(selectedRecipient) == -1) {
            selectedRecipient = CharacterId.NoIdentity;
        }

        UpdatePanel();
    }

    private void UpdatePanel() {
        recipientDropdown.value = GetRecipientDropdownIndex(selectedRecipient);
        
        var choices = choicesProvider.GetChoices(selectedRecipient);
        choiceItems?.ForEach(c => Destroy(c.gameObject));
        choiceItems = new List<ChoiceItem>();

        int choiceNumber = 1;
        foreach (var cmd in choices)
        {
            ChoiceItem newChoiceItem = Instantiate(choiceItemPrefab)
                .GetComponent<ChoiceItem>();
            newChoiceItem.transform.SetParent(choiceItemsParent);
            newChoiceItem.Set(choiceNumber++, cmd);

            choiceItems.Add(newChoiceItem);
        };
    }
    
}
