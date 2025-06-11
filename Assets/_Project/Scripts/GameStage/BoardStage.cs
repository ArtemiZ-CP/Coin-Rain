using UnityEngine;

public class BoardStage : GameStage
{
    [SerializeField] private Board _board;
    [SerializeField] private GameStageController _gameStageController;
    [SerializeField] private int _cardCount = 7;

    private void OnEnable()
    {
        _gameStageController.OnBallStageStarted += HideBoard;
        _board.OnCardClick += HandleCardClick;
    }

    private void OnDisable()
    {
        _gameStageController.OnBallStageStarted -= HideBoard;
        _board.OnCardClick -= HandleCardClick;
    }

    public override void StartStage()
    {
        ShowBoard();

        if (_board.GetActiveCardCount() == 0)
        {
            _gameStageController.EndRound();
        }

        base.StartStage();
    }

    public override void EndStage()
    {
        base.EndStage();
    }

    public void StartNewRound()
    {
        _board.CreateCards(_cardCount);
        StartStage();
    }

    public void EndRound()
    {
        EndStage();
        _board.ClearCards();
    }

    public void ShowBoard()
    {
        _board.Show();
    }

    public void HideBoard()
    {
        _board.Hide();
    }

    private void HandleCardClick(Card card)
    {
        if (IsActive == false)
        {
            return;
        }

        card.Disactive();
        _gameStageController.StartStage(card);
        EndStage();
    }
}
