using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStageController : MonoBehaviour
{
    private readonly List<GameStage> _stagesList = new();

    [SerializeField] private GameStage _dropBallStage;
    [SerializeField] private GameStage _finalStage;
    [SerializeField] private List<StageInfo> _randomStages;

    private int _currentStageIndex = 0;

    public IReadOnlyList<GameStage> StagesList => _stagesList;
    public int CurrentStageIndex => _currentStageIndex;

    public event Action OnStageUpdated;
    public event Action OnStageMapUpdated;

    private void Start()
    {
        foreach (StageInfo stage in _randomStages)
        {
            stage.EndStage();
        }

        SetupStages();
        StartStage();
    }

    public void StartStage()
    {
        GameStage currentStage = _stagesList[_currentStageIndex];
        currentStage.OnStageEnded += EndCurrentStage;
        currentStage.StartStage();
        OnStageUpdated?.Invoke();
    }

    public void EndCurrentStage()
    {
        _stagesList[_currentStageIndex].OnStageEnded -= EndCurrentStage;
        _currentStageIndex++;

        if (_currentStageIndex >= _stagesList.Count)
        {
            SetupStages();
        }

        StartStage();
    }

    private void SetupStages()
    {
        _currentStageIndex = 0;
        _stagesList.Clear();

        for (int i = 0; i < _randomStages.Count; i++)
        {
            for (int j = 0; j < _randomStages[i].Count; j++)
            {
                _stagesList.Add(_randomStages[i].Stage);
            }
        }

        RandomList(_stagesList);
        _stagesList.Add(_finalStage);
        
        for (int i = _stagesList.Count - 1; i >= 0; i--)
        {
            _stagesList.Insert(i, _dropBallStage);
        }

        OnStageMapUpdated?.Invoke();
    }

    private void RandomList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }

    [Serializable]
    public struct StageInfo
    {
        public GameStage Stage;
        public int Count;

        public readonly void EndStage()
        {
            Stage.EndStage();
        }
    }
}
