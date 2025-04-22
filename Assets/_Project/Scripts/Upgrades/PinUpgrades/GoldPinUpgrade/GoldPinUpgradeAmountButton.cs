using UnityEngine;

public class GoldPinUpgradeAmountButton : UpgradeButton<int>
{
    public const int DefaultGoldPinRewardMultiplier = 2;

    private const float StartCost = 10f;
    private const float CostMultiplier = 1.2f;

    protected override int GetDefaultValue()
    {
        return DefaultGoldPinRewardMultiplier;
    }

    protected override void UpdateValue(int value)
    {
        PlayerData.SetGoldPinsValue(value);
    }

    protected override void SetRewardText()
    {
        SetRewardText($"Reward: x{PlayerData.GoldPinsValueUpgrade.Value}");
    }

    protected override int LevelUp()
    {
        return PlayerData.GoldPinValueUpgradeLevelUp();
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.GoldPinsValueUpgrade.Level;
        UpgradeData[] upgrades = new UpgradeData[100];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Cost = Mathf.RoundToInt(StartCost * Mathf.Pow(CostMultiplier, i));
            upgrades[i].Value = i + DefaultGoldPinRewardMultiplier + 1;
        }

        return upgrades;
    }
}
