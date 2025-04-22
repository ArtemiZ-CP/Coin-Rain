using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    private const float MinVelocity = 0.1f;
    private const float MinVelocityTime = 5.0f;

    [SerializeField] private Rigidbody2D _rigidbody;

    private PinConstants _pinConstants;
    private float _lowVelocityTimer = 0f;
    private float _temporaryCoins = 0f;

    public event System.Action<float> OnCoinsChanged;

    private void Awake()
    {
        _pinConstants = GameConstants.Instance.PinConstants;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_rigidbody.velocity.x) < MinVelocity)
        {
            _lowVelocityTimer += Time.fixedDeltaTime;

            if (_lowVelocityTimer >= MinVelocityTime)
            {
                int randomSign = Random.value < 0.5f ? -1 : 1;
                _rigidbody.velocity = new Vector2(randomSign * MinVelocity, _rigidbody.velocity.y);
                _lowVelocityTimer = 0f;
            }
        }
        else
        {
            _lowVelocityTimer = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Pin pin))
        {
            if (pin.Touch(this, out float coins))
            {
                _temporaryCoins += coins;
                OnCoinsChanged?.Invoke(_temporaryCoins);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out WinArea winArea))
        {
            _temporaryCoins *= winArea.Multiplier;
            PlayerData.AddCoins(_temporaryCoins);
            OnCoinsChanged?.Invoke(_temporaryCoins);
        }
    }

    public void AddCoins(float coins)
    {
        _temporaryCoins += coins;
        OnCoinsChanged?.Invoke(_temporaryCoins);
    }

    public void SetRandomImpulse()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        _rigidbody.AddForce(direction * _pinConstants.MultiplyingBallImpulse, ForceMode2D.Impulse);
    }

    public void Stop()
    {
        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = Vector2.zero;
        ResetCoins();
    }

    public void Spawn()
    {
        _rigidbody.gravityScale = 1;
        ResetCoins();
    }

    private void ResetCoins()
    {
        _temporaryCoins = 0;
        OnCoinsChanged?.Invoke(_temporaryCoins);
    }
}
