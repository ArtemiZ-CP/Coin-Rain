using System;
using UnityEngine;

[Serializable]
public class PinItem : Item
{
    [SerializeField] private Pin.Type _type;
    [SerializeField] private Color _color;

    public Pin.Type Type => _type;
    public Color Color => _color;

    public PinItem(Pin.Type type, Color color, float price, Sprite itemSprite, Sprite sideSprite, Rare rare)
                : base(price, itemSprite, sideSprite, rare)
    {
        _type = type;
        _color = color;
    }
}
