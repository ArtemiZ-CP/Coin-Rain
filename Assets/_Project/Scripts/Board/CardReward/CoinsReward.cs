public class CoinsReward : CardReward
{
    private CoinsItems _coinsItemsSO;
    
    private CoinsItems CoinsItemsSO
    {
        get
        {
            if (_coinsItemsSO.Base == null)
            {
                _coinsItemsSO = GameConstants.Instance.CoinsItems;
            }
            return _coinsItemsSO;
        }
    }

    protected override void ConfigureHandlers()
    {
        RegisterHandlers(Card.Type.Base, GetBaseRewardData(), ApplyBaseItemReward);
        RegisterHandlers(Card.Type.Cursed, GetCursedRewardData(), ApplyCursedItemReward);
    }

    private Item GetBaseRewardData()
    {
        return Item.GetRandomItems(CoinsItemsSO.Base.GetAllItems());
    }
        
    private Item GetCursedRewardData()
    {
        return Item.GetRandomItems(CoinsItemsSO.Cursed.GetAllItems());
    }

    private void ApplyBaseItemReward(Item item)
    {
        if (item is CoinsItem coinsItem)
        {
            PlayerCoinsData.AddCoins(coinsItem.Coins);
        }
    }

    private void ApplyCursedItemReward(Item item)
    {
        if (item is CoinsItem coinsItem)
        {
            PlayerCoinsData.RemoveCoins(coinsItem.Coins);
        }
    }
}
