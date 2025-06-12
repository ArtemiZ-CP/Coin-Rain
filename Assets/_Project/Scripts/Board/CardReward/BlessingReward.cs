using System.Collections.Generic;

public class BlessingReward : CardReward
{
    private readonly BlessingItemsScriptableObject _blessingItemsSO;

    public BlessingReward()
    {
        _blessingItemsSO = GameConstants.Instance.BlessingItemsSO;
    }

    public override bool IsRewardAvailable(Card.Type cardType)
    {
        return cardType switch
        {
            Card.Type.Base => true,
            Card.Type.Blessed => true,
            Card.Type.Cursed => true,
            _ => false
        };
    }
    
    protected override void GetBaseRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        items = Item.GetRandomItems(_blessingItemsSO.GetReceivedItems(), 3);
        stageName = "Upgrade Blessing";
        maxSelectCount = 1;
        haveToBuy = true;
        showCloseButton = true;
    }

    protected override void GetBlessedRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        items = Item.GetRandomItems(_blessingItemsSO.GetAllItems(), 1);
        stageName = "Get Blessing";
        maxSelectCount = 1;
        haveToBuy = false;
        showCloseButton = false;
    }

    protected override void GetCursedRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        items = Item.GetRandomItems(_blessingItemsSO.GetReceivedItems(minLevel: 1), 1);
        stageName = "Remove Blessing Upgrade";
        maxSelectCount = 1;
        haveToBuy = false;
        showCloseButton = false;
    }

    protected override void HandleBaseItemSelected(Item item)
    {
        if (item is BlessingItem blessing)
        {
            Blessing.Get(blessing.Type).Upgrade();
        }
    }

    protected override void HandleBlessedItemSelected(Item item)
    {
        if (item is BlessingItem blessing)
        {
            Blessing.Get(blessing.Type).Get();
        }
    }

    protected override void HandleCursedItemSelected(Item item)
    {
        if (item is BlessingItem blessing)
        {
            Blessing.Get(blessing.Type).RemoveUpgrade();
        }
    }
}
