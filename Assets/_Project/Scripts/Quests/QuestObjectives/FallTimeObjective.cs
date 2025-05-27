using UnityEngine;

public class FallTimeObjective : QuestObjective
{
    private readonly float _timer;

    private float _startTime;

    public float Timer => _timer;

    public FallTimeObjective(Condition condition, float coinsCount) : base(condition)
    {
        _timer = coinsCount;
        BallsController.OnFixedUpdate += UpdateTimer;
    }

    ~FallTimeObjective()
    {
        BallsController.OnFixedUpdate -= UpdateTimer;
    }

    public override void OnBallDropped(PlayerBall playerBall)
    {
        base.OnBallDropped(playerBall);
        _startTime = Time.time;
        ChangeObjectiveProgress(0);
    }

    private void UpdateTimer()
    {
        TryComplete(Time.time - _startTime, _timer);
    }
}
