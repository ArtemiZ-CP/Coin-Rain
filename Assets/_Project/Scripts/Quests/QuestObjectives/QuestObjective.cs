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
    private readonly Condition _condition;
    private bool _isCompleted = false;

    public Condition Condition => _condition;
    public bool IsCompleted => _isCompleted;

    public event Action<float, bool> OnObjectiveProgressChanged;
    public event Action OnObjectiveCompleted;

    protected QuestObjective(Condition condition)
    {
        _condition = condition;
        BallsController.OnBallDropped += OnBallDropped;
        BallsController.OnReset += OnReset;
    }

    ~QuestObjective()
    {
        BallsController.OnBallDropped -= OnBallDropped;
        BallsController.OnReset -= OnReset;
    }

    public virtual void OnBallDropped(PlayerBall playerBall)
    {
        _isCompleted = false;
        ChangeObjectiveProgress(0);
    }

    public virtual void OnReset()
    {
        if (_isCompleted == false)
        {
            ChangeObjectiveProgress(1, negativeCondition: true);
            return;
        }

        ChangeObjectiveProgress(1);
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
            if (hitFinish.Condition == Condition.Equal)
            {
                return $"Попасть шаром во множитель x{hitFinish.FinishMultiplierToHit}";
            }
            else if (hitFinish.Condition == Condition.More)
            {
                return $"Попасть шаром вo множитель не меньше x{hitFinish.FinishMultiplierToHit}";
            }
            else if (hitFinish.Condition == Condition.Less)
            {
                return $"Попасть шаром вo множитель не больше x{hitFinish.FinishMultiplierToHit}";
            }
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

            if (hitPins.Condition == Condition.Equal)
            {
                return $"Задеть {pinType} за один раунд: {hitPins.PinsCount}";
            }
            else if (hitPins.Condition == Condition.More)
            {
                return $"Задеть {pinType} за один раунд: не меньше {hitPins.PinsCount}";
            }
            else if (hitPins.Condition == Condition.Less)
            {
                return $"Задеть {pinType} за один раунд: не больше {hitPins.PinsCount}";
            }
        }
        else if (this is EarnCoinsObjective earnCoins)
        {
            if (earnCoins.Condition == Condition.Equal)
            {
                return $"Заработать монет одним шаром: {earnCoins.CoinsCount}";
            }
            else if (earnCoins.Condition == Condition.More)
            {
                return $"Заработать монет одним шаром: не меньше {earnCoins.CoinsCount}";
            }
            else if (earnCoins.Condition == Condition.Less)
            {
                return $"Заработать монет одним шаром: не больше {earnCoins.CoinsCount}";
            }
        }
        else if (this is EarnCoinsByAllBallsObjective earnCoinsByAllBalls)
        {
            if (earnCoinsByAllBalls.Condition == Condition.Equal)
            {
                return $"Заработать монет за один запуск: {earnCoinsByAllBalls.CoinsCount}";
            }
            else if (earnCoinsByAllBalls.Condition == Condition.More)
            {
                return $"Заработать монет за один запуск: не меньше {earnCoinsByAllBalls.CoinsCount}";
            }
            else if (earnCoinsByAllBalls.Condition == Condition.Less)
            {
                return $"Заработать монет за один запуск: не больше {earnCoinsByAllBalls.CoinsCount}";
            }
        }
        else if (this is FallTimeObjective fallTime)
        {
            if (fallTime.Condition == Condition.Equal)
            {
                return $"Провести время в падении: {fallTime.Timer} сек";
            }
            else if (fallTime.Condition == Condition.More)
            {
                return $"Провести время в падении: не меньше {fallTime.Timer} сек";
            }
            else if (fallTime.Condition == Condition.Less)
            {
                return $"Провести время в падении: не больше {fallTime.Timer} сек";
            }
        }

        return "Неизвестный квест";
    }

    protected bool TryComplete(float currentValue, float targetValue)
    {
        if (_condition == Condition.Equal)
        {
            if (Math.Abs(currentValue - targetValue) < 0.01f)
            {
                _isCompleted = true;
                ChangeObjectiveProgress(1);
                return true;
            }

            ChangeObjectiveProgress(0);
        }
        else if (_condition == Condition.More)
        {
            if (currentValue >= targetValue)
            {
                _isCompleted = true;
                ChangeObjectiveProgress(1);
                return true;
            }

            ChangeObjectiveProgress(currentValue / targetValue);
        }
        else if (_condition == Condition.Less)
        {
            if (currentValue <= targetValue)
            {
                _isCompleted = true;
                ChangeObjectiveProgress(currentValue / targetValue, negativeCondition: true);
                return true;
            }

            ChangeObjectiveProgress(1, negativeCondition: true);
        }

        _isCompleted = false;
        return false;
    }

    protected void ChangeObjectiveProgress(float progress, bool negativeCondition = false)
    {
        OnObjectiveProgressChanged?.Invoke(progress, negativeCondition);
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