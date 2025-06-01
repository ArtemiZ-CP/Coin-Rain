using System;
using UnityEngine;

[Serializable]
public class BlessingItem : Item
{
    [SerializeField] private Blessing.Type _blessingType;

    public Blessing.Type Type => _blessingType;

    public BlessingItem(Blessing.Type blessingType, float price, Sprite itemSprite, Sprite sideSprite, Rare rare)
        : base(price, itemSprite, sideSprite, rare)
    {
        _blessingType = blessingType;
    }
}
