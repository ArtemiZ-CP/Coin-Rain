public class IncreaseWinAreaMultiplierReward : QuestReward
{
    public override void ApplyReward()
    {
        PlayerData.IncreaseWinAreaMultiplier();
    }
}
