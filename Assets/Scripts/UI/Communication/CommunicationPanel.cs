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
    private Speaker speaker;
    private Person subject;

    private List<ChoiceItem> choiceItems;

    [SerializeField]
    private TMP_Dropdown recipientsDropdown;

    public void SetSubject(Person subject, Speaker speaker) {
        this.subject = subject;
        this.speaker = speaker;
        SetTestChoice();
    }

    private void SetTestChoice() {
        // For test
        var commands = new [] {
            "Sheaneim Seileish!",
            "Seimiar eum ki lai!",
            "Kias"
        }.Select(str => new SpeechCommand(speaker, new SpeechSoundData() {
            Message = str
        }));
        SetChoices(commands);
    }

    public void SetChoices(IEnumerable<CommunicationCommand> commands) {
        choiceItems?.ForEach(c => Destroy(c.gameObject));
        choiceItems = new List<ChoiceItem>();

        int choiceNumber = 1;
        foreach (var cmd in commands)
        {
            ChoiceItem newChoiceItem = Instantiate(choiceItemPrefab)
                .GetComponent<ChoiceItem>();
            newChoiceItem.transform.SetParent(choiceItemsParent);
            newChoiceItem.Set(choiceNumber++, cmd);

            choiceItems.Add(newChoiceItem);
        };
    }

    public void SetAvailableRecipients(IEnumerable<ISubject> recipients) {
        recipientsDropdown.ClearOptions();
        var options = new List<TMP_Dropdown.OptionData>() {
            new TMP_Dropdown.OptionData() {
                text = "Everybody"
            }
        };
        options.AddRange(recipients
            .Select(recipient => new TMP_Dropdown.OptionData() {
                text = $"{subject.GetKnownSubjectName(recipient.GetSubjectId())}"
            }));
        recipientsDropdown.AddOptions(options);
    }
}
