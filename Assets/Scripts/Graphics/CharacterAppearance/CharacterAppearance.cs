using UnityEngine;

public class CharacterAppearance : MonoBehaviour
{
    [SerializeField]
    private CustomizableAppearance customizableAppearance;

    [SerializeField]
    private CharacterBodyData initialBodyData;

    [SerializeField]
    private CharacterOutfitData outfitData;

    private void Start() {
        SetBody(initialBodyData);
        SetOutfit(outfitData);
    }

    public void SetBody(CharacterBodyData bodyData) {
        customizableAppearance.SetAppearance(bodyData.ToAppearanceData());
    }

    public void SetOutfit(CharacterOutfitData outfitData) {
        customizableAppearance.SetAppearance(outfitData.ToAppearanceData());
    }
}
