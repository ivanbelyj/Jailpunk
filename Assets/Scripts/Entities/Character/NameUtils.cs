using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameUtils
{
    public static string[] firstNames = new [] {
        "Khato",
        "Aliho",
        "Shiho",
        "Neksho",
        "Neho",
        "Shiho",
        "Thago",
        "Negho",
        "Harakoro",
        "Shakhago"
    };

    public static string[] lastNames = new [] {
        "Khash",
        "Tashagho",
        "Kshakhamogho",
        "Takakshaki",
        "Alakshahi",
        "Khatakoro",
        "Nalakhashi",
        "Khai",
        "Kshathagati",
        "Shanaki",
        "Lhakhi",
        "Khalai",
        "Shakahori"
    };

    private static T GetRandomItem<T>(T[] arr)
        => arr[Random.Range(0, arr.Length)];

    public static string GetRandomName() =>
        "Test " + Random.Range(100, 999);
        // GetRandomItem(firstNames) + " " + GetRandomItem(lastNames);
        // Too aesthetic names for characters with a test appearance :D
}
