using UnityEngine;

public class GoldPinUpgradeCountButton : UpgradeButton<int>
{
    private const float StartCost = 10f;
    private const float CostMultiplier = 1.2f;

    [SerializeField] private GoldPinUpgradeAmountButton _goldPinUpgradeAmountButton;

    protected override int GetDefaultValue()
    {
        return PlayerData.DefaultPinsCount;
    }

    protected override void UpdateValue(int value)
    {
        PlayerData.SetGoldPinsCount(value);
    }

    protected override void SetRewardText()
    {
        if (PlayerData.GoldPinsCountUpgrade.Value == 0)
        {
            SetRewardText("Unlock Gold Pins");
            _goldPinUpgradeAmountButton.gameObject.SetActive(false);
        }
        else
        {
            SetRewardText($"Count: {PlayerData.GoldPinsCountUpgrade.Value}");
            _goldPinUpgradeAmountButton.gameObject.SetActive(true);
        }
    }

    protected override int LevelUp()
    {
        return PlayerData.GoldPinCountUpgradeLevelUp();
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.GoldPinsCountUpgrade.Level;
        UpgradeData[] upgrades = new UpgradeData[100];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Cost = Mathf.RoundToInt(StartCost * Mathf.Pow(CostMultiplier, i));
            upgrades[i].Value = i + PlayerData.DefaultPinsCount + 1;
        }

        return upgrades;
    }
}
