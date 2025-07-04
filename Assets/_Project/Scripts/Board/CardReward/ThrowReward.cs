public class ThrowReward : CardReward
{
    private readonly ThrowItems _throwItemsSO;

    public ThrowReward()
    {
        _throwItemsSO = GameConstants.Instance.ThrowItems;
    }

    protected override void ConfigureHandlers()
    {
        RegisterHandlers(Card.Type.Throw, GetBaseRewardData(), HandleBaseItemSelected);
    }

    private Item GetBaseRewardData()
    {
        return Item.GetRandomItems(_throwItemsSO.Throw.GetAllItems());
    }

    private void HandleBaseItemSelected(Item item)
    {
        if (item is PinItem pin)
        {
            Pin.Get(pin.Type).Add();
        }
    }
}