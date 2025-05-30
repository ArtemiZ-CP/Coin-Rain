using System;

public abstract class Pin
{
    public const int DefaultPinsCount = 0;

    protected static float _coinsReward;
    protected static float _count;

    public static event Action OnPinsUpdate;

    public static Type[] GetAllTypes()
    {
        return (Type[])Enum.GetValues(typeof(Type));
    }

    public static void ResetAll()
    {
        foreach (Type pin in GetAllTypes())
        {
            Get(pin).Reset();
            _count = DefaultPinsCount;
        }

        OnPinsUpdate?.Invoke();
    }

    public static Pin Get(Type type)
    {
        return type switch
        {
            Type.Base => new BasePin(),
            Type.Gold => new GoldPin(),
            Type.Multiplying => new MultiPin(),
            Type.Bomb => new BombPin(),
            _ => throw new NotImplementedException(),
        };
    }
    
    public void IncreaseReward(float value)
    {
        _coinsReward += value;
    }

    public float GetPinsCount()
    {
        return _count;
    }

    public abstract void Reset();
    public abstract float Touch(PinObject pin, PlayerBall playerBall);
    public abstract void Upgrade();

    public enum Type
    {
        Base,
        Gold,
        Multiplying,
        Bomb,
    }
}
