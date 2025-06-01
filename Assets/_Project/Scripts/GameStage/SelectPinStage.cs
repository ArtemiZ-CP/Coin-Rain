using System.Linq;
using UnityEngine;

public class SelectPinStage : GameStage
{
    [SerializeField] private ItemSelectWindow _itemSelectWindow;
    [SerializeField] private PinItemsScriptableObject _pinItemsScriptableObject;

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
        _itemSelectWindow.Initialize(_pinItemsScriptableObject.PinItems.Cast<Item>().ToList(),
            "Select Pin", haveToBuy: false, showCloseButton: false);
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

        if (item is PinItem pinItem)
        {
            Pin.Get(pinItem.Type).IncreaseCount();
        }

        EndStage();
    }
}
