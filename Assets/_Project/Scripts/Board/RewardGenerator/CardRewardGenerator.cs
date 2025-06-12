using System.Collections.Generic;
using UnityEngine;

public static class CardRewardGenerator
{
    private static readonly Dictionary<RewardType, CardReward> _rewards = new()
    {
        { RewardType.Blessing, new BlessingReward() },
        { RewardType.Pin, new PinReward() },
        { RewardType.MapUpgrade, new MapUpgradeReward() },
        { RewardType.Coins, new CoinsReward() },
    };

    public static CardReward GenerateReward(Card.Type cardType)
    {
        List<CardReward> cardRewards = new();

        foreach (KeyValuePair<RewardType, CardReward> kvp in _rewards)
        {
            if (kvp.Value.CardType == cardType && kvp.Value.IsRewardAvailable(cardType))
            {
                cardRewards.Add(kvp.Value);
            }
        }

        CardReward clonedReward = (CardReward)System.Activator.CreateInstance(cardRewards[Random.Range(0, cardRewards.Count)].GetType());
        clonedReward.SetCardType(cardType);
        return clonedReward;
    }

    public enum RewardType
    {
        Blessing,
        Pin,
        MapUpgrade,
        Coins,
        // Cleansing,
        // BallUpgrade,
        // CardUpgrade,
        // VIPShop,
        // RestartRound,
    }
}
