public class BlessingReward : CardReward
{
    private readonly BlessingItems _blessingItemsSO;

    public BlessingReward() : base()
    {
        _blessingItemsSO = GameConstants.Instance.BlessingItems;
    }

    protected override void ConfigureHandlers()
    {
        RegisterHandlers(Card.Type.Blessed, GenerateBlessedReward(), HandleBlessedItemSelected);
    }

    private void HandleBlessedItemSelected(Item item)
    {
        if (item is BlessingItem blessing)
        {
            Blessing.Get(blessing.Type).Get();
        }
    }

    private Item GenerateBlessedReward()
    {
        return Item.GetRandomItems(_blessingItemsSO.Blessed.GetAllItems());
    }
}
