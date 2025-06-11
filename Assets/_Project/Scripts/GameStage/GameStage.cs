using System;
using UnityEngine;

[Serializable]
public abstract class GameStage : MonoBehaviour
{
    private bool _isActive;

    public bool IsActive => _isActive;

    public event Action OnStageEnded;

    protected virtual void Awake()
    {
        EndStage();
    }

    public virtual void StartStage()
    {
        _isActive = true;
    }

    public virtual void EndStage()
    {
        _isActive = false;
        OnStageEnded?.Invoke();
    }
}