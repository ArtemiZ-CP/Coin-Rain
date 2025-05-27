public class EarnCoinsByAllBallsObjective : QuestObjective
{
    private readonly float _coinsCount;

    public float CoinsCount => _coinsCount;

    public EarnCoinsByAllBallsObjective(Condition condition, float coinsCount) : base(condition)
    {
        _coinsCount = coinsCount;
        CurrencyDisplayer.OnTemporaryCoinsChanged += OnBallHitPin;
    }

    ~EarnCoinsByAllBallsObjective()
    {
        CurrencyDisplayer.OnTemporaryCoinsChanged -= OnBallHitPin;
    }

    private void OnBallHitPin(float coins)
    {
        TryComplete(coins, _coinsCount);
    }
}
