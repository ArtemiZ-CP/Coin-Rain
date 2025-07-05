using System.Collections.Generic;

public abstract class CardReward
{
    protected delegate void ApplyRewardDelegate(Item item);

    private readonly Dictionary<Card.Type, RewardData> _rewards = new();

    private Card.Type _cardType;

    public Card.Type CardType => _cardType;

    public CardReward()
    {
        ConfigureHandlers();
    }

    public void SetCardType(Card.Type type)
    {
        _cardType = type;
    }

    public Item GetRewardData()
    {
        if (_rewards.TryGetValue(_cardType, out var reward))
        {
            return reward.Item;
        }

        throw new System.NotImplementedException($"Reward data for card type {_cardType} is not implemented.");
    }

    public void ApplyReward()
    {
        if (_rewards.TryGetValue(_cardType, out var reward))
        {
            reward.HandleSelect.Invoke(reward.Item);
            return;
        }

        throw new System.NotImplementedException($"Item selection for card type {_cardType} is not implemented.");
    }

    public virtual bool IsRewardAvailable(Card.Type type)
    {
        return _rewards.ContainsKey(type);
    }

    protected void RegisterHandlers(Card.Type type, Item item, ApplyRewardDelegate handler)
    {
        _rewards.Add(type, new RewardData { Item = item, HandleSelect = handler });
    }

    protected abstract void ConfigureHandlers();

    private struct RewardData
    {
        public Item Item;
        public ApplyRewardDelegate HandleSelect;
    }
}
