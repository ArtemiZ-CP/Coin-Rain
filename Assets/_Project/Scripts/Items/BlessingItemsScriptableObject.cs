using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlessingItems", menuName = "Game/BlessingItems", order = 1)]
public class BlessingItemsScriptableObject : ScriptableObject
{
    [SerializeField] private List<BlessingItem> _blessingItems = default;

    public IReadOnlyList<BlessingItem> BlessingItem => _blessingItems;
}
