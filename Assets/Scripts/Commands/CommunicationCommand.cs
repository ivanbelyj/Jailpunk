using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Communication is an action of a character addressed to another character
/// </summary>
public abstract class CommunicationCommand : ICommand
{
    /// <summary>
    /// Unique id of the recipient of the speech. 0 - everyone
    /// </summary>
    public uint RecipientId { get; set; }
    public abstract string GetDisplayText();
    public abstract void Execute();
}
