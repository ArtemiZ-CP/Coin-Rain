public class PinReward : CardReward
{
    private readonly PinItems _pinItemsSO;

    public PinReward()
    {
        _pinItemsSO = GameConstants.Instance.PinItems;
    }

    protected override void ConfigureHandlers()
    {
        RegisterHandlers(Card.Type.Base, GetBaseRewardData(), HandleBaseItemSelected);
        RegisterHandlers(Card.Type.Blessed, GetBlessedRewardData(), HandleBlessedItemSelected);
    }

    private Item GetBaseRewardData()
    {
        return Item.GetRandomItems(_pinItemsSO.Base.GetAllItems(includeBasePin: false));
    }

    private Item GetBlessedRewardData()
    {
        return Item.GetRandomItems(_pinItemsSO.Blessed.GetReceivedItems(includeBasePin: true));
    }

    private void HandleBaseItemSelected(Item item)
    {
        if (item is PinItem pin)
        {
            Pin.Get(pin.Type).Add();
        }
    }

    private void HandleBlessedItemSelected(Item item)
    {
        if (item is PinItem pin)
        {
            Pin.Get(pin.Type).Upgrade();
        }
    }
}
