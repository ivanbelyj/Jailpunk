using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI textMeshPro;

    public Color hoveredColor = Color.gray;
    public float transitionDuration = 0.3f;

    private Color initialColor;
    private CommunicationCommand command;

    private void Awake()
    {
        initialColor = textMeshPro.color;
    }

    public void Set(int choiceNumber, CommunicationCommand command) {
        this.command = command;
        textMeshPro.text = $"{choiceNumber}. {command.GetDisplayText()}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CrossFadeColor(hoveredColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CrossFadeColor(initialColor);
    }

    private void CrossFadeColor(Color color) {
        textMeshPro.CrossFadeColor(color, transitionDuration, true, true);
    }

    public void MakeChoice() {
        command.Execute();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MakeChoice();
    }
}
