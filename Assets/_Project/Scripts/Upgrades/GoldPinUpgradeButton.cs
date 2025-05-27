public class GoldPinUpgradeButton : UpgradeButton<int>
{
    private const int MaxLevel = 100;
    private const int StartCost = 100;
    private const int StepCost = 50;
    private const int AddValuePerLevel = 1;

    private float _rewardFromPin;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (PlayerPinsData.GoldPinsCount == 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (PlayerPinsData.RewardFromPin != _rewardFromPin)
        {
            SetRewardText();
        }
    }

    protected override int GetDefaultValue()
    {
        return PlayerPinsData.DefaultGoldPinRewardMultiplier;
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
                Value = PlayerPinsData.DefaultGoldPinRewardMultiplier + (i + 1) * AddValuePerLevel
            };
        }

        return upgrades;
    }

    protected override void SetRewardText()
    {
        _rewardFromPin = PlayerPinsData.RewardFromPin;
        SetRewardText($"Gold Pin Reward: x{PlayerPinsData.GoldPinsValue} from Base Pin ({PlayerPinsData.RewardFromPin * PlayerPinsData.GoldPinsValue})");
    }

    protected override void UpdateValue(int value)
    {
        PlayerPinsData.SetGoldPinsValue(value);
    }
}
