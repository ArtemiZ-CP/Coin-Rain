public class HitFinishObjective : QuestObjective
{
    private readonly int _finishMultiplierToHit;

    public int FinishMultiplierToHit => _finishMultiplierToHit;

    public HitFinishObjective(int finishMultiplierToHit)
    {
        _finishMultiplierToHit = finishMultiplierToHit;
        BallsController.Instance.OnBallFinished += OnBallFinished;
    }

    ~HitFinishObjective()
    {
        BallsController.Instance.OnBallFinished -= OnBallFinished;
    }

    private void OnBallFinished(PlayerBall playerBall, int finishMultiplier, float coint)
    {
        if (finishMultiplier == _finishMultiplierToHit)
        {
            CompleteObjective();
        }
    }
}
