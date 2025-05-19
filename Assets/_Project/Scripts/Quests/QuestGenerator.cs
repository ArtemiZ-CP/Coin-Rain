using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    [SerializeField] private SelectQuestWindow _selectQuestWindow;
    [SerializeField] private int _chooseQuestsCount;
    [SerializeField] private Rarity[] _questRarities;
    [SerializeField] private ObjectiveSampleData[] _objectiveSamples;
    [SerializeField] private RewardSampleData[] _rewardSamples;

    public void GenerateQuest()
    {
        Quest.Data[] datas = new Quest.Data[_chooseQuestsCount];

        for (int i = 0; i < _chooseQuestsCount; i++)
        {
            Rarity randomDifficulty = GetRandomRarity();
            datas[i] = GenerateQuest(randomDifficulty);
        }

        _selectQuestWindow.DisplayQuests(datas);
    }

    private Quest.Data GenerateQuest(Rarity rarity)
    {
        Quest.Data multiData = new()
        {
            ObjectivesData = CreateObjectives(rarity.ObjectivesCount),
            RewardsData = CreateRewards(rarity.RewardsCount)
        };

        return multiData;
    }

    private QuestObjectiveData[] CreateObjectives(int objectivesCount)
    {
        QuestObjectiveData[] questObjectiveData = GetRandomObjectives(objectivesCount);

        for (int i = 0; i < questObjectiveData.Length; i++)
        {
            SetupRandomObjective(ref questObjectiveData[i]);
        }

        return questObjectiveData;
    }

    private QuestRewardData[] CreateRewards(int rewardsCount)
    {
        QuestRewardData[] questRewardDatas = GetRandomRewards(rewardsCount);

        for (int i = 0; i < questRewardDatas.Length; i++)
        {
            SetupRandomReward(ref questRewardDatas[i]);
        }

        return questRewardDatas;
    }

    private Rarity GetRandomRarity()
    {
        int rarityIndex = GetRandomIndex(i => _questRarities[i].Chance, _questRarities.Length);
        return _questRarities[rarityIndex];
    }

    private void SetupRandomObjective(ref QuestObjectiveData questObjectiveData)
    {
        switch (questObjectiveData.Type)
        {
            case QuestObjectiveType.HitFinish:
                questObjectiveData.IntProperties = new[] { 0 };
                break;
            case QuestObjectiveType.HitPins:
                questObjectiveData.IntProperties = new[] { 0, 0 };
                break;
            case QuestObjectiveType.EarnCoinsByOneBall:
                questObjectiveData.FloatProperties = new[] { 0f };
                break;
            case QuestObjectiveType.EarnCoinsByAllBalls:
                questObjectiveData.FloatProperties = new[] { 0f };
                break;
            case QuestObjectiveType.FallTime:
                questObjectiveData.FloatProperties = new[] { 0f };
                break;
        }
    }

    private void SetupRandomReward(ref QuestRewardData questRewardData)
    {

    }

    private QuestObjectiveData[] GetRandomObjectives(int objectivesCount)
    {
        List<ObjectiveSampleData> objectives = _objectiveSamples.ToList();
        QuestObjectiveData[] questObjectiveDatas = new QuestObjectiveData[objectivesCount];

        for (int i = 0; i < objectivesCount; i++)
        {
            if (objectives.Count == 0)
            {
                break;
            }

            int objectiveIndex = GetRandomIndex(i => objectives[i].Chance, objectives.Count);
            questObjectiveDatas[i] = objectives[objectiveIndex].QuestObjectiveData;
            objectives.RemoveAt(objectiveIndex);
        }

        return questObjectiveDatas;
    }

    private QuestRewardData[] GetRandomRewards(int rewardsCount)
    {
        List<RewardSampleData> rewards = _rewardSamples.ToList();
        QuestRewardData[] questRewardData = new QuestRewardData[rewardsCount];

        for (int i = 0; i < rewardsCount; i++)
        {
            if (rewards.Count == 0)
            {
                break;
            }

            int rewardIndex = GetRandomIndex(i => rewards[i].Chance, rewards.Count);
            questRewardData[i] = rewards[rewardIndex].QuestRewardData;
            rewards.RemoveAt(rewardIndex);
        }

        return questRewardData;
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
    private struct Rarity
    {
        public int ObjectivesCount;
        public int RewardsCount;
        public float Chance;
        public Color Color;
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
