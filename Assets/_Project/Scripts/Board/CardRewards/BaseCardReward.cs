using System.Collections.Generic;

public class BaseCardReward : CardReward
{
    private static readonly Dictionary<Type, BaseCardReward> _rewards = new()
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
        BaseCardReward reward = new();
        return reward;
    }

    public enum Type
    {
        
    }
}
