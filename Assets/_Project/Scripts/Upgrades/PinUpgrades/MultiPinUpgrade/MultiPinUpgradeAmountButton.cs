using UnityEngine;

public class MultiPinUpgradeAmountButton : UpgradeButton<int>
{
    public const int DefaultMultiPinsValue = 1;

    private const float StartCost = 10f;
    private const float CostMultiplier = 1.2f;

    protected override int GetDefaultValue()
    {
        return DefaultMultiPinsValue;
    }

    protected override void UpdateValue(int value)
    {
        PlayerData.SetMultiPinsValue(value);
    }

    protected override void SetRewardText()
    {
        SetRewardText($"Extra balls: {PlayerData.MultiPinsValueUpgrade.Value}");
    }

    protected override int LevelUp()
    {
        return PlayerData.MultiPinValueUpgradeLevelUp();
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.MultiPinsValueUpgrade.Level;
        UpgradeData[] upgrades = new UpgradeData[100];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Cost = Mathf.RoundToInt(StartCost * Mathf.Pow(CostMultiplier, i));
            upgrades[i].Value = i + DefaultMultiPinsValue + 1;
        }

        return upgrades;
    }
}
