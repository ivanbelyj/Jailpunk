using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public static class LifecycleEffectReadWrite
{
    public static void WriteLifecycleEffect(this NetworkWriter writer,
        LifecycleEffect effect) {

        writer.WriteFloat(effect.duration);
        writer.WriteBool(effect.isInfinite);
        writer.WriteFloat(effect.speed);

        writer.WriteByte((byte)effect.targetParameterId);
        writer.WriteDouble(effect.StartTime);
    }

    public static LifecycleEffect ReadLifecycleEffect(this NetworkReader reader) {
        
        float duration = reader.ReadFloat();
        bool isInfinite = reader.ReadBool();
        float speed = reader.ReadFloat();

        byte targetParameterIndex = reader.ReadByte();
        double startTime = reader.ReadDouble();

        LifecycleEffect effect = new LifecycleEffect() {
            speed = speed,
            isInfinite = isInfinite,
            targetParameterId = (LifecycleParameterEnum)targetParameterIndex,
            duration = duration,
            StartTime = startTime,
        };
        return effect;
    }
}
