public class GoldenTouchBlessing : Blessing
{
    public override void Add()
    {
        IncreasePinsReward();
    }

    public override void Upgrade()
    {
        IncreasePinsReward();
        base.Upgrade();
    }

    private void IncreasePinsReward(float value = 1)
    {
        Pin.Get(Pin.Type.Base)?.IncreaseReward(value);
    }
}
