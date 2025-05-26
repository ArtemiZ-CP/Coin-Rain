public class DiamondsReward : QuestReward
{
    private readonly int _diamondsCount;

    public int DiamondsCount => _diamondsCount;

    public DiamondsReward(int coinsCount)
    {
        _diamondsCount = coinsCount;
    }

    public override void ApplyReward()
    {
        PlayerCurrencyData.AddCoins(_diamondsCount);
    }
}
