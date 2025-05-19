using System;

[Serializable]
public struct QuestObjectiveData
{
    public QuestObjectiveType Type;
    public Condition Condition;
    public int[] IntProperties;
    public float[] FloatProperties;
}

public abstract class QuestObjective
{
    private bool _isCompleted = false;

    public bool IsCompleted => _isCompleted;

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

    public string GetDescriptionText()
    {
        if (this is HitFinishObjective hitFinish)
        {
            return $"Попасть шаром во множитель x{hitFinish.FinishMultiplierToHit}";
        }
        else if (this is HitPinsObjective hitPins)
        {
            string pinType = hitPins.PinType switch
            {
                Pin.Type.Base => "обычные штырьки",
                Pin.Type.Gold => "золотые штырьки",
                Pin.Type.Multiplying => "множители",
                Pin.Type.Bomb => "бомбы",
                _ => "неизвестные"
            };

            return $"Задеть {pinType} за один раунд: {hitPins.PinsCount}";
        }
        else if (this is EarnCoinsObjective earnCoins)
        {
            return $"Заработать монет одним шаром: {earnCoins.CoinsCount}";
        }
        else if (this is EarnCoinsByAllBallsObjective earnCoinsByAllBalls)
        {
            return $"Заработать монет за один запуск: {earnCoinsByAllBalls.CoinsCount}";
        }
        else
        {
            return "Неизвестный квест";
        }
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
    HitFinish,
    HitPins,
    EarnCoinsByOneBall,
    EarnCoinsByAllBalls,
    FallTime,
}