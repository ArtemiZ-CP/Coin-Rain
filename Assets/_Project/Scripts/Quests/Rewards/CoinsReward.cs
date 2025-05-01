public class CoinsReward : QuestReward
{
    private readonly int _coinsCount;

    public int CoinsCount => _coinsCount;

    public CoinsReward(int coinsCount)
    {
        _coinsCount = coinsCount;
    }

    public override void ApplyReward()
    {
        PlayerData.AddCoins(_coinsCount);
    }
}
