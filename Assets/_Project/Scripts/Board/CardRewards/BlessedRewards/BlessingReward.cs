using System.Collections.Generic;
using System.Linq;

public class BlessingReward : BlessedCardReward
{
    private readonly List<BlessingItem> _blessingItems;

    public BlessingReward()
    {
        _blessingItems = GameConstants.Instance.BlessingItemsSO.BlessingItems.ToList();
    }

    public override void GetRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        base.GetRewardData(out items, out stageName, out maxSelectCount, out haveToBuy, out showCloseButton);

        items = Item.GetRandomItems(_blessingItems, 1);
    }

    public override void HandleItemSelected(Item item)
    {
        if (item is BlessingItem blessing)
        {
            Blessing.Get(blessing.Type).Get();
        }
    }
}
