public class EarnCoinsObjective : QuestObjective
{
    private readonly float _coinsCount;

    public float CoinsCount => _coinsCount;

    public EarnCoinsObjective(float coinsCount)
    {
        _coinsCount = coinsCount;
        BallsController.Instance.OnBallFinished += OnBallFinished;
    }

    ~EarnCoinsObjective()
    {
        BallsController.Instance.OnBallFinished -= OnBallFinished;
    }

    private void OnBallFinished(PlayerBall playerBall, int finishMultiplier, float coins)
    {
        if (coins >= _coinsCount)
        {
            SetCompleted();
        }
    }
}
