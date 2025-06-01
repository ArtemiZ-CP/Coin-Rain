using System;
using System.Collections.Generic;

public abstract class Blessing
{
    private static Dictionary<Type, Blessing> _blessingInstances = null;

    protected int level = 0;

    public int Level => level;

    public static Type[] GetAllTypes()
    {
        return (Type[])Enum.GetValues(typeof(Type));
    }

    public static Blessing Get(Type type)
    {
        _blessingInstances ??= new Dictionary<Type, Blessing>()
        {
            { Type.GoldenTouch, new GoldenTouchBlessing() },
        };

        if (_blessingInstances.TryGetValue(type, out Blessing blessing))
        {
            return blessing;
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
            Blessing blessing = Get(type);

            if (blessing != null && blessing.Level > 0)
            {
                receivedPins.Add(type);
            }
        }

        return receivedPins;
    }

    public virtual void Upgrade()
    {
        level++;
    }

    public abstract void Get();

    public enum Type
    {
        GoldenTouch,
    }
}