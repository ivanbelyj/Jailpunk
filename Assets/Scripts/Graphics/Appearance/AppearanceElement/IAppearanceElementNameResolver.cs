using UnityEngine;

/// <summary>
/// Resolves element name defined in appearance schema to appearance sprite name.
/// For example, element name 'torso' in appearance schema can be resolved
/// into 'torso_male' or 'torso_female' appearance sprite name
/// depending on character data
/// </summary>
public interface IAppearanceElementNameResolver {
    string GetAppearanceSpriteName(string appearanceElementName);
}
