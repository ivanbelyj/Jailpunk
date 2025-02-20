using System.Collections.Generic;

public class AppearanceElementNameResolver : IAppearanceElementNameResolver
{
    private Dictionary<string, string> spriteNamesByElementName = new();
    public void SetAppearanceSprite(
        string appearanceElementName,
        string appearanceSpriteName)
    {
        spriteNamesByElementName[appearanceElementName] = appearanceSpriteName;
    }

    public string GetAppearanceSpriteName(string appearanceElementName)
    {
        return spriteNamesByElementName[appearanceElementName];
    }
}
