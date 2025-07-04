using UnityEngine;

public class SelectItem : GameStage
{
    [SerializeField] private ItemSelectWindow _itemSelectWindow;

    private CardReward _cardReward;
    private int _maxSelectCount;
    private int _selectedCount;

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
        _cardReward.GetRewardData();
        // _itemSelectWindow.Show(items, stageName, haveToBuy, showCloseButton);
        // _selectedCount = 0;
        // _maxSelectCount = Mathf.Min(maxSelectCount, items.Count);
    }

    public override void EndStage()
    {
        base.EndStage();
        _itemSelectWindow.Hide();
    }

    private void HandleItemSelected()
    {
        if (IsActive == false)
        {
            return;
        }

        _cardReward.HandleItemSelected();
        _selectedCount++;

        if (_selectedCount >= _maxSelectCount)
        {
            EndStage();
        }
    }
}
