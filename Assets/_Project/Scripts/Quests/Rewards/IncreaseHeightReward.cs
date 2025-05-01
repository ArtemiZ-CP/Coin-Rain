public class IncreaseHeightReward : QuestReward
{
    public override void ApplyReward()
    {
        PlayerData.IncreaseHeight();
    }
}
