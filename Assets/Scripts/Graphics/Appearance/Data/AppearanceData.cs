using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AppearanceData
{
    [Serializable]
    public class AppearanceDataItem {
        public string elementName;
        public AppearanceElementConfig elementData;
    }

    public AppearanceDataItem[] elements;
}
