using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowItems", menuName = "Game/ThrowItems", order = 1)]
public class ThrowItemsScriptableObject : ScriptableObject
{
    [SerializeField] private List<ThrowItem> _throwItems;

    public List<ThrowItem> GetAllItems()
    {
        return _throwItems;
    }
}
