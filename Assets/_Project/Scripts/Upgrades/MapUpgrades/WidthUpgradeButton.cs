using UnityEngine;

public class WidthUpgradeButton : UpgradeButton<int>
{
    public const int DefaultWidth = 1;

    private const float StartCost = 10f;
    private const float CostMultiplier = 1.2f;

    protected override int GetDefaultValue()
    {
        return DefaultWidth;
    }

    protected override void UpdateValue(int value)
    {
        PlayerData.SetWidth(value);
    }

    protected override void SetRewardText()
    {
        SetRewardText($"Map width: {PlayerData.WidthUpgrade.Value}");
    }

    protected override int LevelUp()
    {
        return PlayerData.WidthUpgradeLevelUp();
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.WidthUpgrade.Level;
        UpgradeData[] upgrades = new UpgradeData[100];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Cost = Mathf.RoundToInt(StartCost * Mathf.Pow(CostMultiplier, i));
            upgrades[i].Value = i + DefaultWidth + 1;
        }

        return upgrades;
    }
}
