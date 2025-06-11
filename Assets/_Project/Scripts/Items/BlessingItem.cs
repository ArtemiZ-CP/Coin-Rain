using System;
using UnityEngine;

[Serializable]
public class BlessingItem : Item
{
    [SerializeField] private Blessing.Type _blessingType;

    public Blessing.Type Type => _blessingType;
}
