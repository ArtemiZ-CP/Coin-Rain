using UnityEngine;

public abstract class Item
{
    [SerializeField] private float _price;
    [SerializeField] private Rare _rare;

    public float Price => _price;
    public Rare ItemRare => _rare;

    public enum Rare
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}
