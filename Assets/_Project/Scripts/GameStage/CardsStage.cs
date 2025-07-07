using UnityEngine;

public class CardsStage : GameStage
{
    [SerializeField] private Board _board;
    [SerializeField] private SelectItem _selectItemStage;

    private void OnEnable()
    {
        _board.OnCardClick += HandleCardClick;
        _board.OnEndStageButtonClick += EndStage;
    }

    private void OnDisable()
    {
        _board.OnCardClick -= HandleCardClick;
        _board.OnEndStageButtonClick -= EndStage;
    }

    public override void StartStage()
    {
        base.StartStage();
        _board.CreateCards();
    }

    public override void EndStage()
    {
        _board.Hide();
        base.EndStage();
    }

    private void HandleCardClick(Card card)
    {
        if (IsActive == false)
        {
            return;
        }

        _selectItemStage.SetReward(card.CardReward);
        _selectItemStage.StartStage();
    }
}
