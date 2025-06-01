public class GoldenTouchBlessing : Blessing
{
    public override void Get()
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
        foreach (Pin.Type type in Pin.GetAllTypes())
        {
            Pin.Get(type)?.IncreaseReward(value);
        }
    }
}
