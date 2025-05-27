using System.Collections.Generic;
using UnityEngine;

public static class ObjectiveGenerator
{
    private const float Offset = 0.05f;

    public static void SetupRandomObjectives(List<QuestObjectiveData> questObjectiveData)
    {
        questObjectiveData.Sort((x, y) => x.Type.CompareTo(y.Type));

        int finishMultiplier = -1;

        for (int i = 0; i < questObjectiveData.Count; i++)
        {
            questObjectiveData[i] = SetupObjective(questObjectiveData[i], ref finishMultiplier);
        }
    }

    private static QuestObjectiveData SetupObjective(QuestObjectiveData questObjectiveData, ref int finishMultiplier)
    {
        switch (questObjectiveData.Type)
        {
            case QuestObjectiveType.HitFinish:
                questObjectiveData.IntProperties = GetHitFinishObjectiveData(ref finishMultiplier);
                break;
            case QuestObjectiveType.HitPins:
                questObjectiveData.IntProperties = GetHitPinsObjectiveData(questObjectiveData.Condition);
                break;
            case QuestObjectiveType.EarnCoinsByOneBall:
                questObjectiveData.FloatProperties = GetEarnCoinsByOneBallObjectiveData();
                break;
            case QuestObjectiveType.EarnCoinsByAllBalls:
                questObjectiveData.FloatProperties = GetEarnCoinsByAllBallsObjectiveData();
                break;
            case QuestObjectiveType.FallTime:
                questObjectiveData.FloatProperties = GetFallTimeObjectiveData();
                break;
        }

        return questObjectiveData;
    }

    public static int[] GetHitFinishObjectiveData(ref int finishMultiplier)
    {
        int minFinishMultiplier = PlayerMapUpgradesData.MinFinishMultiplier;
        int maxFinishMultiplier = minFinishMultiplier + PlayerMapUpgradesData.MapWidth;

        if (minFinishMultiplier == 0 && maxFinishMultiplier > 0)
        {
            minFinishMultiplier = 1;
        }

        finishMultiplier = Random.Range(minFinishMultiplier, maxFinishMultiplier + 1);

        return new[] { finishMultiplier };
    }

    public static int[] GetHitPinsObjectiveData(Condition condition)
    {
        List<(Pin.Type Type, int Count)> pinTypes = PinsMap.Instance.GetPinsCountByType();
        (Pin.Type Type, int Count) = pinTypes[Random.Range(0, pinTypes.Count)];
        int pinsCount = PlayerStatisticData.GetAveragePinHitsByType(Type);

        if (pinsCount == 0)
        {
            pinsCount = Count / 2;
        }

        pinsCount = GetValueBasedOnCondition(pinsCount, Count, condition);

        return new[] { pinsCount, (int)Type };
    }

    public static float[] GetEarnCoinsByOneBallObjectiveData()
    {
        return new[] { 30f };
    }

    public static float[] GetEarnCoinsByAllBallsObjectiveData()
    {
        return new[] { 30f };
    }

    public static float[] GetFallTimeObjectiveData()
    {
        return new[] { 3f };
    }

    private static int GetValueBasedOnCondition(int value, int maxValue, Condition condition)
    {
        if (condition == Condition.Less)
        {
            value += Mathf.RoundToInt(maxValue * Offset);
        }
        else if (condition == Condition.More)
        {
            value -= Mathf.RoundToInt(maxValue * Offset);
        }

        return value;
    }
}
