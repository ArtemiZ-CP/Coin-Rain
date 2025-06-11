using System.Collections.Generic;

public class PinUpgradeReward : BlessedCardReward
{
    private readonly List<PinItem> _pinItems;

    public PinUpgradeReward()
    {
        _pinItems = GameConstants.Instance.UpgradePinItemsSO.GetReceivedItems();
    }

    public override void GetRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        base.GetRewardData(out items, out stageName, out maxSelectCount, out haveToBuy, out showCloseButton);

        items = Item.GetRandomItems(_pinItems, 1);
    }

    public override void HandleItemSelected(Item item)
    {
        if (item is PinItem pinItem)
        {
            Pin.Get(pinItem.Type).Upgrade();
        }
    }
}
