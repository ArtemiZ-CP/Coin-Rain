using System;
using UnityEngine;

[Serializable]
public class CoinsItem : Item
{
    [SerializeField] private float _coins;

    public float Coins => _coins;
}
