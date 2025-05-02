using UnityEngine;

public class PlayerBallCollider : MonoBehaviour
{
    [SerializeField] private PlayerBall _playerBall;

    public PlayerBall PlayerBall => _playerBall;

    private void OnEnable()
    {
        _playerBall.OnSpawn += OnSpawn;
    }

    private void OnDisable()
    {
        _playerBall.OnSpawn -= OnSpawn;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Pin pin))
        {
            _playerBall.OnPinHit(pin);
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
}
