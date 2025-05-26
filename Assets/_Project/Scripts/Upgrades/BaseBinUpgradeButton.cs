public class BaseBinUpgradeButton : UpgradeButton<float>
{
    private const int MaxLevel = 100;
    private const int StartCost = 50;
    private const int StepCost = 25;
    private const float StepValue = 0.1f;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (PlayerMapUpgradesData.IsUpgradeUnlocked == false)
        {
            gameObject.SetActive(false);
        }
    }

    protected override float GetDefaultValue()
    {
        return PlayerPinsData.DefaultRewardFromPin;
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerMapUpgradesData.DefaultLevel;

        UpgradeData[] upgrades = new UpgradeData[MaxLevel];

        for (int i = 0; i < MaxLevel; i++)
        {
            upgrades[i] = new UpgradeData
            {
                Cost = StartCost + i * StepCost,
                Value = PlayerPinsData.DefaultRewardFromPin + (i + 1) * StepValue
            };
        }

        return upgrades;
    }

    protected override void SetRewardText()
    {
        SetRewardText($"Reward from Bin: {PlayerPinsData.RewardFromPin}");
    }

    protected override void UpdateValue(float value)
    {
        PlayerPinsData.SetRewardFromPin(value);
    }
}
