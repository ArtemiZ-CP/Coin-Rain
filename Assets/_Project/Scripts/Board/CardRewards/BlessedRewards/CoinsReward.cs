using System.Collections.Generic;
using System.Linq;

public class CoinsReward : BlessedCardReward
{
    private readonly List<CoinsItem> _coinsItems;

    public CoinsReward()
    {
        _coinsItems = GameConstants.Instance.CoinsItemsSO.CoinsItems.ToList();
    }

    public override void GetRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        base.GetRewardData(out items, out stageName, out maxSelectCount, out haveToBuy, out showCloseButton);

        items = Item.GetRandomItems(_coinsItems, 1);
    }
}
