using UnityEngine;

public class DropBallStage : GameStage
{
    [SerializeField] private BallsController _ballsController;

    private int _dropCount;
    private int _currentDropCount;

    public void StartStage(int dropCount)
    {
        if (dropCount <= 0)
        {
            EndStage();
            return;
        }

        _dropCount = dropCount;
        StartStage();
    }

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

        base.EndStage();
        BallsController.OnReset -= EndStage;
        _ballsController.SetControllable(false);
    }
}
