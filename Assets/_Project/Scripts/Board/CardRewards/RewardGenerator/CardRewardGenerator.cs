using System.Collections.Generic;

public static class CardRewardGenerator
{
    private static readonly Dictionary<Card.Type, CardReward> _cardRewards = new()
    {
        { Card.Type.Blessed, new BlessedCardReward() },
        { Card.Type.Base, new BaseCardReward() },
        { Card.Type.Cursed, new CursedCardReward() }
    };

    public static CardReward GenerateReward(Card.Type type)
    {
        if (_cardRewards.TryGetValue(type, out var reward))
        {
            return reward.GenerateReward();
        }

        return null;
    }

    public static Card.Type GetType(CardReward cardReward)
    {
        foreach (var kvp in _cardRewards)
        {
            if (kvp.Value.GetType() == cardReward.GetType())
            {
                return kvp.Key;
            }
        }

        throw new KeyNotFoundException("CardReward type not found in the generator.");
    }
}
