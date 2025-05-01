using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerQuests : MonoBehaviour
{
    private readonly List<Quest> _activeQuests = new();
    private readonly List<Quest> _disactiveQuests = new();

    [SerializeField] private int _maxActiveQuests = 3;
    [SerializeField] private List<QuestData> _playerQuests = new();

    public IReadOnlyList<Quest> ActiveQuests => _activeQuests;

    public event Action OnQuestsUpdated;

    private void Awake()
    {
        foreach (QuestData objectiveData in _playerQuests)
        {
            _disactiveQuests.Add(CreateQuest(objectiveData));
        }

        List<QuestData> questsToRemove = PlayerData.CompletedQuests.ToList();

        for (int i = 0; i < _disactiveQuests.Count; i++)
        {
            QuestData questData = GetQuestData(_disactiveQuests[i]);

            if (questsToRemove.Contains(questData))
            {
                questsToRemove.Remove(questData);
                _disactiveQuests.RemoveAt(i);
                i--;
            }
        }

        UpdateActiveQuests();
        OnQuestsUpdated?.Invoke();
    }

    public void AddActiveQuest(Quest quest)
    {
        _activeQuests.Add(quest);
        quest.OnQuestCompleted += OnQuestCompleted;
    }

    public void RemoveActiveQuest(Quest quest)
    {
        if (_activeQuests.Contains(quest))
        {
            _activeQuests.Remove(quest);
            quest.OnQuestCompleted -= OnQuestCompleted;
        }
    }

    private void OnQuestCompleted(Quest quest)
    {
        PlayerData.CompleteQuest(quest);
        RemoveActiveQuest(quest);
        UpdateActiveQuests();
        OnQuestsUpdated?.Invoke();
    }

    public static Quest CreateQuest(QuestData data)
    {
        return new Quest(CreateObjective(data.ObjectivesData), CreateReward(data.RewardData));
    }

    public static QuestData GetQuestData(Quest quest)
    {
        return new QuestData
        {
            ObjectivesData = GetObjectiveData(quest.Objective),
            RewardData = GetRewardData(quest.Reward),
        };
    }

    public static QuestObjective CreateObjective(QuestObjectiveData data)
    {
        return data.Type switch
        {
            QuestObjectiveType.DropBall => new DropBallObjective(),
            QuestObjectiveType.HitFinish => new HitFinishObjective(data.FinishMultiplierToHit),
            QuestObjectiveType.HitPins => new HitPinsObjective(data.PinsCount),
            QuestObjectiveType.EarnCoins => new EarnCoinsObjective(data.CoinsCount),
            _ => null,
        };
    }

    public static QuestObjectiveData GetObjectiveData(QuestObjective objective)
    {
        return objective switch
        {
            DropBallObjective => new QuestObjectiveData { Type = QuestObjectiveType.DropBall },
            HitFinishObjective hitFinish => new QuestObjectiveData { Type = QuestObjectiveType.HitFinish, FinishMultiplierToHit = hitFinish.FinishMultiplierToHit },
            HitPinsObjective hitPins => new QuestObjectiveData { Type = QuestObjectiveType.HitPins, PinsCount = hitPins.PinsCount },
            EarnCoinsObjective earnCoins => new QuestObjectiveData { Type = QuestObjectiveType.EarnCoins, CoinsCount = earnCoins.CoinsCount },
            _ => new QuestObjectiveData(),
        };
    }

    public static QuestReward CreateReward(QuestRewardData data)
    {
        return data.Type switch
        {
            QuestRewardType.Coins => new CoinsReward(data.CoinsCount),
            QuestRewardType.UnlockUpgrade => new UnlockUpgrade(data.UpgradeType),
            _ => null,
        };
    }

    public static QuestRewardData GetRewardData(QuestReward reward)
    {
        return reward switch
        {
            CoinsReward coinsReward => new QuestRewardData { Type = QuestRewardType.Coins, CoinsCount = coinsReward.CoinsCount },
            UnlockUpgrade unlockUpgrade => new QuestRewardData { Type = QuestRewardType.UnlockUpgrade, UpgradeType = unlockUpgrade.UpgradeType },
            _ => new QuestRewardData(),
        };
    }

    private void UpdateActiveQuests()
    {
        for (int i = 0; i < _disactiveQuests.Count; i++)
        {
            if (_activeQuests.Count < _maxActiveQuests)
            {
                Quest quest = _disactiveQuests[i];
                AddActiveQuest(quest);
                _disactiveQuests.Remove(quest);
                i--;
            }
            else
            {
                return;
            }
        }
    }

    [Serializable]
    public struct QuestData
    {
        public QuestObjectiveData ObjectivesData;
        public QuestRewardData RewardData;
    }
}
