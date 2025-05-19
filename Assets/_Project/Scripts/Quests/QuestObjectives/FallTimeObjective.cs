using UnityEngine;

public class FallTimeObjective : QuestObjective
{
    private readonly float _timer;

    private float _startTime;

    public float Timer => _timer;

    public FallTimeObjective(float coinsCount)
    {
        _timer = coinsCount;
        BallsController.OnBallDropped += StartTimer;
        BallsController.OnFixedUpdate += UpdateTimer;
        BallsController.OnAllBallsFinished += OnBallsFinished;
    }

    ~FallTimeObjective()
    {
        BallsController.OnBallDropped -= StartTimer;
        BallsController.OnFixedUpdate -= UpdateTimer;
        BallsController.OnAllBallsFinished -= OnBallsFinished;
    }

    private void StartTimer(PlayerBall playerBall)
    {
        _startTime = Time.time;
        ChangeObjectiveProgress(0);
    }

    private void UpdateTimer()
    {
        float time = _startTime - Time.time;
        ChangeObjectiveProgress(time / _timer);
    }

    private void OnBallsFinished(PlayerBall playerBall, float coins)
    {
        float time = _startTime - Time.time;

        if (time < _timer)
        {
            SetCompleted();
        }
    }
}
