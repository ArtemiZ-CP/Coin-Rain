using UnityEngine;

public class PinCollider : MonoBehaviour
{
    [SerializeField] private Pin _pin;

    public Pin Pin => _pin;
}
