using System.Collections.Generic;
using UnityEngine;

public class CursedCardReward : CardReward
{
    private static readonly Dictionary<Type, CursedCardReward> _rewards = new()
    {
    };

    public Type GetCardType()
    {
        foreach (var kvp in _rewards)
        {
            if (kvp.Value.GetType() == GetType())
            {
                return kvp.Key;
            }
        }

        throw new KeyNotFoundException("BlessedCardReward type not found in the generator.");
    }

    public override CardReward GenerateReward()
    {
        CursedCardReward reward = new();
        return reward;
    }

    public enum Type
    {
    }
}
