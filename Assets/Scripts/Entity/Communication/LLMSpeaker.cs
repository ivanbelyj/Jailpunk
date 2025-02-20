using System.Collections;
using System;
using UnityEngine;
using LLMUnity;

public class LLMCharacterDecorator {
    private readonly LLMCharacter llmCharacter;
    private readonly LLM llm;

    public LLMCharacterDecorator(
        LLMCharacter llmCharacter,
        LLM llm)
    {
        this.llmCharacter = llmCharacter;
        this.llm = llm;
    }

    public void Init(string aiName) {
        InitLLMCharacter(aiName);
    }

    public void SetRecipient(string playerName) {
        llmCharacter.playerName = playerName;
    }

    private void InitLLMCharacter(string aiName)
    {
        llmCharacter.llm = llm;
        llmCharacter.AIName = aiName;
        SetRecipient("Stranger");
        llmCharacter.stream = true;
    }
}

[RequireComponent(typeof(Person))]
[RequireComponent(typeof(SoundReceiver))]
[RequireComponent(typeof(Speaker))]
public class LLMSpeaker : MonoBehaviour
{
    private LLM llm;

    [SerializeField]
    private LLMCharacter llmCharacter;

    private Person subject;
    private Speaker speaker;

    [SerializeField]
    private bool answerOnSpeechReceived = true;

    private LLMCharacterDecorator llmCharacterDecorator;

    private void Start() {
        subject = GetComponent<Person>();
        speaker = GetComponent<Speaker>();
        
        if (answerOnSpeechReceived) {
            GetComponent<SoundReceiver>().SoundReceived += OnSoundReceived;
        }

        llm = FindAnyObjectByType<LLM>();

        llmCharacterDecorator = new(llmCharacter, llm);
        llmCharacterDecorator.Init(subject.PersonName);
    }

    public void Answer(CharacterId recipientId, string query) {
        string knownName = subject.GetKnownCharacterName(recipientId);
        llmCharacterDecorator.SetRecipient(knownName);

        Guid messageId = Guid.NewGuid();
        
        llmCharacter.Chat(
            query,
            (x) => Speak(messageId, recipientId, x),
            () => Debug.Log("LLMSpeaker answered"));
    }

    private void OnSoundReceived(
        object sender,
        SoundReceiver.SoundReceivedEventArgs args)
    {
        if (args.SoundData is SpeechSoundData speech) {
            
            if (speech.RecipientId == subject.GetCharacterId()) {
                Answer(speech.SpeakerId, speech.Message);
            }
            if (speech.SpeakerId.IsCharacterIdentity &&
                speech.RecipientId.IsNoIdentity) {
                // Ignore ?
            }
        }
    }

    private void Speak(Guid messageId, CharacterId speakerId, string message) {
        speaker.Speak(GetAnswer(messageId, speakerId, message));
    }

    private SpeechSoundData GetAnswer(
        Guid messageId,
        CharacterId characterId,
        string message)
    {
        return new SpeechSoundData(messageId) {
            SpeakerId = subject.GetCharacterId(),
            RecipientId = characterId,
            Message = message
        };
    }
}
