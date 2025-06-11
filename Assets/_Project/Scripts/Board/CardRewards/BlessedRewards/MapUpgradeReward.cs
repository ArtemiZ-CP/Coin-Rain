using System.Collections.Generic;
using System.Linq;

public class MapUpgradeReward : BlessedCardReward
{
    private readonly List<MapItem> _pinItems;

    public MapUpgradeReward()
    {
        _pinItems = GameConstants.Instance.MapItemsSO.MapItems.ToList();
    }

    public override void GetRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        base.GetRewardData(out items, out stageName, out maxSelectCount, out haveToBuy, out showCloseButton);

        items = Item.GetRandomItems(_pinItems, 1);
    }

    public override void HandleItemSelected(Item item)
    {
        if (item is MapItem mapItem)
        {
            if (mapItem.ItemType == MapItem.Type.IncreaseMapHeight)
            {
                PlayerMapData.IncreaseHeight();
            }
            else if (mapItem.ItemType == MapItem.Type.IncreaseMapWidth)
            {
                PlayerMapData.IncreaseWidth();
            }
            else if (mapItem.ItemType == MapItem.Type.IncreasePinsDurability)
            {
                PlayerMapData.IncreaseDurability();
            }
            else if (mapItem.ItemType == MapItem.Type.IncreaseBounce)
            {
                PlayerMapData.IncreaseBounce();
            }
            else if (mapItem.ItemType == MapItem.Type.ReduceGravity)
            {
                PlayerMapData.ReduceGravity();
            }
        }
    }
}
