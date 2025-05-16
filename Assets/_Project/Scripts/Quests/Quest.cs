using System;

public class Quest
{
    private readonly QuestObjective _objective;
    private readonly QuestReward _reward;

    public delegate void QuestUpdated(Quest quest);
    public event QuestUpdated OnQuestCompleted;

    public QuestObjective Objective => _objective;
    public QuestReward Reward => _reward;

    public Quest(QuestObjective objective, QuestReward rewardData)
    {
        _objective = objective;
        _reward = rewardData;
        _objective.OnObjectiveCompleted += CompleteQuest;
    }

    public void CompleteQuest()
    {
        _reward.ApplyReward();
        OnQuestCompleted?.Invoke(this);
        _objective.OnObjectiveCompleted -= CompleteQuest;
    }

    public static Data GetQuestData(Quest quest)
    {
        return new Data
        {
            ObjectivesData = GetObjectiveData(quest.Objective),
            RewardData = GetRewardData(quest.Reward),
        };
    }

    public static Quest CreateQuest(Data data)
    {
        return new Quest(CreateObjective(data.ObjectivesData), CreateReward(data.RewardData));
    }

    public static QuestObjective CreateObjective(QuestObjectiveData data)
    {
        return data.Type switch
        {
            QuestObjectiveType.HitFinish => new HitFinishObjective(data.IntProperties[0]),
            QuestObjectiveType.HitPins => new HitPinsObjective(data.IntProperties[0], (Pin.Type)data.IntProperties[1]),
            QuestObjectiveType.EarnCoinsByOneBall => new EarnCoinsObjective(data.FloatProperties[0]),
            QuestObjectiveType.EarnCoinsByAllBalls => new EarnCoinsByAllBallsObjective(data.FloatProperties[0]),
            _ => null,
        };
    }

    public static QuestObjectiveData GetObjectiveData(QuestObjective objective)
    {
        return objective switch
        {
            HitFinishObjective hitFinish => new QuestObjectiveData
            {
                Type = QuestObjectiveType.HitFinish,
                IntProperties = new int[] { hitFinish.FinishMultiplierToHit }
            },
            HitPinsObjective hitPins => new QuestObjectiveData
            {
                Type = QuestObjectiveType.HitPins,
                IntProperties = new int[] { hitPins.PinsCount, (int)hitPins.PinType }
            },
            EarnCoinsObjective earnCoins => new QuestObjectiveData
            {
                Type = QuestObjectiveType.EarnCoinsByOneBall,
                FloatProperties = new float[] { earnCoins.CoinsCount }
            },
            EarnCoinsByAllBallsObjective earnCoinsByAllBalls => new QuestObjectiveData
            {
                Type = QuestObjectiveType.EarnCoinsByAllBalls,
                FloatProperties = new float[] { earnCoinsByAllBalls.CoinsCount }
            },
            _ => default,
        };
    }

    public static QuestReward CreateReward(QuestRewardData data)
    {
        return data.Type switch
        {
            QuestRewardType.Coins => new CoinsReward(data.IntProperties[0]),
            QuestRewardType.UnlockUpgrade => new UnlockUpgrade((Pin.Type)data.IntProperties[0]),
            QuestRewardType.IncreaseHeight => new IncreaseHeightReward(),
            QuestRewardType.IncreaseWidth => new IncreaseWidthReward(),
            QuestRewardType.IncreaseWinAreaMultiplier => new IncreaseWinAreaMultiplierReward(),
            _ => null,
        };
    }

    public static QuestRewardData GetRewardData(QuestReward reward)
    {
        return reward switch
        {
            CoinsReward coinsReward => new QuestRewardData
            {
                Type = QuestRewardType.Coins,
                IntProperties = new int[] { coinsReward.CoinsCount }
            },
            UnlockUpgrade unlockUpgrade => new QuestRewardData
            {
                Type = QuestRewardType.UnlockUpgrade,
                IntProperties = new int[] { (int)unlockUpgrade.PinType }
            },
            IncreaseHeightReward => new QuestRewardData
            {
                Type = QuestRewardType.IncreaseHeight
            },
            IncreaseWidthReward => new QuestRewardData
            {
                Type = QuestRewardType.IncreaseWidth
            },
            IncreaseWinAreaMultiplierReward => new QuestRewardData
            {
                Type = QuestRewardType.IncreaseWinAreaMultiplier
            },
            _ => new QuestRewardData(),
        };
    }

    [Serializable]
    public struct Data
    {
        public QuestObjectiveData ObjectivesData;
        public QuestRewardData RewardData;
    }
}
