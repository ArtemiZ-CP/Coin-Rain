using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    [SerializeField] private SelectQuestWindow _selectQuestWindow;
    [SerializeField] private int _chooseQuestsCount;
    [SerializeField] private Quest.Rarity[] _questRarities;
    [SerializeField] private ObjectiveSampleData[] _objectiveSamples;
    [SerializeField] private RewardSampleData[] _rewardSamples;
    [SerializeField] private GetNewQuestsButton _generateQuestsButton;

    private void OnEnable()
    {
        _generateQuestsButton.OnClick += GenerateQuest;
    }

    private void OnDisable()
    {
        _generateQuestsButton.OnClick -= GenerateQuest;
    }

    public void GenerateQuest()
    {
        if (BallsController.Instance.IsStarting)
        {
            return;
        }

        Quest.Data[] datas = new Quest.Data[_chooseQuestsCount];

        for (int i = 0; i < _chooseQuestsCount; i++)
        {
            Quest.Rarity randomDifficulty = GetRandomRarity();
            datas[i] = GenerateQuest(randomDifficulty);
        }

        _selectQuestWindow.DisplayQuests(datas);
    }

    private Quest.Data GenerateQuest(Quest.Rarity rarity)
    {
        Quest.Data multiData = new()
        {
            ObjectivesData = CreateObjectives(rarity.ObjectivesCount),
            RewardsData = CreateRewards(rarity),
            Rarity = rarity
        };

        return multiData;
    }

    private QuestObjectiveData[] CreateObjectives(int objectivesCount)
    {
        List<QuestObjectiveData> questObjectiveData = GetRandomObjectives(objectivesCount);

        ObjectiveGenerator.SetupRandomObjectives(questObjectiveData);

        return questObjectiveData.ToArray();
    }

    private QuestRewardData[] CreateRewards(Quest.Rarity rarity)
    {
        QuestRewardData[] questRewardDatas = GetRandomRewards(rarity);

        for (int i = 0; i < questRewardDatas.Length; i++)
        {
            SetupRandomReward(ref questRewardDatas[i]);
        }

        return questRewardDatas;
    }

    private Quest.Rarity GetRandomRarity()
    {
        int rarityIndex = GetRandomIndex(i => _questRarities[i].Chance, _questRarities.Length);
        return _questRarities[rarityIndex];
    }

    private void SetupRandomReward(ref QuestRewardData questRewardData)
    {
        switch (questRewardData.Type)
        {
            case QuestRewardType.Coins:
                questRewardData.IntProperties = RewardGenerator.GetCoinsRewardData();
                break;
            case QuestRewardType.Diamonds:
                questRewardData.IntProperties = RewardGenerator.GetDiamondsRewardData();
                break;
            case QuestRewardType.UnlockUpgrade:
                questRewardData.IntProperties = RewardGenerator.GetUnlockUpgradeRewardData();
                break;
            case QuestRewardType.IncreaseHeight:
                break;
            case QuestRewardType.IncreaseWidth:
                break;
            case QuestRewardType.IncreaseWinAreaMultiplier:
                break;
        }
    }

    private List<QuestObjectiveData> GetRandomObjectives(int objectivesCount)
    {
        List<ObjectiveSampleData> objectives = _objectiveSamples.ToList();
        List<QuestObjectiveData> questObjectiveDatas = new();

        for (int i = 0; i < objectivesCount; i++)
        {
            if (objectives.Count == 0)
            {
                break;
            }

            int objectiveIndex = GetRandomIndex(i => objectives[i].Chance, objectives.Count);
            QuestObjectiveData objectiveData = objectives[objectiveIndex].QuestObjectiveData;
            questObjectiveDatas.Add(objectiveData);

            for (int j = objectives.Count - 1; j >= 0; j--)
            {
                if (objectives[j].QuestObjectiveData.Type == objectiveData.Type)
                {
                    objectives.RemoveAt(j);
                }
            }
        }

        return questObjectiveDatas;
    }

    private QuestRewardData[] GetRandomRewards(Quest.Rarity rarity)
    {
        List<RewardSampleData> rewards = _rewardSamples.ToList();
        List<QuestRewardData> questRewardData = new();

        for (int i = 0; i < rarity.RandomRewardsCount; i++)
        {
            if (rewards.Count == 0)
            {
                break;
            }

            int rewardIndex = GetRandomIndex(i => rewards[i].Chance, rewards.Count);
            questRewardData.Add(rewards[rewardIndex].QuestRewardData);
            rewards.RemoveAt(rewardIndex);
        }

        return questRewardData.ToArray();
    }

    private int GetRandomIndex(Func<int, float> getChanceByIndex, int length)
    {
        float chance = 0;

        for (int i = 0; i < length; i++)
        {
            chance += getChanceByIndex(i);
        }

        chance = UnityEngine.Random.Range(0, chance);

        for (int i = 0; i < length; i++)
        {
            chance -= getChanceByIndex(i);

            if (chance <= 0)
            {
                return i;
            }
        }

        return 0;
    }

    [Serializable]
    private struct ObjectiveSampleData
    {
        public QuestObjectiveData QuestObjectiveData;
        public float Chance;
    }

    [Serializable]
    private struct RewardSampleData
    {
        public QuestRewardData QuestRewardData;
        public float Chance;
        public float RewardAmount;
    }
}
