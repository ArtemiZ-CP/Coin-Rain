using System.Linq;
using UnityEngine;

public class SelectBlessingStage : GameStage
{
    [SerializeField] private ItemSelectWindow _itemSelectWindow;
    [SerializeField] private BlessingItemsScriptableObject _blessingItemsScriptableObject;

    private void OnEnable()
    {
        _itemSelectWindow.OnItemSelected += HandleItemSelected;
    }

    private void OnDisable()
    {
        _itemSelectWindow.OnItemSelected -= HandleItemSelected;
    }

    public override void StartStage()
    {
        base.StartStage();
        _itemSelectWindow.Initialize(_blessingItemsScriptableObject.BlessingItems.Cast<Item>().ToList(),
            "Select Blessing", haveToBuy: false, showCloseButton: false);
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

        if (item is BlessingItem pinItem)
        {
            Blessing.Get(pinItem.Type).Get();
        }

        EndStage();
    }
}
