public class DropBallObjective : QuestObjective
{
    public DropBallObjective()
    {
        BallsController.OnBallDropped += OnBallDropped;
    }

    ~DropBallObjective()
    {
        BallsController.OnBallDropped -= OnBallDropped;
    }

    private void OnBallDropped(PlayerBall playerBall)
    {
        SetCompleted();
    }
}
