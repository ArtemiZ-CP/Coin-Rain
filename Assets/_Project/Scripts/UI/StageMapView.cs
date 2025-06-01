using System.Collections.Generic;
using UnityEngine;

public class StageMapView : MonoBehaviour
{
    private readonly List<GameObject> _stagesList = new();

    [SerializeField] private GameStageController _gameStageController;
    [SerializeField] private Transform _stagesParent;
    [SerializeField] private GameObject _dropBallStageViewPrefab;
    [SerializeField] private GameObject _selectPinStageViewPrefab;
    [SerializeField] private GameObject _selectBlessingStageViewPrefab;
    [SerializeField] private GameObject _shopStageViewPrefab;
    [SerializeField] private GameObject _finalStageViewPrefab;
    [SerializeField] private GameObject _currentStagePosition;

    private void OnEnable()
    {
        _gameStageController.OnStageMapUpdated += UpdateStageMap;
        _gameStageController.OnStageUpdated += UpdateCurrentStagePosition;
    }

    private void OnDisable()
    {
        _gameStageController.OnStageMapUpdated -= UpdateStageMap;
        _gameStageController.OnStageUpdated -= UpdateCurrentStagePosition;
    }

    private void UpdateStageMap()
    {
        ClearStageMap();

        foreach (GameStage stage in _gameStageController.StagesList)
        {
            GameObject stageViewPrefab = GetStageViewPrefab(stage);

            if (stageViewPrefab != null)
            {
                _stagesList.Add(Instantiate(stageViewPrefab, _stagesParent));
            }
        }
    }

    private void ClearStageMap()
    {
        _stagesList.Clear();

        foreach (Transform child in _stagesParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateCurrentStagePosition()
    {
        GameObject stageView = _stagesList[_gameStageController.CurrentStageIndex];
        _currentStagePosition.transform.SetParent(stageView.transform);
        _currentStagePosition.transform.localPosition = Vector3.zero;
    }

    private GameObject GetStageViewPrefab(GameStage stage)
    {
        if (stage is DropBallStage)
        {
            return _dropBallStageViewPrefab;
        }
        else if (stage is SelectPinStage)
        {
            return _selectPinStageViewPrefab;
        }
        else if (stage is SelectBlessingStage)
        {
            return _selectBlessingStageViewPrefab;
        }
        else if (stage is ShopStage)
        {
            return _shopStageViewPrefab;
        }
        else if (stage is RentPaymentStage)
        {
            return _finalStageViewPrefab;
        }

        return null;
    }
}
