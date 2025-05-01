using System;

[Serializable]
public struct QuestRewardData
{
    public QuestRewardType Type;
    public int CoinsCount;
    public UpgradeType UpgradeType;
}

public abstract class QuestReward
{
    public abstract void ApplyReward();
}

public enum QuestRewardType
{
    Coins,
    UnlockUpgrade,
}

public enum UpgradeType
{
    Base,
    Gold,
}