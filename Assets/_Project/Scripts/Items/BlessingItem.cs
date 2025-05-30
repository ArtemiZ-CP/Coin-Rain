using UnityEngine;

public class BlessingItem : Item
{
    [SerializeField] private Blessing.Type _blessingType;

    public Blessing.Type BlessingType => _blessingType;
}
