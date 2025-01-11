using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComplexGenerationException : System.Exception
{
    public ComplexGenerationException() { }
    public ComplexGenerationException(string message) : base(message) { }
    public ComplexGenerationException(string message, System.Exception inner) : base(message, inner) { }
    protected ComplexGenerationException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
