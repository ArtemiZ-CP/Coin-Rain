using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PinItems", menuName = "Game/PinItems", order = 1)]
public class PinItemsScriptableObject : ScriptableObject
{
    [SerializeField] private PinItem _basePinItem = default;
    [SerializeField] private List<PinItem> _pinItems = default;

    public PinItem BasePinItem => _basePinItem;
    public IReadOnlyList<PinItem> PinItems => _pinItems;
}
