using System.Collections.Generic;

public class PinReward : CardReward
{
    private readonly PinItemsScriptableObject _pinItemsSO;

    public PinReward()
    {
        _pinItemsSO = GameConstants.Instance.PinItemsSO;
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
        items = Item.GetRandomItems(_pinItemsSO.GetAllItems(includeBasePin: false), 3);
        stageName = "Add Pin";
        maxSelectCount = 1;
        haveToBuy = true;
        showCloseButton = true;
    }

    protected override void GetBlessedRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        items = Item.GetRandomItems(_pinItemsSO.GetReceivedItems(includeBasePin: true), 3);
        stageName = "Upgrade Pin Type";
        maxSelectCount = 1;
        haveToBuy = false;
        showCloseButton = false;
    }

    protected override void GetCursedRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        items = Item.GetRandomItems(_pinItemsSO.GetReceivedItems(includeBasePin: false), 1);
        stageName = "Remove Pin";
        maxSelectCount = 1;
        haveToBuy = false;
        showCloseButton = false;
    }

    protected override void HandleBaseItemSelected(Item item)
    {
        if (item is PinItem pin)
        {
            Pin.Get(pin.Type).Add();
        }
    }

    protected override void HandleBlessedItemSelected(Item item)
    {
        if (item is PinItem pin)
        {
            Pin.Get(pin.Type).Upgrade();
        }
    }

    protected override void HandleCursedItemSelected(Item item)
    {
        if (item is PinItem pin)
        {
            Pin.Get(pin.Type).Remove();
        }
    }
}
