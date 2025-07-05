public class MapUpgradeReward : CardReward
{
    private MapItems _mapItemsSO;

    public MapItems MapItemsSO
    {
        get
        {
            if (_mapItemsSO.Blessed == null)
            {
                _mapItemsSO = GameConstants.Instance.MapItems;
            }
            return _mapItemsSO;
        }
    }

    protected override void ConfigureHandlers()
    {
        RegisterHandlers(Card.Type.Blessed, GenerateBlessedReward(), ApplyBlessedItemReward);
    }

    private void ApplyBlessedItemReward(Item item)
    {
        ApplyMapUpgrade(item);
    }

    private Item GenerateBlessedReward()
    {
        return Item.GetRandomItems(MapItemsSO.Blessed.GetAllItems());
    }
    
    private void ApplyMapUpgrade(Item item)
    {
        if (item is MapItem mapItem)
        {
            switch (mapItem.ItemType)
            {
                case MapItem.Type.IncreaseMapHeight:
                    PlayerMapData.IncreaseHeight();
                    break;
                case MapItem.Type.IncreaseMapWidth:
                    PlayerMapData.IncreaseWidth();
                    break;
                case MapItem.Type.IncreasePinsDurability:
                    PlayerMapData.IncreaseDurability();
                    break;
                case MapItem.Type.IncreaseBounce:
                    PlayerMapData.IncreaseBounce();
                    break;
                case MapItem.Type.ReduceGravity:
                    PlayerMapData.ReduceGravity();
                    break;
            }
        }
    }
}
