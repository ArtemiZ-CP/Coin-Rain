public class GoldPin : Pin
{
    public const int DefaultReward = 2;

    public override void Reset()
    {
        coinsReward = DefaultReward;
    }

    public override float Touch(PinObject pin, PlayerBall playerBall)
    {
        return coinsReward;
    }

    public override void Upgrade()
    {
        IncreaseReward(2);
    }
}
