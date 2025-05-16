using System;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    [SerializeField] private SelectQuestWindow _selectQuestWindow;
    [SerializeField] private float[] _difficulties;
    [SerializeField] private MultiQuestObjectiveData[] _samples;

    public void GenerateQuest()
    {
        Quest.Data[] datas = new Quest.Data[_difficulties.Length];

        for (int i = 0; i < _difficulties.Length; i++)
        {
            datas[i] = GenerateQuest(_difficulties[i]);
        }

        _selectQuestWindow.DisplayQuests(datas);
    }

    private Quest.Data GenerateQuest(float difficulty)
    {
        return new Quest.Data()
        {
            ObjectivesData = CreateObjective(difficulty),
            RewardData = CreateReward(difficulty)
        };
    }

    private QuestObjectiveData CreateObjective(float difficulty)
    {
        int typesCount = Enum.GetNames(typeof(QuestObjectiveType)).Length;
        QuestObjectiveType randomType = (QuestObjectiveType)UnityEngine.Random.Range(0, typesCount);

        QuestObjectiveData questObjectiveData = new()
        {
            Type = randomType
        };

        switch (randomType)
        {
            case QuestObjectiveType.HitFinish:
                break;
        }

        return questObjectiveData;
    }

    private QuestRewardData CreateReward(float difficulty)
    {
        int typesCount = Enum.GetNames(typeof(QuestRewardType)).Length;
        QuestRewardType randomType = (QuestRewardType)UnityEngine.Random.Range(0, typesCount);

        QuestRewardData questRewardData = new()
        {
            Type = randomType
        };


        switch (randomType)
        {
            case QuestRewardType.Coins:
                break;
        }

        return questRewardData;
    }
}
