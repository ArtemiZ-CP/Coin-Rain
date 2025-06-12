using System.Collections.Generic;

public abstract class CardReward
{
    private Card.Type _cardType;

    public Card.Type CardType => _cardType;

    public void SetCardType(Card.Type type)
    {
        _cardType = type;
    }

    public void GetRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        switch (_cardType)
        {
            case Card.Type.Base:
                GetBaseRewardData(out items, out stageName, out maxSelectCount, out haveToBuy, out showCloseButton);
                break;
            case Card.Type.Blessed:
                GetBlessedRewardData(out items, out stageName, out maxSelectCount, out haveToBuy, out showCloseButton);
                break;
            case Card.Type.Cursed:
                GetCursedRewardData(out items, out stageName, out maxSelectCount, out haveToBuy, out showCloseButton);
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(_cardType), _cardType, null);
        }
    }

    public void HandleItemSelected(Item item)
    {
        switch (_cardType)
        {
            case Card.Type.Base:
                HandleBaseItemSelected(item);
                break;
            case Card.Type.Blessed:
                HandleBlessedItemSelected(item);
                break;
            case Card.Type.Cursed:
                HandleCursedItemSelected(item);
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(_cardType), _cardType, null);
        }
    }

    public abstract bool IsRewardAvailable(Card.Type type);

    protected abstract void GetBaseRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton);
    protected abstract void GetBlessedRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton);
    protected abstract void GetCursedRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton);
    protected abstract void HandleBaseItemSelected(Item item);
    protected abstract void HandleBlessedItemSelected(Item item);
    protected abstract void HandleCursedItemSelected(Item item);
}
