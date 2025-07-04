public class CoinsReward : CardReward
{
    private readonly CoinsItems _coinsItemsSO;

    public CoinsReward()
    {
        _coinsItemsSO = GameConstants.Instance.CoinsItems;
    }

    protected override void ConfigureHandlers()
    {
        RegisterHandlers(Card.Type.Base, GetBaseRewardData(), HandleBaseItemSelected);
        RegisterHandlers(Card.Type.Cursed, GetCursedRewardData(), HandleCursedItemSelected);
    }

    private Item GetBaseRewardData()
    {
        return Item.GetRandomItems(_coinsItemsSO.Base.GetAllItems());
    }
        
    private Item GetCursedRewardData()
    {
        return Item.GetRandomItems(_coinsItemsSO.Cursed.GetAllItems());
    }

    private void HandleBaseItemSelected(Item item)
    {
        if (item is CoinsItem coinsItem)
        {
            PlayerCoinsData.AddCoins(coinsItem.Coins);
        }
    }

    private void HandleCursedItemSelected(Item item)
    {
        if (item is CoinsItem coinsItem)
        {
            PlayerCoinsData.RemoveCoins(coinsItem.Coins);
        }
    }
}
