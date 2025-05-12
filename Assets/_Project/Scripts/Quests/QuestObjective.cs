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

    public event Action<float> OnObjectiveProgressChanged;
    public event Action OnObjectiveCompleted;

    protected QuestObjective()
    {
        BallsController.OnReset += OnReset;
    }

    ~QuestObjective()
    {
        BallsController.OnReset -= OnReset;
    }

    public virtual void OnReset()
    {
        if (_isCompleted == false)
        {
            ChangeObjectiveProgress(0);
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
        ChangeObjectiveProgress(1);
    }

    protected void ChangeObjectiveProgress(float progress)
    {
        OnObjectiveProgressChanged?.Invoke(progress);
    }
}

public enum QuestObjectiveType
{
    DropBall,
    HitFinish,
    HitPins,
    EarnCoinsByOneBall,
    EarnCoinsByAllBalls,
}