using UnityEngine;

public class ThrowItem : Item
{
    [SerializeField] private int _count;

    public int Count => _count;
}
