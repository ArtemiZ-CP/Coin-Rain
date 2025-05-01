using System;

[Serializable]
public struct QuestObjectiveData
{
    public QuestObjectiveType Type;
    public int FinishMultiplierToHit;
    public int PinsCount;
    public float CoinsCount;
    public Pin.Type PinType;
}

public abstract class QuestObjective
{
    private bool _isCompleted = false;

    public event Action OnObjectiveCompleted;

    protected QuestObjective()
    {
        BallsController.Instance.OnReset += OnReset;
    }

    ~QuestObjective()
    {
        BallsController.Instance.OnReset -= OnReset;
    }

    public virtual void OnReset()
    {
        if (_isCompleted == false)
        {
            return;
        }

        CompleteObjective();
    }

    public virtual void CompleteObjective()
    {
        OnObjectiveCompleted?.Invoke();
    }

    protected void SetCompleted()
    {
        _isCompleted = true;
    }
}

public enum QuestObjectiveType
{
    DropBall,
    HitFinish,
    HitPins,
    EarnCoins,
}