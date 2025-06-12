using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinsItem", menuName = "Game/CoinsItem", order = 1)]
public class CoinsItemScriptableObject : ScriptableObject
{
    [SerializeField] private List<CoinsItem> _coinsItems = default;

    public List<Item> GetAllItems()
    {
        return new(_coinsItems);
    }
}
