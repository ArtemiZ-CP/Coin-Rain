public class IncreaseWidthReward : QuestReward
{
    public override void ApplyReward()
    {
        PlayerData.IncreaseWidth();
    }
}
