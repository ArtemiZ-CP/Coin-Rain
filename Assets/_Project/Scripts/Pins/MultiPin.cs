public class MultiPin : Pin
{
    public const int DefaultReward = 0;

    private static int _multiplyingBallsCount = 1;

    public override void Reset()
    {
        _coinsReward = DefaultReward;
    }

    public override float Touch(PinObject pin, PlayerBall playerBall)
    {
        for (int i = 0; i < _multiplyingBallsCount; i++)
        {
            PlayerBall newBall = BallsController.Instance.SpawnBall(playerBall.Position);
            newBall.SetRandomImpulse();
        }

        return _coinsReward;
    }

    public override void Upgrade()
    {
        _multiplyingBallsCount += 1;
    }
}
