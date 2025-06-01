using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItems", menuName = "Game/ShopItems", order = 1)]
public class ShopItemsScriptableObject : ScriptableObject
{
    [SerializeField] private List<ShopItem> _shopItems;

    public IReadOnlyList<ShopItem> ShopItems => _shopItems;
}
