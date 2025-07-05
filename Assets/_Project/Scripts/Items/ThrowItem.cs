using System;
using UnityEngine;

[Serializable]
public class ThrowItem : Item
{
    [SerializeField] private int _count;

    public int Count => _count;
}
