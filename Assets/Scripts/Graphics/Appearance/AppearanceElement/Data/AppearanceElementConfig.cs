using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public struct AppearanceElementConfig {
    public bool isActive;
    public Color color;
    
    [Tooltip(
        "Leave empty to use element name from appearance schema or set this field " +
        "to override. For example, element 'tail' could be overriden by " +
        "'fluffy_tail' and another sprite would be used for the element")]
    public string appearanceSpriteName;
}
