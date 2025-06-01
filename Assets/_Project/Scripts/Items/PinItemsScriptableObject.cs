using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PinItems", menuName = "Game/PinItems", order = 1)]
public class PinItemsScriptableObject : ScriptableObject
{
    [SerializeField] private PinItem _basePinItem = default;
    [SerializeField] private List<PinItem> _pinItems = default;

    public PinItem BasePinItem => _basePinItem;
    public IReadOnlyList<PinItem> PinItems => _pinItems;

    public List<PinItem> GetReceivedItems(bool includeBasePin = false)
    {
        List<PinItem> pinItems = new();

        if (includeBasePin)
        {
            pinItems.Add(_basePinItem);
        }

        foreach (Pin.Type type in Pin.GetRecievedTypes())
        {
            PinItem item = _pinItems.FirstOrDefault(pinItem => pinItem.Type == type);

            if (item != null)
            {
                pinItems.Add(item);
            }
        }

        return pinItems;
    }
}
