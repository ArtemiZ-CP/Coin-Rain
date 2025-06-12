using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BlessingItems", menuName = "Game/BlessingItems", order = 1)]
public class BlessingItemsScriptableObject : ScriptableObject
{
    [SerializeField] private List<BlessingItem> _blessingItems = default;

    public List<BlessingItem> GetAllItems()
    {
        return new(_blessingItems);
    }

    public List<BlessingItem> GetReceivedItems(int minLevel = 0)
    {
        List<BlessingItem> blessingItems = new();

        foreach (Blessing.Type type in Blessing.GetRecievedTypes(minLevel))
        {
            BlessingItem item = _blessingItems.FirstOrDefault(blessingItem => blessingItem.Type == type);

            if (item != null)
            {
                blessingItems.Add(item);
            }
        }

        return blessingItems;
    }
}
