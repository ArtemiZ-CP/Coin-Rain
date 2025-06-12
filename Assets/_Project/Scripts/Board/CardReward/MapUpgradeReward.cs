using System.Collections.Generic;

public class MapUpgradeReward : CardReward
{
    private readonly MapItemsScriptableObject _mapItemsSO;

    public MapUpgradeReward()
    {
        _mapItemsSO = GameConstants.Instance.MapItemsSO;
    }

    public override bool IsRewardAvailable(Card.Type cardType)
    {
        return cardType switch
        {
            Card.Type.Base => true,
            Card.Type.Blessed => true,
            Card.Type.Cursed => false,
            _ => false
        };
    }

    protected override void GetBaseRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        items = Item.GetRandomItems(_mapItemsSO.GetAllItems(), 3);
        stageName = "Upgrade Map";
        maxSelectCount = 1;
        haveToBuy = true;
        showCloseButton = true;
    }

    protected override void GetBlessedRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        items = Item.GetRandomItems(_mapItemsSO.GetAllItems(), 1);
        stageName = "Upgrade Map";
        maxSelectCount = 1;
        haveToBuy = false;
        showCloseButton = false;
    }

    protected override void GetCursedRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        throw new System.NotImplementedException();
    }

    protected override void HandleBaseItemSelected(Item item)
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

    protected override void HandleBlessedItemSelected(Item item)
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

    protected override void HandleCursedItemSelected(Item item)
    {
        throw new System.NotImplementedException();
    }
}
