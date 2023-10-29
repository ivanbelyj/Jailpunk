using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject choiceItemPrefab;

    [SerializeField]
    private Transform choiceItemsParent;

    private List<ChoiceItem> choiceItems;

    private void Start() {
        // For test
        var commands = new [] {
            new SpeechCommand() {
                SpeechSoundData = new SpeechSoundData() {
                    Message = "Sheaneim Seileish!"
                },
                SpeechVolume = SpeechVolume.Normal
            },
            new SpeechCommand() {
                SpeechSoundData = new SpeechSoundData() {
                    Message = "Seimiar eum ki lai!"
                },
                SpeechVolume = SpeechVolume.Normal
            },
            new SpeechCommand() {
                SpeechSoundData = new SpeechSoundData() {
                    Message = "Kias"
                },
                SpeechVolume = SpeechVolume.Normal
            }
        };
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
}
