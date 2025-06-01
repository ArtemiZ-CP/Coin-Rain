using UnityEngine;

public class DropBallStage : GameStage
{
    [SerializeField] private BallsController _ballsController;

    public override void StartStage()
    {
        base.StartStage();
        _ballsController.SetControllable(true);
        BallsController.OnReset += EndStage;
    }

    public override void EndStage()
    {
        base.EndStage();
        BallsController.OnReset -= EndStage;
        _ballsController.SetControllable(false);
    }
}
