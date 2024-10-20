using System;
using System.Collections;
using UnityEngine;

public static class SmoothTransitionUtils
{
    public static IEnumerator SmoothTransition(
        float startValue,
        float endValue,
        float duration,
        Action<float> applyNewValue)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            t = t * t * (3f - 2f * t);

            float newValue = Mathf.Lerp(startValue, endValue, t);
            applyNewValue(newValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        applyNewValue(endValue);
    }
}
