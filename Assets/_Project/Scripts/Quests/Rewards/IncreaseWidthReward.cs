public class IncreaseWidthReward : QuestReward
{
    public override void ApplyReward()
    {
        PlayerMapUpgradesData.IncreaseWidth();
    }
}
