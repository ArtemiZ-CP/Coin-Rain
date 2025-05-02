public class BaseBinUpgradeButton : UpgradeButton<float>
{
    private const int MaxLevel = 10;
    private const int StartCost = 50;
    private const int StepCost = 25;
    private const float StepValue = 0.1f;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (PlayerData.IsUpgradeUnlocked == false)
        {
            gameObject.SetActive(false);
        }
    }

    protected override float GetDefaultValue()
    {
        return PlayerData.DefaultRewardFromPin;
    }

    protected override UpgradeData[] GetUpgrades(out int level)
    {
        level = PlayerData.DefaultLevel;

        UpgradeData[] upgrades = new UpgradeData[MaxLevel];

        for (int i = 0; i < MaxLevel; i++)
        {
            upgrades[i] = new UpgradeData
            {
                Cost = StartCost + i * StepCost,
                Value = PlayerData.DefaultRewardFromPin + (i + 1) * StepValue
            };
        }

        return upgrades;
    }

    protected override void SetRewardText()
    {
        SetRewardText($"Reward from Bin: {PlayerData.RewardFromPin}");
    }

    protected override void UpdateValue(float value)
    {
        PlayerData.SetRewardFromPin(value);
    }
}
