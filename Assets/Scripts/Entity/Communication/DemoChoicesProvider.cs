using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemoChoicesProvider : IChoicesProvider
{
    public Person Subject { get; set; }
    public Speaker Speaker { get; set; }
    
    public DemoChoicesProvider(Person subject, Speaker speaker) {
        Subject = subject;
        Speaker = speaker;
    }

    public IEnumerable<CommunicationCommand> GetChoices(CharacterId recipientId) {
        return GetTestCommands(recipientId);
    }
    private IEnumerable<SpeechCommand> GetTestCommands(CharacterId recipientId) {
        List<string> commands;
        if (recipientId.IsCharacterIdentity) {
            commands = new List<string> {
                $"Sheaneim seileish {Subject.GetKnownCharacterName(recipientId)}!",
                "Seimiar lai she he!",
                "Kias"
            };
        } else {
            commands = new List<string> {
                "Nemeu nreshou dei he!",
                "Loushea neraa laa?"
            };
        }
        return commands.Select(str => new SpeechCommand(Speaker, new SpeechSoundData() {
            SpeakerId = Subject.GetCharacterId(),
            Message = str,
            RecipientId = recipientId
        }));
    }
}
