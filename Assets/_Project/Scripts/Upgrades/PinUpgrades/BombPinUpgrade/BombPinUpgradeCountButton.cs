using UnityEngine;

public class BombPinUpgradeCountButton : UpgradeButton<int>
{
    private const float StartCost = 10f;
    private const float CostMultiplier = 1.2f;

    [SerializeField] private BombPinUpgradeAmountButton _bombPinUpgradeAmountButton;

    protected override int GetDefaultValue()
    {
        return PlayerData.DefaultPinsCount;
    }

    protected override void UpdateValue(int value)
    {
        PlayerData.SetBombPinsCount(value);
    }

    protected override void SetRewardText()
    {
        if (PlayerData.BombPinsCountUpgrade.Value == 0)
        {
            SetRewardText("Unlock Bomb Pins");
            _bombPinUpgradeAmountButton.gameObject.SetActive(false);
        }
        else
        {
            SetRewardText($"Count: {PlayerData.BombPinsCountUpgrade.Value}");
            _bombPinUpgradeAmountButton.gameObject.SetActive(true);
        }
    }

    protected override int LevelUp()
    {
        return PlayerData.BombPinCountUpgradeLevelUp();
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.BombPinsCountUpgrade.Level;
        UpgradeData[] upgrades = new UpgradeData[100];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Cost = Mathf.RoundToInt(StartCost * Mathf.Pow(CostMultiplier, i));
            upgrades[i].Value = i + PlayerData.DefaultPinsCount + 1;
        }

        return upgrades;
    }
}
