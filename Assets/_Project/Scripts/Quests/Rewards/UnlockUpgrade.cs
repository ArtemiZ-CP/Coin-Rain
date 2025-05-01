public class UnlockUpgrade : QuestReward
{
    private readonly Pin.Type _pinType;

    public Pin.Type PinType => _pinType;

    public UnlockUpgrade(Pin.Type pinType)
    {
        _pinType = pinType;
    }

    public override void ApplyReward()
    {
        PlayerData.UnlockUpgrade(_pinType);
    }
}
