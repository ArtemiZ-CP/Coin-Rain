using System.Collections.Generic;

public static class PlayerFinishData
{
    public const int DefaultBaseFinishCount = 8;
    public const int DefaultBaseFinishMultiplier = 1;
    public const float DefaultBaseFinishWidth = 1;

    public const int DefaultJackpotFinishCount = 1;
    public const int DefaultJackpotFinishMultiplier = 2;
    public const float DefaultJackpotFinishWidth = 1;

    public const int DefaultEmptyFinishCount = 1;
    public const int DefaultEmptyFinishMultiplier = 0;
    public const float DefaultEmptyFinishWidth = 1;

    private static readonly List<FinishData> _finishData = new();

    public static event System.Action OnFinishUpdate;

    public static void Reset()
    {
        _finishData.Clear();

        _finishData.Add(new FinishData
        {
            Type = FinishType.Base,
            Count = DefaultBaseFinishCount,
            Multiplier = DefaultBaseFinishMultiplier,
            Width = DefaultBaseFinishWidth
        });
        _finishData.Add(new FinishData
        {
            Type = FinishType.Jackpot,
            Count = DefaultJackpotFinishCount,
            Multiplier = DefaultJackpotFinishMultiplier,
            Width = DefaultJackpotFinishWidth
        });
        _finishData.Add(new FinishData
        {
            Type = FinishType.Empty,
            Count = DefaultEmptyFinishCount,
            Multiplier = DefaultEmptyFinishMultiplier,
            Width = DefaultEmptyFinishWidth
        });

        OnFinishUpdate?.Invoke();
    }
    
    public static FinishData GetFinishData(FinishType finishType)
    {
        foreach (FinishData finish in _finishData)
        {
            if (finish.Type == finishType)
            {
                return finish;
            }
        }

        return null;
    }

    public static void IncreaseFinishCount(FinishType finishType, int value = 1)
    {
        if (value < 1)
        {
            return;
        }

        foreach (FinishData finish in _finishData)
        {
            if (finish.Type == finishType)
            {
                finish.Count += value;
                OnFinishUpdate?.Invoke();
                return;
            }
        }
    }

    public enum FinishType
    {
        Empty,
        Base,
        Jackpot
    }

    public class FinishData
    {
        public FinishType Type;
        public int Count;
        public int Multiplier;
        public float Width;
    }
}
