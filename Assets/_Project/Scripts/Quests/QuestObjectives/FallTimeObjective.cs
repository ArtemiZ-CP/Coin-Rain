using System.Security.Cryptography;
using UnityEngine;

public class FallTimeObjective : QuestObjective
{
    private readonly float _timer;

    private float _startTime;

    public float Timer => _timer;

    public FallTimeObjective(float coinsCount)
    {
        _timer = coinsCount;
        BallsController.OnAllBallsFinished += OnBallsFinished;
        BallsController.OnFixedUpdate += UpdateTimer;
    }

    ~FallTimeObjective()
    {
        BallsController.OnAllBallsFinished -= OnBallsFinished;
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
        float time = Time.time - _startTime;
        ChangeObjectiveProgress(time / _timer);
    }

    private void OnBallsFinished(PlayerBall playerBall, float coins)
    {
        float time = Time.time - _startTime;

        if (time < _timer)
        {
            SetCompleted();
        }
    }
}
