public class HitPinsObjective : QuestObjective
{
    private readonly int _pinsCount;
    private readonly Pin.Type _pinType;

    private int _currentPinsCount = 0;

    public int PinsCount => _pinsCount;
    public Pin.Type PinType => _pinType;

    public HitPinsObjective(Condition condition, int pinsCount, Pin.Type pinType) : base(condition)
    {
        _pinsCount = pinsCount;
        _pinType = pinType;
        PlayerBall.OnBallHitPin += OnBallHitPin;
    }

    ~HitPinsObjective()
    {
        PlayerBall.OnBallHitPin -= OnBallHitPin;
    }

    private void OnBallHitPin(PlayerBall playerBall, Pin.Type type)
    {
        if (type != _pinType)
        {
            return;
        }

        _currentPinsCount++;
        TryComplete(_currentPinsCount, _pinsCount);
    }

    public override void OnReset()
    {
        _currentPinsCount = 0;
        base.OnReset();
    }
}
