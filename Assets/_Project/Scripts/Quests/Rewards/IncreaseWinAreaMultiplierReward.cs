public class IncreaseWinAreaMultiplierReward : QuestReward
{
    public override void ApplyReward()
    {
        PlayerMapUpgradesData.IncreaseWinAreaMultiplier();
    }
}
