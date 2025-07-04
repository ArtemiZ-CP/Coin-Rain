using System.Collections.Generic;
using UnityEngine;

public static class CardRewardGenerator
{
    private static readonly List<CardReward> _rewards = new()
    {
        new BlessingReward(),
        new PinReward(),
        new MapUpgradeReward(),
        new CoinsReward(),
        new ThrowReward(),
    };

    public static CardReward GenerateReward(Card.Type cardType)
    {
        List<CardReward> cardRewards = new();

        foreach (CardReward cardReward in _rewards)
        {
            if (cardReward.IsRewardAvailable(cardType))
            {
                cardRewards.Add(cardReward);
            }
        }

        int randomCardIndex = Random.Range(0, cardRewards.Count);
        CardReward clonedReward = (CardReward)System.Activator.CreateInstance(cardRewards[randomCardIndex].GetType());
        clonedReward.SetCardType(cardType);
        return clonedReward;
    }

    public enum BlessedRewardType
    {
        Blessing,
        Pin,
        MapUpgrade,
        Coins,
        Throw,
        // Cleansing,
        // BallUpgrade,
        // CardUpgrade,
        // VIPShop,
        // RestartRound,
    }
}
