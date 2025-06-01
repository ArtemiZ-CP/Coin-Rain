using System;
using System.Collections.Generic;

public abstract class Pin
{
    private static Dictionary<Type, Pin> _pinInstances = null;

    protected float coinsReward;
    protected int count = 0;
    protected int durability = 1;

    public float CoinsReward => coinsReward;
    public int Count => count;
    public int Durability => durability;

    public static event Action OnPinsUpdate;

    public static Type[] GetAllTypes()
    {
        return (Type[])Enum.GetValues(typeof(Type));
    }

    public static void ResetAll()
    {
        foreach (Pin pin in _pinInstances.Values)
        {
            pin.Reset();
        }

        OnPinsUpdate?.Invoke();
    }

    public static Pin Get(Type type)
    {
        _pinInstances ??= new Dictionary<Type, Pin>()
        {
            { Type.Base, new BasePin() },
            { Type.Gold, new GoldPin() },
            { Type.Multiplying, new MultiPin() },
            { Type.Bomb, new BombPin() }
        };

        if (_pinInstances.TryGetValue(type, out Pin pin))
        {
            return pin;
        }

        return null;
    }

    public static List<Type> GetRecievedTypes()
    {
        List<Type> receivedPins = new();
        Type[] allTypes = GetAllTypes();
        
        for (int i = 0; i < allTypes.Length; i++)
        {
            Type type = allTypes[i];
            Pin pin = Get(type);

            if (pin != null && pin.Count > 0)
            {
                receivedPins.Add(type);
            }
        }

        return receivedPins;
    }

    public void IncreaseReward(float value = 1)
    {
        coinsReward += value;
    }

    public void IncreaseCount(int value = 1)
    {
        count += value;
        OnPinsUpdate?.Invoke();
    }

    public void DecreaseCount(int value = 1)
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
