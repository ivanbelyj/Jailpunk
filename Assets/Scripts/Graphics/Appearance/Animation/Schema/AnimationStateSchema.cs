using System;

public enum AnimationType {
    Loop = 1,
    Once = 2
}

[Serializable]
public record AnimationStateSchema {
    public string state;
    public int framesCount;
    public float secondsPerFrame;
    public AnimationType animationType = AnimationType.Loop;
}