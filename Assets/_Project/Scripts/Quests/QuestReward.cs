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

    public string GetDescriptionText()
    {
        if (this is CoinsReward coinsReward)
        {
            return $"{coinsReward.CoinsCount} монет";
        }
        else if (this is UnlockUpgrade unlockUpgrade)
        {
            if (unlockUpgrade.PinType == Pin.Type.Base)
            {
                return "Открыть улучшения";
            }
            else if (unlockUpgrade.PinType == Pin.Type.Gold)
            {
                return "Добавить золотой штырёк";
            }
            else if (unlockUpgrade.PinType == Pin.Type.Multiplying)
            {
                return "Добавить множитель";
            }
            else if (unlockUpgrade.PinType == Pin.Type.Bomb)
            {
                return "Добавить бомбу";
            }
            else
            {
                return "Неизвестная награда";
            }
        }
        else if (this is IncreaseHeightReward)
        {
            return "Добавить линию штырьков";
        }
        else if (this is IncreaseWidthReward)
        {
            return "Увеличить ширину линии штырьков";
        }
        else if (this is IncreaseWinAreaMultiplierReward)
        {
            return $"Финальный множитель + 1";
        }
        else
        {
            return "Неизвестная награда";
        }
    }
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