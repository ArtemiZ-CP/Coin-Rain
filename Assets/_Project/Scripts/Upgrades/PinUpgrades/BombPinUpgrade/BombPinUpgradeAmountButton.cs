using UnityEngine;

public class BombPinUpgradeAmountButton : UpgradeButton<int>
{
    public const int DefaultBombPinValue = 1;

    private const float StartCost = 10f;
    private const float CostMultiplier = 1.2f;

    protected override int GetDefaultValue()
    {
        return DefaultBombPinValue;
    }

    protected override void UpdateValue(int value)
    {
        PlayerData.SetBombPinsValue(value);
    }

    protected override void SetRewardText()
    {
        SetRewardText($"Explosion range: {PlayerData.MultiPinsValueUpgrade.Value}");
    }

    protected override int LevelUp()
    {
        return PlayerData.BombPinValueUpgradeLevelUp();
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.BombPinsValueUpgrade.Level;
        UpgradeData[] upgrades = new UpgradeData[100];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Cost = Mathf.RoundToInt(StartCost * Mathf.Pow(CostMultiplier, i));
            upgrades[i].Value = i + DefaultBombPinValue + 1;
        }

        return upgrades;
    }
}
