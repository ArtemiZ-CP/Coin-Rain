using System.Collections.Generic;
using UnityEngine;

public class BlessingReward : CardReward
{
    private BlessingItems _blessingItemsSO;
    
    private BlessingItems BlessingItemsSO
    {
        get
        {
            if (_blessingItemsSO.Blessed == null)
            {
                _blessingItemsSO = GameConstants.Instance.BlessingItems;
            }
            return _blessingItemsSO;
        }
    }

    protected override void ConfigureHandlers()
    {
        RegisterHandlers(Card.Type.Blessed, GenerateBlessedReward(), ApplyBlessedItemReward);
    }

    private void ApplyBlessedItemReward(Item item)
    {
        if (item is BlessingItem blessing)
        {
            Blessing.Get(blessing.Type).Get();
        }
    }

    private Item GenerateBlessedReward()
    {
        List<BlessingItem> items = BlessingItemsSO.Blessed.GetAllItems();
        return Item.GetRandomItems(items);
    }
}
