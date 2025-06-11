using System.Collections.Generic;

public abstract class CardReward
{
    public abstract CardReward GenerateReward();
    public virtual void GetRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton)
    {
        items = new List<Item>();
        stageName = "Card Reward Stage";
        maxSelectCount = 1;
        haveToBuy = false;
        showCloseButton = true;
    }
}
