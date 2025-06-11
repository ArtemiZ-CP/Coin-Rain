using UnityEngine;

public class PlayerBallCollider : MonoBehaviour
{
    [SerializeField] private PlayerBall _playerBall;
    [SerializeField] private Collider2D _collider;

    public PlayerBall PlayerBall => _playerBall;

    private void Awake()
    {
        UpdateMaterial();
    }

    private void OnEnable()
    {
        _playerBall.OnSpawn += OnSpawn;
        PlayerMapData.OnMaterialUpdate += UpdateMaterial;
    }

    private void OnDisable()
    {
        _playerBall.OnSpawn -= OnSpawn;
        PlayerMapData.OnMaterialUpdate -= UpdateMaterial;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PinCollider pinCollider))
        {
            _playerBall.OnPinHit(pinCollider.Pin);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out WinArea winArea))
        {
            _playerBall.OnWinAreaHit(winArea);
        }
    }

    private void OnSpawn()
    {
        transform.localPosition = Vector3.zero;
    }

    private void UpdateMaterial()
    {
        _collider.enabled = false;
        _collider.enabled = true;
    }
}
