using UnityEngine;

public class HeightUpgradeButton : UpgradeButton<int>
{
    public const int DefaultHeight = 3;

    private const float StartCost = 10f;
    private const float CostMultiplier = 1.2f;

    protected override int GetDefaultValue()
    {
        return DefaultHeight;
    }

    protected override void UpdateValue(int value)
    {
        PlayerData.SetHeight(value);
    }

    protected override void SetRewardText()
    {
        SetRewardText($"Map height: {PlayerData.HeightUpgrade.Value}");
    }

    protected override int LevelUp()
    {
        return PlayerData.HeightUpgradeLevelUp();
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.HeightUpgrade.Level;
        UpgradeData[] upgrades = new UpgradeData[100];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Cost = Mathf.RoundToInt(StartCost * Mathf.Pow(CostMultiplier, i));
            upgrades[i].Value = i + DefaultHeight + 1;
        }

        return upgrades;
    }
}
