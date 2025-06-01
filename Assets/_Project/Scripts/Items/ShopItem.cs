using System;
using UnityEngine;

[Serializable]
public class ShopItem : Item
{
    [SerializeField] private Type _itemType;

    private readonly Pin.Type _pinType;
    private readonly Blessing.Type _blessingType;

    public Type ItemType => _itemType;
    public Pin.Type PinType => _pinType;
    public Blessing.Type BlessingType => _blessingType;

    public ShopItem(Type itemType, float price, Sprite itemSprite, Sprite sideSprite, Rare rare)
        : base(price, itemSprite, sideSprite, rare)
    {
        _itemType = itemType;
    }

    public ShopItem(Type itemType, Pin.Type pinType, float price, Sprite itemSprite, Sprite sideSprite, Rare rare)
    : base(price, itemSprite, sideSprite, rare)
    {
        _itemType = itemType;
        _pinType = pinType;
    }

    public ShopItem(Type itemType, Blessing.Type blessingType, float price, Sprite itemSprite, Sprite sideSprite, Rare rare)
    : base(price, itemSprite, sideSprite, rare)
    {
        _itemType = itemType;
        _blessingType = blessingType;
    }

    public enum Type
    {
        BuyPin,
        DestroyPin,
        UpgradePin,
        IncreasePinDurability,
        UpgradeBlessing,
        IncreaseMapHeight,
        IncreaseMapWidth,
    }
}
