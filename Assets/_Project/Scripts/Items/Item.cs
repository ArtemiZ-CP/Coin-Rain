using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    [SerializeField] private float _price;
    [SerializeField] private Sprite _itemSprite;
    [SerializeField] private Sprite _sideSprite;
    [SerializeField] private Rare _rare;

    public float Price => _price;
    public Rare ItemRare => _rare;
    public Sprite ItemSprite => _itemSprite;
    public Sprite SideSprite => _sideSprite;

    public Item(float price, Sprite itemSprite, Sprite sideSprite, Rare rare)
    {
        _price = price;
        _itemSprite = itemSprite;
        _sideSprite = sideSprite;
        _rare = rare;
    }

    public static T GetRandomItem<T>(List<T> items) where T : Item
    {
        if (items == null || items.Count == 0)
        {
            return null;
        }

        float totalWeight = 0f;

        foreach (var item in items)
        {
            totalWeight += GetItemChance(item);
        }

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var item in items)
        {
            currentWeight += GetItemChance(item);

            if (randomValue <= currentWeight)
            {
                return item;
            }
        }

        return items[^1];
    }

    private static float GetItemChance(Item item)
    {
        return item.ItemRare switch
        {
            Rare.Common => 0.5f,
            Rare.Uncommon => 0.3f,
            Rare.Rare => 0.15f,
            Rare.Epic => 0.04f,
            Rare.Legendary => 0.01f,
            _ => 0f,
        };
    }

    public enum Rare
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}
