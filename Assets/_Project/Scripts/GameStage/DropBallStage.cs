using UnityEngine;

public class DropBallStage : GameStage
{
    [SerializeField] private BallsController _ballsController;

    private int _dropCount = 1;
    private int _currentDropCount;

    public override void StartStage()
    {
        base.StartStage();
        _ballsController.SetControllable(true);
        BallsController.OnReset += EndStage;
        _currentDropCount = 0;
    }

    public override void EndStage()
    {
        _currentDropCount++;
        
        if (_currentDropCount < _dropCount && IsActive)
        {
            _ballsController.SetControllable(true);
            return;
        }

        BallsController.OnReset -= EndStage;
        _ballsController.SetControllable(false);
        base.EndStage();
    }
}
