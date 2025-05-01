using System;

[Serializable]
public struct QuestObjectiveData
{
    public QuestObjectiveType Type;
    public int FinishMultiplierToHit;
    public int PinsCount;
    public float CoinsCount;
}

public abstract class QuestObjective
{
    public event Action OnObjectiveCompleted;

    public virtual void CompleteObjective()
    {
        OnObjectiveCompleted?.Invoke();
    }
}

public enum QuestObjectiveType
{
    DropBall,
    HitFinish,
    HitPins,
    EarnCoins,
}