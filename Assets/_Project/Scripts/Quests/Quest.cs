using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Quest
{
    private readonly QuestObjective[] _objectives;
    private readonly QuestReward[] _rewards;

    public delegate void QuestUpdated(Quest quest);
    public event QuestUpdated OnQuestCompleted;

    public QuestObjective[] Objectives => _objectives;
    public QuestReward[] Rewards => _rewards;

    public Quest(QuestObjective[] objectives, QuestReward[] rewards)
    {
        _objectives = objectives;
        _rewards = rewards;

        foreach (QuestObjective objective in _objectives)
        {
            objective.OnObjectiveCompleted += CompleteObjective;
        }
    }

    public void CompleteObjective()
    {
        foreach (QuestObjective objective in _objectives)
        {
            if (objective.IsCompleted == false)
            {
                return;
            }
        }

        foreach (QuestObjective objective in _objectives)
        {
            objective.OnObjectiveCompleted -= CompleteObjective;
        }

        foreach (QuestReward reward in _rewards)
        {
            reward.ApplyReward();
        }

        OnQuestCompleted?.Invoke(this);
    }

    public static Data GetQuestData(Quest quest)
    {
        return new Data
        {
            ObjectivesData = GetObjectiveData(quest.Objectives),
            RewardsData = GetRewardData(quest.Rewards),
        };
    }

    public static Quest CreateQuest(Data data)
    {
        return new Quest(CreateObjective(data.ObjectivesData), CreateReward(data.RewardsData));
    }

    public static QuestObjective[] CreateObjective(QuestObjectiveData[] data)
    {
        QuestObjective[] questObjectives = new QuestObjective[data.Length];

        for (int i = 0; i < questObjectives.Length; i++)
        {
            questObjectives[i] = CreateObjective(data[i]);
        }

        return questObjectives;
    }

    public static QuestReward[] CreateReward(QuestRewardData[] data)
    {
        QuestReward[] questRewards = new QuestReward[data.Length];

        for (int i = 0; i < questRewards.Length; i++)
        {
            questRewards[i] = CreateReward(data[i]);
        }

        return questRewards;
    }

    public static QuestObjectiveData[] GetObjectiveData(QuestObjective[] objective)
    {
        QuestObjectiveData[] questObjectiveDatas = new QuestObjectiveData[objective.Length];

        for (int i = 0; i < questObjectiveDatas.Length; i++)
        {
            questObjectiveDatas[i] = GetObjectiveData(objective[i]);
        }

        return questObjectiveDatas;
    }

    public static QuestRewardData[] GetRewardData(QuestReward[] rewards)
    {
        QuestRewardData[] questRewardDatas = new QuestRewardData[rewards.Length];

        for (int i = 0; i < questRewardDatas.Length; i++)
        {
            questRewardDatas[i] = GetRewardData(rewards[i]);
        }

        return questRewardDatas;
    }

    public static QuestObjective CreateObjective(QuestObjectiveData data)
    {
        return data.Type switch
        {
            QuestObjectiveType.HitFinish => new HitFinishObjective(data.IntProperties[0]),
            QuestObjectiveType.HitPins => new HitPinsObjective(data.IntProperties[0], (Pin.Type)data.IntProperties[1]),
            QuestObjectiveType.EarnCoinsByOneBall => new EarnCoinsObjective(data.FloatProperties[0]),
            QuestObjectiveType.EarnCoinsByAllBalls => new EarnCoinsByAllBallsObjective(data.FloatProperties[0]),
            QuestObjectiveType.FallTime => new FallTimeObjective(data.FloatProperties[0]),
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
            FallTimeObjective fallTimeObjective => new QuestObjectiveData
            {
                Type = QuestObjectiveType.FallTime,
                FloatProperties = new float[] { fallTimeObjective.Timer }
            },
            _ => default,
        };
    }

    public static QuestReward CreateReward(QuestRewardData data)
    {
        return data.Type switch
        {
            QuestRewardType.Coins => new CoinsReward(data.IntProperties[0]),
            QuestRewardType.Diamonds => new DiamondsReward(data.IntProperties[0]),
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
            DiamondsReward diamondsReward => new QuestRewardData
            {
                Type = QuestRewardType.Coins,
                IntProperties = new int[] { diamondsReward.DiamondsCount }
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
        public QuestObjectiveData[] ObjectivesData;
        public QuestRewardData[] RewardsData;
        public Rarity Rarity;
    }

    [Serializable]
    public struct Rarity
    {
        public int ObjectivesCount;
        public int RewardsCount;
        public float Chance;
        public Color Color;
    }
}
