using UnityEngine;

public class RentPaymentStage : GameStage
{
    [SerializeField] private RentPaymentWindow _rentPaymentWindow;

    public override void StartStage()
    {
        base.StartStage();
        _rentPaymentWindow.Show(PlayerRentData.RentCost);
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
