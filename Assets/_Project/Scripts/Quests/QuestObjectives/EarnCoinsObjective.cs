public class EarnCoinsObjective : QuestObjective
{
    private readonly float _coinsCount;

    public float CoinsCount => _coinsCount;

    public EarnCoinsObjective(float coinsCount)
    {
        _coinsCount = coinsCount;
        BallsController.OnBallHitPin += OnBallHitPin;
        BallsController.OnBallFinished += OnBallFinished;
    }

    ~EarnCoinsObjective()
    {
        BallsController.OnBallHitPin -= OnBallHitPin;
        BallsController.OnBallFinished -= OnBallFinished;
    }

    private void OnBallHitPin(PlayerBall playerBall, Pin.Type pinType)
    {
        float coins = CurrencyDisplayer.TemporaryCoins[playerBall];
        ChangeObjectiveProgress(coins / _coinsCount);
    }

    private void OnBallFinished(PlayerBall playerBall, int finishMultiplier, float coins)
    {
        if (coins >= _coinsCount)
        {
            SetCompleted();
        }
    }
}
