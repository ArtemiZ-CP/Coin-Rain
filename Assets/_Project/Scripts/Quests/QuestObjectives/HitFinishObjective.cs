public class HitFinishObjective : QuestObjective
{
    private readonly int _finishMultiplierToHit;

    public int FinishMultiplierToHit => _finishMultiplierToHit;

    public HitFinishObjective(Condition condition, int finishMultiplierToHit) : base(condition)
    {
        _finishMultiplierToHit = finishMultiplierToHit;
        BallsController.OnBallFinished += OnBallFinished;
    }

    ~HitFinishObjective()
    {
        BallsController.OnBallFinished -= OnBallFinished;
    }

    private void OnBallFinished(PlayerBall playerBall, int finishMultiplier, float coint)
    {
        TryComplete(finishMultiplier, _finishMultiplierToHit);
    }
}
