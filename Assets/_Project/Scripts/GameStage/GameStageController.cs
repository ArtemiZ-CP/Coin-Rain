using System;
using UnityEngine;

public class GameStageController : MonoBehaviour
{
    [SerializeField] private DropBallStage _dropBallStage;
    [SerializeField] private BoardStage _boardStage;
    [SerializeField] private GameStage _startStage;
    [SerializeField] private GameStage _finalStage;
    [SerializeField] private SelectItem _selectItemStage;

    private GameStage _currentStage;
    private int _dropCount;

    public event Action OnBallStageStarted;

    private void Start()
    {
        StartNewRound();
    }

    public void StartNewRound()
    {
        _currentStage = _startStage;
        _currentStage.StartStage();
        _currentStage.OnStageEnded += HandleStartRound;
    }

    public void EndRound()
    {
        _currentStage = _finalStage;
        _currentStage.StartStage();
        _currentStage.OnStageEnded += HandleEndRound;
    }

    public void StartStage(Card card)
    {
        if (card == null)
        {
            throw new ArgumentNullException(nameof(card), "Card cannot be null");
        }

        _dropCount = card.TurnsCount;
        StartStage(card.CardReward);
    }

    private void StartStage(CardReward cardReward)
    {
        _selectItemStage.SetReward(cardReward);
        _currentStage = _selectItemStage;
        _currentStage.StartStage();
        _currentStage.OnStageEnded += StartBallStage;
    }

    private void StartBallStage()
    {
        OnBallStageStarted?.Invoke();
        _currentStage.OnStageEnded -= StartBallStage;
        _currentStage = _dropBallStage;
        _currentStage.OnStageEnded += EndStage;
        _dropBallStage.StartStage(_dropCount);
    }

    private void EndStage()
    {
        if (_currentStage == null)
        {
            throw new InvalidOperationException("Current stage is not set.");
        }

        _currentStage.OnStageEnded -= EndStage;
        _boardStage.StartStage();
    }

    private void HandleStartRound()
    {
        _currentStage.OnStageEnded -= HandleStartRound;
        _boardStage.StartNewRound();
    }

    private void HandleEndRound()
    {
        _currentStage.OnStageEnded -= HandleEndRound;
        _boardStage.EndRound();
        StartNewRound();
    }
}
