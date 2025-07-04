public class MapUpgradeReward : CardReward
{
    private readonly MapItems _mapItemsSO;

    public MapUpgradeReward()
    {
        _mapItemsSO = GameConstants.Instance.MapItems;
    }

    protected override void ConfigureHandlers()
    {
        RegisterHandlers(Card.Type.Blessed, GenerateBlessedReward(), HandleBlessedItemSelected);
    }

    private void HandleBlessedItemSelected(Item item)
    {
        ApplyMapUpgrade(item);
    }

    private Item GenerateBlessedReward()
    {
        return Item.GetRandomItems(_mapItemsSO.Blessed.GetAllItems());
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
