using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItems", menuName = "Game/ShopItems", order = 1)]
public class ConstructionItemsScriptableObject : ScriptableObject
{
    [SerializeField] private List<ConstructionItem> _constructionItems;

    public IReadOnlyList<ConstructionItem> ConstructionItems => _constructionItems;
}