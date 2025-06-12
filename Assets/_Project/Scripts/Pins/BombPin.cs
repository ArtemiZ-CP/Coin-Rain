public class BombPin : Pin
{
    public const int DefaultReward = 1;
    public const int DefaultBlastImpulse = 10;
    public const int DefaultBlastRange = 1;

    private int _blastRange;
    private int _blastImpulse;

    public override void Reset()
    {
        base.Reset();
        coinsReward = DefaultReward;
        _blastImpulse = DefaultBlastImpulse;
        _blastRange = DefaultBlastRange;
    }

    public override float Touch(PinObject pin, PlayerBall playerBall)
    {
        return BallsController.Instance.Blast(pin, playerBall, _blastRange, _blastImpulse, coinsReward);
    }

    public override void Upgrade()
    {
        _blastRange++;
        base.Upgrade();
    }
}