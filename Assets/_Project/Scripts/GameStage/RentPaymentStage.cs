using UnityEngine;

public class RentPaymentStage : GameStage
{
    [SerializeField] private RentPaymentWindow _rentPaymentWindow;

    public override void StartStage()
    {
        base.StartStage();
        _rentPaymentWindow.Initialize(PlayerRentData.RentCost);
        _rentPaymentWindow.OnPaid += EndStage;
    }

    public override void EndStage()
    {
        _rentPaymentWindow.OnPaid -= EndStage;
        PlayerRentData.IncreaseRentCost();
        _rentPaymentWindow.Hide();
        base.EndStage();
    }
}
