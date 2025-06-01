using UnityEngine;

[System.Serializable]
public abstract class GameStage : MonoBehaviour
{
    private bool _isActive;

    public bool IsActive => _isActive;

    public event System.Action OnStageEnded;

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