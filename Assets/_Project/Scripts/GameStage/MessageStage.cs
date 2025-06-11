using UnityEngine;

public class MessageStage : GameStage
{
    [SerializeField] private MessageWindow _messageWindow;

    public override void StartStage()
    {
        base.StartStage();
        _messageWindow.Show(PlayerRentData.RentCost);
        _messageWindow.OnClosed += EndStage;
    }

    public override void EndStage()
    {
        _messageWindow.OnClosed -= EndStage;
        _messageWindow.Hide();
        base.EndStage();
    }
}
