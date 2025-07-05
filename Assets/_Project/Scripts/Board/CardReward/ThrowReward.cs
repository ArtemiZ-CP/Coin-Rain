public class ThrowReward : CardReward
{
    private ThrowItems _throwItemsSO;
    
    private ThrowItems ThrowItemsSO
    {
        get
        {
            if (_throwItemsSO.Throw == null)
            {
                _throwItemsSO = GameConstants.Instance.ThrowItems;
            }
            return _throwItemsSO;
        }
    }

    protected override void ConfigureHandlers()
    {
        RegisterHandlers(Card.Type.Throw, GetBaseRewardData(), ApplyBaseItemReward);
    }

    private Item GetBaseRewardData()
    {
        return Item.GetRandomItems(ThrowItemsSO.Throw.GetAllItems());
    }

    private void ApplyBaseItemReward(Item item)
    {
        if (item is ThrowItem throwItem)
        {
            PlayerCardsData.AddThrow(throwItem.Count);
        }
    }
}