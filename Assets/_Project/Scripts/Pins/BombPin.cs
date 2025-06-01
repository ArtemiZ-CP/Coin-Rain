public class BombPin : Pin
{
    public const int DefaultReward = 1;

    private static int _blastRange = 1;

    public static int BlastRange => _blastRange;

    public override void Reset()
    {
        coinsReward = DefaultReward;
    }

    public override float Touch(PinObject pin, PlayerBall playerBall)
    {
        return BallsController.Instance.Blast(pin, playerBall, _blastRange, coinsReward);
    }

    public override void Upgrade()
    {
        _blastRange++;
    }
}