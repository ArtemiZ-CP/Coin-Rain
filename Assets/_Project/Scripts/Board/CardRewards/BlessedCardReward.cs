using System.Collections.Generic;
using UnityEngine;

public class BlessedCardReward : CardReward
{
    private static readonly Dictionary<Type, BlessedCardReward> _rewards = new()
    {
        { Type.Blessing, new BlessingReward() },
        { Type.Coins, new CoinsReward() },
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
        var rewardType = GetRandomRewardType();
        return TryGetReward(rewardType);
    }

    private BlessedCardReward TryGetReward(Type type)
    {
        if (_rewards.TryGetValue(type, out BlessedCardReward reward))
        {
            return (BlessedCardReward)System.Activator.CreateInstance(reward.GetType());
        }
        
        throw new KeyNotFoundException($"BlessedCardReward type '{type}' not found in the generator.");
    }

    private Type GetRandomRewardType()
    {
        var values = (Type[])System.Enum.GetValues(typeof(Type));
        return values[Random.Range(0, values.Length)];
    }

    public enum Type
    {
        Blessing,
        Coins,
        // Cleansing,
        // MapUpgrade,
        // BallUpgrade,
        // PinUpgrade,
        // CardUpgrade,
        // VIPShop,
        // RestartRound,
    }
}
