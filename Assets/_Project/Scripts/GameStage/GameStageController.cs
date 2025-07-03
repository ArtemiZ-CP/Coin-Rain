using UnityEngine;

public class GameStageController : MonoBehaviour
{
    [SerializeField] private GameStage[] _gameStages;

    private int _currentStageIndex = 0;

    private void Start()
    {
        StartNewRound();
    }

    public void StartNewRound()
    {
        _currentStageIndex = 0;
        NestStage();
    }

    public void NestStage()
    {
        if (_currentStageIndex >= _gameStages.Length)
        {
            StartNewRound();
            return;
        }

        _gameStages[_currentStageIndex].OnStageEnded += HandleStageEnded;
        _gameStages[_currentStageIndex].StartStage();
    }

    private void HandleStageEnded()
    {
        _gameStages[_currentStageIndex].OnStageEnded -= HandleStageEnded;
        _currentStageIndex++;
        NestStage();
    }
}
