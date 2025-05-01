using UnityEngine;

public class GoldPinUpgradeButton : UpgradeButton<int>
{
    private const int MaxLevel = 10;
    private const int StartCost = 100;
    private const int AddValuePerLevel = 1;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (PlayerData.GoldPinsCountUpgrade == 0)
        {
            gameObject.SetActive(false);
        }
    }

    protected override int GetDefaultValue()
    {
        return PlayerData.DefaultGoldPinRewardMultiplier;
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.DefaultLevel;

        UpgradeData[] upgrades = new UpgradeData[MaxLevel];

        for (int i = 0; i < MaxLevel; i++)
        {
            upgrades[i] = new UpgradeData
            {
                Cost = Mathf.Pow(2, i) * StartCost,
                Value = PlayerData.DefaultGoldPinRewardMultiplier + (i + 1) * AddValuePerLevel
            };
        }

        return upgrades;
    }

    protected override void SetRewardText()
    {
        SetRewardText($"Gold Pin Reward: x{PlayerData.GoldPinsValueUpgrade} from Base Pin ({PlayerData.RewardFromPin * PlayerData.GoldPinsValueUpgrade})");
    }

    protected override void UpdateValue(int value)
    {
        PlayerData.SetGoldPinsValue(value);
    }
}
