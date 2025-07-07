using System;
using System.Collections.Generic;

public abstract class Pin
{
    private const int InitialCount = 0;
    private const int InitialDurability = 0;

    private static Dictionary<Type, Pin> _pinInstances;

    protected float coinsReward;
    protected int count;
    protected int durability;
    protected int level = 0;

    public float CoinsReward => coinsReward;
    public int Count => count;
    public int Durability => durability;
    public int Level => level;

    public static event Action OnPinsUpdate;

    public event Action OnPinRewardUpdate;

    public static Type[] GetAllTypes()
    {
        return (Type[])Enum.GetValues(typeof(Type));
    }

    public static void ResetAll()
    {
        foreach (Pin pin in GetPinDictionary().Values)
        {
            pin.Reset();
        }

        OnPinsUpdate?.Invoke();
    }

    public static Pin Get(Type type)
    {
        if (GetPinDictionary().TryGetValue(type, out Pin pin))
        {
            return pin;
        }

        return null;
    }

    public static List<Type> GetRecievedTypes(int minLevel = 0)
    {
        List<Type> receivedPins = new();
        Type[] allTypes = GetAllTypes();

        for (int i = 0; i < allTypes.Length; i++)
        {
            Type type = allTypes[i];
            Pin pin = Get(type);

            if (pin != null && pin.Count > 0 && pin.level >= minLevel)
            {
                receivedPins.Add(type);
            }
        }

        return receivedPins;
    }

    public void IncreaseReward(float value = 1)
    {
        coinsReward += value;
        OnPinRewardUpdate?.Invoke();
    }

    public virtual void Add(int value = 1)
    {
        count += value;
        OnPinsUpdate?.Invoke();
    }

    public virtual void Remove(int value = 1)
    {
        count -= value;

        if (count < 0)
        {
            count = 0;
        }

        OnPinsUpdate?.Invoke();
    }

    public void IncreaseDurability(int value = 1)
    {
        durability += value;
        OnPinsUpdate?.Invoke();
    }

    public float GetPinsCount()
    {
        return count;
    }

    public virtual void Reset()
    {
        count = InitialCount;
        durability = InitialDurability;
    }

    public virtual void Upgrade()
    {
        level++;
    }

    public abstract float Touch(PinObject pin, PlayerBall playerBall);

    private static Dictionary<Type, Pin> GetPinDictionary()
    {
        if (_pinInstances == null)
        {
            BasePin basePin = new();

            _pinInstances = new Dictionary<Type, Pin>
            {
                { Type.Base, basePin },
                { Type.Gold, new GoldPin(basePin) },
                { Type.Multiplying, new MultiPin() },
                { Type.Bomb, new BombPin() }
            };
        }

        return _pinInstances;
    }

    public enum Type
    {
        Base,
        Gold,
        Multiplying,
        Bomb,
    }
}
