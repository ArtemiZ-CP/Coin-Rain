public class GoldPin : Pin
{
    public const int DefaultRewardMultiplier = 2;

    private float _rewardMultiplier = DefaultRewardMultiplier;

    public GoldPin(BasePin basePin)
    {
        basePin.OnPinRewardUpdate += UpdateCoinsReward;
    }

    ~GoldPin()
    {
        Get(Type.Base).OnPinRewardUpdate -= UpdateCoinsReward;
    }

    public override void Reset()
    {
        base.Reset();
        UpdateCoinsReward();
    }

    public override float Touch(PinObject pin, PlayerBall playerBall)
    {
        return coinsReward;
    }

    public override void Upgrade()
    {
        _rewardMultiplier++;
        UpdateCoinsReward();
        base.Upgrade();
    }

    private void UpdateCoinsReward()
    {
        float baseReward = Get(Type.Base).CoinsReward;
        coinsReward = _rewardMultiplier * baseReward;
    }
}
