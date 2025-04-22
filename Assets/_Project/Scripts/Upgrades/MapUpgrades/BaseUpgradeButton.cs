using UnityEngine;

public class BaseUpgradeButton : UpgradeButton<float>
{
    public const float DefaultBaseCoinsRewardFromPin = 1;
    
    private const float StartCost = 10f;
    private const float CostMultiplier = 1.2f;

    protected override float GetDefaultValue()
    {
        return DefaultBaseCoinsRewardFromPin;
    }

    protected override void UpdateValue(float value)
    {
        PlayerData.SetBaseCoinsRewardFromPin(value);
    }

    protected override void SetRewardText()
    {
        SetRewardText($"Base reward: {PlayerData.BaseUpgrade.Value} coins");
    }

    protected override int LevelUp()
    {
        return PlayerData.BaseUpgradeLevelUp();
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.BaseUpgrade.Level;
        UpgradeData[] upgrades = new UpgradeData[100];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Cost = Mathf.RoundToInt(StartCost * Mathf.Pow(CostMultiplier, i));
            upgrades[i].Value = i + DefaultBaseCoinsRewardFromPin + 1;
        }

        return upgrades;
    }
}
