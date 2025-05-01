public class DropBallObjective : QuestObjective
{
    public DropBallObjective()
    {
        BallsController.Instance.OnBallDropped += OnBallDropped;
    }

    ~DropBallObjective()
    {
        BallsController.Instance.OnBallDropped -= OnBallDropped;
    }

    private void OnBallDropped(PlayerBall playerBall)
    {
        CompleteObjective();
    }
}
