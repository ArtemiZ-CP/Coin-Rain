using UnityEngine;

public class GameSpeedUpgradeButton : UpgradeButton<float>
{
    public const float DefaultGameSpeed = 1;

    private const float StartCost = 10f;
    private const float CostMultiplier = 1.2f;
    private const float AdditionalSpeedPerRound = 0.1f;

    protected override float GetDefaultValue()
    {
        return DefaultGameSpeed;
    }

    protected override void UpdateValue(float value)
    {
        PlayerData.SetGameSpeed(value);
    }

    protected override void SetRewardText()
    {
        SetRewardText($"Game speed: {PlayerData.GameSpeedUpgrade.Value}x");
    }

    protected override int LevelUp()
    {
        return PlayerData.GameSpeedUpgradeLevelUp();
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.GameSpeedUpgrade.Level;
        UpgradeData[] upgrades = new UpgradeData[100];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Cost = Mathf.RoundToInt(StartCost * Mathf.Pow(CostMultiplier, i));
            upgrades[i].Value = (i + 1) * AdditionalSpeedPerRound + DefaultGameSpeed;
        }

        return upgrades;
    }
}
