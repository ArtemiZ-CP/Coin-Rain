using UnityEngine;

public class PinCollider : MonoBehaviour
{
    [SerializeField] private PinObject _pin;

    public PinObject Pin => _pin;
}
