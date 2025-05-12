using System;

[Serializable]
public struct QuestRewardData
{
    public QuestRewardType Type;
    public int CoinsCount;
    public Pin.Type PinType;
    public float IncreaseValue;
}

public abstract class QuestReward
{
    public abstract void ApplyReward();
}

public enum QuestRewardType
{
    Coins,
    UnlockUpgrade,
    IncreaseHeight,
    IncreaseWidth,
    IncreaseBallSize,
    IncreaseWinAreaMultiplier,
}