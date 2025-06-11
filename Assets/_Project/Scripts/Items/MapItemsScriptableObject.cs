using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItems", menuName = "Game/ShopItems", order = 1)]
public class MapItemsScriptableObject : ScriptableObject
{
    [SerializeField] private List<MapItem> _mapItems;

    public IReadOnlyList<MapItem> MapItems => _mapItems;
}