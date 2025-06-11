using UnityEngine;

public class PinCollider : MonoBehaviour
{
    [SerializeField] private PinObject _pin;
    [SerializeField] private Collider2D _collider;

    public PinObject Pin => _pin;

    private void Awake()
    {
        UpdateMaterial();
    }

    private void OnEnable()
    {
        PlayerMapData.OnMaterialUpdate += UpdateMaterial;
    }

    private void OnDisable()
    {
        PlayerMapData.OnMaterialUpdate -= UpdateMaterial;
    }

    private void UpdateMaterial()
    {
        _collider.enabled = false;
        _collider.enabled = true;
    }
}
