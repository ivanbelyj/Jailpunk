using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    private void Awake() {
        SetConsoleActive(false);
    }

    public void ToggleConsole()
    {
        SetConsoleActive(!IsConsoleActive);
    }

    private void SetConsoleActive(bool isActive) {
        Console.DeveloperConsole.active = isActive;
    }

    private bool IsConsoleActive => Console.DeveloperConsole.active;
}
