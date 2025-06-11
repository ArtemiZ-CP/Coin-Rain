public class BasePin : Pin
{
    private const int DefaultReward = 1;

    public override void Reset()
    {
        base.Reset();
        coinsReward = DefaultReward;
        count = int.MaxValue;
    }

    public override float Touch(PinObject pin, PlayerBall playerBall)
    {
        return coinsReward;
    }

    public override void Upgrade()
    {
        IncreaseReward(1);
    }
}
