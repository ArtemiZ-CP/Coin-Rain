public class HitPinsObjective : QuestObjective
{
    private readonly int _pinsCount;
    private readonly Pin.Type _pinType;

    private int _currentPinsCount = 0;

    public int PinsCount => _pinsCount;
    public Pin.Type PinType => _pinType;

    public HitPinsObjective(int pinsCount, Pin.Type pinType)
    {
        _pinsCount = pinsCount;
        _pinType = pinType;
        BallsController.Instance.OnBallHitPin += OnBallHitPin;
    }

    ~HitPinsObjective()
    {
        BallsController.Instance.OnBallHitPin -= OnBallHitPin;
    }

    private void OnBallHitPin(PlayerBall playerBall, Pin.Type type)
    {
        if (type != _pinType)
        {
            return;
        }

        _currentPinsCount++;

        if (_currentPinsCount >= _pinsCount)
        {
            SetCompleted();
        }
    }

    public override void OnReset()
    {
        _currentPinsCount = 0;
        base.OnReset();
    }
}
