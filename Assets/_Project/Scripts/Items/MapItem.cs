using System;
using UnityEngine;

[Serializable]
public class MapItem : Item
{
    [SerializeField] private Type _itemType;

    public Type ItemType => _itemType;

    public enum Type
    {
        IncreaseMapHeight,
        IncreaseMapWidth,
        IncreasePinsDurability,
        IncreaseBounce,
        ReduceGravity,
    }
}
