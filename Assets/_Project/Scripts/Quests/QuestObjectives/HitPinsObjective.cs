public class HitPinsObjective : QuestObjective
{
    private readonly int _pinsCount;

    private int _currentPinsCount = 0;

    public int PinsCount => _pinsCount;

    public HitPinsObjective(int pinsCount)
    {
        _pinsCount = pinsCount;
        BallsController.Instance.OnBallHitPin += OnBallHitPin;
        BallsController.Instance.OnReset += OnReset;
    }

    ~HitPinsObjective()
    {
        BallsController.Instance.OnReset -= OnReset;
    }

    private void OnBallHitPin(PlayerBall playerBall)
    {
        _currentPinsCount++;

        if (_currentPinsCount >= _pinsCount)
        {
            CompleteObjective();
        }
    }

    private void OnReset()
    {
        _currentPinsCount = 0;
    }
}
