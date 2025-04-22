using UnityEngine;

public class MultiPinUpgradeCountButton : UpgradeButton<int>
{
    private const float StartCost = 10f;
    private const float CostMultiplier = 1.2f;

    [SerializeField] private MultiPinUpgradeAmountButton _multiPinUpgradeAmountButton;

    protected override int GetDefaultValue()
    {
        return PlayerData.DefaultPinsCount;
    }

    protected override void UpdateValue(int value)
    {
        PlayerData.SetMultiPinsCount(value);
    }

    protected override void SetRewardText()
    {
        if (PlayerData.MultiPinsCountUpgrade.Value == 0)
        {
            SetRewardText("Unlock Multiplying Pins");
            _multiPinUpgradeAmountButton.gameObject.SetActive(false);
        }
        else
        {
            SetRewardText($"Count: {PlayerData.MultiPinsCountUpgrade.Value}");
            _multiPinUpgradeAmountButton.gameObject.SetActive(true);
        }
    }

    protected override int LevelUp()
    {
        return PlayerData.MultiPinCountUpgradeLevelUp();
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.MultiPinsCountUpgrade.Level;
        UpgradeData[] upgrades = new UpgradeData[100];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Cost = Mathf.RoundToInt(StartCost * Mathf.Pow(CostMultiplier, i));
            upgrades[i].Value = i + PlayerData.DefaultPinsCount + 1;
        }

        return upgrades;
    }
}
