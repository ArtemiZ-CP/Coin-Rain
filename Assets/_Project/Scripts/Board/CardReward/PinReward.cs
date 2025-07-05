using System.Collections.Generic;

public class PinReward : CardReward
{
    private PinItems _pinItemsSO;
    
    private PinItems PinItemsSO
    {
        get
        {
            if (_pinItemsSO.Base == null)
            {
                _pinItemsSO = GameConstants.Instance.PinItems;
            }
            return _pinItemsSO;
        }
    }

    protected override void ConfigureHandlers()
    {
        RegisterHandlers(Card.Type.Base, GetBaseRewardData(), ApplyBaseItemReward);
        RegisterHandlers(Card.Type.Blessed, GetBlessedRewardData(), ApplyBlessedItemReward);
    }

    public override bool IsRewardAvailable(Card.Type type)
    {
        if (type == Card.Type.Base)
        {
            return GetBaseRewardDataList().Count > 0;
        }
        if (type == Card.Type.Blessed)
        {
            return GetBlessedRewardDataList().Count > 0;
        }

        return base.IsRewardAvailable(type);
    }

    private Item GetBaseRewardData()
    {
        return Item.GetRandomItems(GetBaseRewardDataList());
    }

    private Item GetBlessedRewardData()
    {
        return Item.GetRandomItems(GetBlessedRewardDataList());
    }

    private List<PinItem> GetBaseRewardDataList()
    {
        return PinItemsSO.Base.GetAllItems(includeBasePin: false, Item.Rare.Common, Item.Rare.Rare);
    }

    private List<PinItem> GetBlessedRewardDataList()
    {
        return PinItemsSO.Base.GetAllItems(includeBasePin: false, Item.Rare.Rare, Item.Rare.Legendary);
    }

    private void ApplyBaseItemReward(Item item)
    {
        if (item is PinItem pin)
        {
            Pin.Get(pin.Type).Add();
        }
    }

    private void ApplyBlessedItemReward(Item item)
    {
        if (item is PinItem pin)
        {
            Pin.Get(pin.Type).Upgrade();
        }
    }
}
