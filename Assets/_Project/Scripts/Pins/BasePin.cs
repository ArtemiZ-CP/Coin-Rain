public class BasePin : Pin
{
    private const int DefaultReward = 1;

    public BasePin()
    {
        _count = int.MaxValue;
    }

    public override void Reset()
    {
        _coinsReward = DefaultReward;
    }

    public override float Touch(PinObject pin, PlayerBall playerBall)
    {
        return _coinsReward;
    }

    public override void Upgrade()
    {
        IncreaseReward(1);
    }
}
