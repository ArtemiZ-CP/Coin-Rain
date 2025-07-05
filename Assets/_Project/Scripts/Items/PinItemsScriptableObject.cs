using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PinItems", menuName = "Game/PinItems", order = 1)]
public class PinItemsScriptableObject : ScriptableObject
{
    [SerializeField] private PinItem _basePinItem = default;
    [SerializeField] private List<PinItem> _pinItems = default;

    public List<PinItem> GetAllItems(bool includeBasePin, Item.Rare minRare = Item.Rare.Common, Item.Rare maxRare = Item.Rare.Legendary)
    {
        List<PinItem> pinItems = _pinItems.Where(pinItem => pinItem.ItemRare >= minRare && pinItem.ItemRare <= maxRare).ToList();

        if (includeBasePin)
        {
            pinItems.Add(_basePinItem);
        }

        return pinItems;
    }

    public List<PinItem> GetReceivedItems(bool includeBasePin, int minLevel = 0)
    {
        List<PinItem> pinItems = new();

        if (includeBasePin)
        {
            pinItems.Add(_basePinItem);
        }

        foreach (Pin.Type type in Pin.GetRecievedTypes(minLevel))
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
