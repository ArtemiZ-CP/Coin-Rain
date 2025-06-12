using System.Collections.Generic;

public class CoinsReward : CardReward
{
    private readonly CoinsItemScriptableObject _coinsItemsSO;

    public CoinsReward()
    {
        _coinsItemsSO = GameConstants.Instance.CoinsItemsSO;
    }

    public override bool IsRewardAvailable(Card.Type cardType)
    {
        return cardType switch
        {
            Card.Type.Base => true,
            Card.Type.Blessed => false,
            Card.Type.Cursed => true,
            _ => false
        };
    }

    protected override void GetBaseRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        items = Item.GetRandomItems(_coinsItemsSO.GetAllItems(), 1);
        stageName = "Gold Reward";
        maxSelectCount = 1;
        haveToBuy = false;
        showCloseButton = false;
    }

    protected override void GetBlessedRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        throw new System.NotImplementedException();
    }

    protected override void GetCursedRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        items = Item.GetRandomItems(_coinsItemsSO.GetAllItems(), 1);
        stageName = "Remove Gold";
        maxSelectCount = 1;
        haveToBuy = false;
        showCloseButton = false;
    }

    protected override void HandleBaseItemSelected(Item item)
    {
        if (item is CoinsItem coinsItem)
        {
            PlayerCoinsData.AddCoins(coinsItem.Coins);
        }
    }

    protected override void HandleBlessedItemSelected(Item item)
    {
        throw new System.NotImplementedException();
    }

    protected override void HandleCursedItemSelected(Item item)
    {
        if (item is CoinsItem coinsItem)
        {
            PlayerCoinsData.RemoveCoins(coinsItem.Coins);
        }
    }
}
