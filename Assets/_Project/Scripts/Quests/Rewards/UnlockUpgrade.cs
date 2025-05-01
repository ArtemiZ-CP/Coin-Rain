public class UnlockUpgrade : QuestReward
{
    private readonly UpgradeType _upgradeType;

    public UpgradeType UpgradeType => _upgradeType;

    public UnlockUpgrade(UpgradeType upgradeType)
    {
        _upgradeType = upgradeType;
    }

    public override void ApplyReward()
    {
        PlayerData.UnlockUpgrade(_upgradeType);
    }
}
