using System;
using UnityEngine;

[Serializable]
public class CoinsItem : Item
{
    [SerializeField, Min(0)] private float _coins;

    public float Coins => _coins;
}
