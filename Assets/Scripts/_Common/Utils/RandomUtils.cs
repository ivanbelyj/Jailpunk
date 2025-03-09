using System.Collections.Generic;
using UnityEngine;

public static class RandomUtils
{
    /// <summary>
    /// Selects a random element from the list with equal probability for each item.
    /// </summary>
    /// <returns>A randomly selected item.</returns>
    public static T GetRandomOne<T>(IList<T> items)
        => items[(int)(Random.value * items.Count)];

    /// <summary>
    /// Selects a random element from the list using weighted probabilities (roulette wheel algorithm).
    /// </summary>
    /// <param name="items">The list of items to choose from.</param>
    /// <param name="weights">The list of weights corresponding to each item.</param>
    /// <returns>A randomly selected item based on the provided weights.</returns>
    public static T GetRandomWeighted<T>(IList<T> items, IList<float> weights)
    {
        if (items == null || weights == null
            || items.Count != weights.Count || items.Count == 0)
        {
            throw new System.ArgumentException("Items and weights must be non-null, of the same length, and non-empty.");
        }

        float totalWeight = 0f;
        foreach (float weight in weights)
            totalWeight += weight;

        float randomValue = Random.value * totalWeight;
        float cumulativeWeight = 0f;

        for (int i = 0; i < items.Count; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue <= cumulativeWeight)
                return items[i];
        }

        return items[items.Count - 1];
    }
}
