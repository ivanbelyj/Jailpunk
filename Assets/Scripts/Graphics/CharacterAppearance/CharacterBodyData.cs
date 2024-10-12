using System;
using UnityEngine;

[Serializable]
public class CharacterBodyData
{
    public Gender gender;
    public Ears ears;
    public Head head;
    public Muzzle muzzle;
    public Nose nose;
    public NeckFur neckFur;
    public Tail tail;
    public Color furColor;
    public Color eyesColor;
    public bool useSeparateEyeColors;
    public Color leftEyeColor;
    public Color rightEyeColor;

    public AppearanceData ToAppearanceData() {
        return new AppearanceData {
            elements = new AppearanceData.AppearanceDataItem [] {
                CreateBodyPart(
                    head switch {
                        _ => "torso"
                    }),
                CreateBodyPart(
                    head switch {
                        _ => "head"
                    }),
                CreateBodyPart(
                    head switch {
                        _ => "eye_left"
                    },
                    useSeparateEyeColors ? leftEyeColor : eyesColor),
                CreateBodyPart(
                    head switch {
                        _ => "eye_right"
                    },
                    useSeparateEyeColors ? rightEyeColor : eyesColor),
                CreateBodyPart(
                    head switch {
                        _ => "hand_left"
                    }),
                CreateBodyPart(
                    head switch {
                        _ => "hand_right"
                    }),
                // Todo: remove Color.white when normal paws will be drawn
                CreateBodyPart("leg_left", Color.white),
                CreateBodyPart("leg_right", Color.white),
                new() {
                    elementName = tail switch {
                        _ => "tail"
                    },
                    elementData = new() {
                        color = furColor,
                        isActive = tail != Tail.None
                    }
                },
            }
        };
    }

    private AppearanceData.AppearanceDataItem CreateBodyPart(
        string name,
        Color? color = null) {
        return new () {
            elementName = name,
            elementData = new() {
                color = color ?? furColor,
                isActive = true
            }
        };
    }
}
