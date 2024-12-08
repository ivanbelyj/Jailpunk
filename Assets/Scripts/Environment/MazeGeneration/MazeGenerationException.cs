using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MazeGenerationException : System.Exception
{
    public MazeGenerationException() { }
    public MazeGenerationException(string message) : base(message) { }
    public MazeGenerationException(string message, System.Exception inner) : base(message, inner) { }
    protected MazeGenerationException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
