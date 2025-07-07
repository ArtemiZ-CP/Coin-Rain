using System;
using System.Collections.Generic;

public abstract class Blessing
{
    private static readonly Dictionary<Type, Blessing> _blessingInstances = new()
    {
        { Type.GoldenTouch, new GoldenTouchBlessing() },
    };

    protected int level = 0;

    public int Level => level;

    public static Type[] GetAllTypes()
    {
        return (Type[])Enum.GetValues(typeof(Type));
    }

    public static Blessing Get(Type type)
    {
        if (_blessingInstances.TryGetValue(type, out Blessing blessing))
        {
            return blessing;
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
            Blessing blessing = Get(type);

            if (blessing != null && blessing.Level >= minLevel)
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

    public virtual void RemoveUpgrade()
    {
        if (level > 0)
        {
            level--;
        }
    }

    public abstract void Add();

    public enum Type
    {
        GoldenTouch,
    }
}