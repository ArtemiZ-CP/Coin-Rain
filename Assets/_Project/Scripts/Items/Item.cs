using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    [SerializeField] private Rare _rare;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _itemSprite;

    public string Name => _name;
    public Rare ItemRare => _rare;
    public Sprite ItemSprite => _itemSprite;

    public static List<Item> GetRandomItems<T>(List<T> items, int count) where T : Item
    {
        if (items == null || items.Count == 0 || count <= 0)
        {
            return new List<Item>();
        }
        
        List<Item> selectedItems = new();
        
        for (int i = 0; i < count; i++)
        {
            if (items.Count == 0)
            {
                break;
            }

            T itemToRemove = GetRandomItems(items);
            items.Remove(itemToRemove);
            selectedItems.Add(itemToRemove);
        }

        return selectedItems;
    }

    public static T GetRandomItems<T>(List<T> items) where T : Item
    {
        if (items == null || items.Count == 0)
        {
            return null;
        }

        float totalWeight = 0f;

        foreach (T item in items)
        {
            totalWeight += GetItemChance(item);
        }

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (T item in items)
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
