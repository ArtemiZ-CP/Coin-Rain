using System;

[Serializable]
public struct MultiQuestRewardData
{
    public QuestRewardData[] QuestRewardDatas;
}

[Serializable]
public struct QuestRewardData
{
    public QuestRewardType Type;
    public int[] IntProperties;
}

public abstract class QuestReward
{
    public abstract void ApplyReward();
}

public enum QuestRewardType
{
    Coins,
    Diamonds,
    UnlockUpgrade,
    IncreaseHeight,
    IncreaseWidth,
    IncreaseWinAreaMultiplier,
}