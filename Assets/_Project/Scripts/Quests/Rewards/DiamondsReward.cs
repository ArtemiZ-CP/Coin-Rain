public class DiamondsReward : QuestReward
{
    private readonly int _diamondsCount;

    public int CoinsCount => _diamondsCount;

    public DiamondsReward(int coinsCount)
    {
        _diamondsCount = coinsCount;
    }

    public override void ApplyReward()
    {
        PlayerData.AddCoins(_diamondsCount);
    }
}
