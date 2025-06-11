using System.Collections.Generic;
using UnityEngine;

public class SelectItem : GameStage
{
    [SerializeField] private ItemSelectWindow _itemSelectWindow;

    private CardReward _cardReward;

    private void OnEnable()
    {
        _itemSelectWindow.OnItemSelected += HandleItemSelected;
        _itemSelectWindow.OnWindowClosed += EndStage;
    }

    private void OnDisable()
    {
        _itemSelectWindow.OnItemSelected -= HandleItemSelected;
        _itemSelectWindow.OnWindowClosed -= EndStage;
    }

    public void SetReward(CardReward cardReward)
    {
        _cardReward = cardReward;
    }

    public override void StartStage()
    {
        base.StartStage();
        _cardReward.GetRewardData(out List<Item> items, out string stageName, out int maxSelectCount, out bool haveToBuy, out bool showCloseButton);
        _itemSelectWindow.Show(items, stageName, haveToBuy, showCloseButton);
    }

    public override void EndStage()
    {
        base.EndStage();
        _itemSelectWindow.Hide();
    }

    private void HandleItemSelected(Item item)
    {
        if (IsActive == false)
        {
            return;
        }

        if (item is BlessingItem blessing)
        {
            Blessing.Get(blessing.Type).Get();
        }
        else if (item is CoinsItem coinsItem)
        {
            PlayerCoinsData.AddCoins(coinsItem.Coins);
        }

        EndStage();
    }
}
