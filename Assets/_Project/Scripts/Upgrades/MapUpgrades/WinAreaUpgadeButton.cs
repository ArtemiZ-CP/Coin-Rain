using UnityEngine;

public class WinAreaUpgadeButton : UpgradeButton<int>
{
    public const int DefaultWinAreasUpgrade = 0;

    private const float StartCost = 10f;
    private const float CostMultiplier = 1.2f;

    protected override int GetDefaultValue()
    {
        return DefaultWinAreasUpgrade;
    }

    protected override void UpdateValue(int value)
    {
        PlayerData.SetWinAreasUpgrade(value);
    }

    protected override void SetRewardText()
    {
        SetRewardText($"Finish level: {PlayerData.WinAreasUpgrade.Value + 1}");
    }

    protected override int LevelUp()
    {
        return PlayerData.WinAreasUpgradeLevelUp();
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.WinAreasUpgrade.Level;
        UpgradeData[] upgrades = new UpgradeData[100];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Cost = Mathf.RoundToInt(StartCost * Mathf.Pow(CostMultiplier, i));
            upgrades[i].Value = i + DefaultWinAreasUpgrade + 1;

        }

        return upgrades;
    }
}
