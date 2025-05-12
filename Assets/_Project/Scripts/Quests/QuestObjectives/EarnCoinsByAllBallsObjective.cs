public class EarnCoinsByAllBallsObjective : QuestObjective
{
    private readonly float _coinsCount;

    public float CoinsCount => _coinsCount;

    public EarnCoinsByAllBallsObjective(float coinsCount)
    {
        _coinsCount = coinsCount;
        CurrencyDisplayer.OnTemporaryCoinsChanged += OnBallHitPin;
        BallsController.OnAllBallsFinished += OnAllBallsFinished;
    }

    ~EarnCoinsByAllBallsObjective()
    {
        CurrencyDisplayer.OnTemporaryCoinsChanged -= OnBallHitPin;
        BallsController.OnAllBallsFinished -= OnAllBallsFinished;
    }

    private void OnBallHitPin(float coins)
    {
        ChangeObjectiveProgress(coins / _coinsCount);
    }

    private void OnAllBallsFinished(PlayerBall playerBall, float coins)
    {
        if (coins >= _coinsCount)
        {
            SetCompleted();
        }
    }
}
