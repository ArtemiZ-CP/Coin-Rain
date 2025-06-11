public class MultiPin : Pin
{
    public const int DefaultReward = 0;
    public const int DefaultMultiplyingBallsCount = 1;

    private int _multiplyingBallsCount;

    public override void Reset()
    {
        base.Reset();
        coinsReward = DefaultReward;
        _multiplyingBallsCount = DefaultMultiplyingBallsCount;
    }

    public override float Touch(PinObject pin, PlayerBall playerBall)
    {
        for (int i = 0; i < _multiplyingBallsCount; i++)
        {
            PlayerBall newBall = BallsController.Instance.SpawnBall(playerBall.Position);
            newBall.SetRandomImpulse();
        }

        return coinsReward;
    }

    public override void Upgrade()
    {
        _multiplyingBallsCount += 1;
    }
}
