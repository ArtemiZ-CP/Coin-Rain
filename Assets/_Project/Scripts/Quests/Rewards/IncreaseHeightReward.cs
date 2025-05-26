public class IncreaseHeightReward : QuestReward
{
    public override void ApplyReward()
    {
        PlayerMapUpgradesData.IncreaseHeight();
    }
}
