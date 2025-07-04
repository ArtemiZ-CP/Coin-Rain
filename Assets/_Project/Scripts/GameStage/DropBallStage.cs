using UnityEngine;

public class DropBallStage : GameStage
{
    [SerializeField] private BallsController _ballsController;

    private int _dropCount;
    private int _currentDropCount;

    public override void StartStage()
    {
        base.StartStage();
        _currentDropCount = 0;
        _dropCount = PlayerCardsData.TurnsCount;
        PlayerCardsData.ResetTurns();
        _ballsController.SetControllable(true);
        BallsController.OnReset += EndStage;

        if (_dropCount == 0)
        {
            EndStage();
        }
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
