public class EarnCoinsObjective : QuestObjective
{
    private readonly float _coinsCount;

    public float CoinsCount => _coinsCount;

    public EarnCoinsObjective(Condition condition, float coinsCount) : base(condition)
    {
        _coinsCount = coinsCount;
        PlayerBall.OnBallHitPin += OnBallHitPin;
        BallsController.OnBallFinished += OnBallFinished;
    }

    ~EarnCoinsObjective()
    {
        PlayerBall.OnBallHitPin -= OnBallHitPin;
        BallsController.OnBallFinished -= OnBallFinished;
    }

    private void OnBallHitPin(PlayerBall playerBall, Pin.Type pinType)
    {
        float maxCoins = 0;

        foreach (var ballInfo in CurrencyDisplayer.TemporaryCoins)
        {
            if (ballInfo.Value > maxCoins)
            {
                maxCoins = ballInfo.Value;
            }
        }

        ChangeObjectiveProgress(maxCoins / _coinsCount);
    }

    private void OnBallFinished(PlayerBall playerBall, int finishMultiplier, float coins)
    {
        TryComplete(coins, _coinsCount);
    }
}
