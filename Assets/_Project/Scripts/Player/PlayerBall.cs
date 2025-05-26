using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    private const float MinVelocity = 0.1f;
    private const float MinVelocityTime = 2.0f;

    [SerializeField] private Rigidbody2D _rigidbody;

    private PinConstants _pinConstants;
    private float _lowVelocityTimer = 0f;
    private float _temporaryCoins = 0f;
    private bool _isMoving = false;

    public float TemporaryCoins => _temporaryCoins;
    public Vector3 Position => _rigidbody.transform.localPosition;

    public static event System.Action<PlayerBall, float> OnTemporaryCoinsChanged;
    public static event System.Action<PlayerBall, Pin.Type> OnBallHitPin;
    public event System.Action<float> OnCoinsChanged;
    public event System.Action<PlayerBall, int, float> OnBallFinished;
    public event System.Action OnSpawn;

    private void Awake()
    {
        _pinConstants = GameConstants.Instance.PinConstants;
    }

    private void FixedUpdate()
    {
        if (_isMoving && Mathf.Abs(_rigidbody.velocity.x) < MinVelocity)
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

    public void SetPosition(Vector3 position)
    {
        _rigidbody.transform.localPosition = position;
    }

    public void OnPinHit(Pin pin)
    {
        if (pin.Touch(this, out float coins))
        {
            _temporaryCoins += coins;
            OnCoinsChanged?.Invoke(_temporaryCoins);
            OnTemporaryCoinsChanged?.Invoke(this, _temporaryCoins);
            OnBallHitPin?.Invoke(this, pin.PinType);
        }
    }

    public void OnWinAreaHit(WinArea winArea)
    {
        _temporaryCoins *= winArea.Multiplier;
        PlayerCurrencyData.AddCoins(_temporaryCoins);
        OnCoinsChanged?.Invoke(_temporaryCoins);
        OnTemporaryCoinsChanged?.Invoke(this, _temporaryCoins);
        OnBallFinished?.Invoke(this, winArea.Multiplier, _temporaryCoins);
    }

    public void SetScale(float scale)
    {
        _rigidbody.transform.localScale = new Vector3(scale, scale, 1);
    }

    public void AddCoins(float coins)
    {
        _temporaryCoins += coins;
        OnCoinsChanged?.Invoke(_temporaryCoins);
        OnTemporaryCoinsChanged?.Invoke(this, _temporaryCoins);
    }

    public void SetRandomImpulse()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        _rigidbody.AddForce(direction * _pinConstants.MultiplyingBallImpulse, ForceMode2D.Impulse);
    }

    public void Spawn()
    {
        _rigidbody.transform.localPosition = Vector3.zero;
        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = Vector2.zero;
        _isMoving = false;
        ResetCoins();
        OnSpawn?.Invoke();
    }

    public void Drop()
    {
        _rigidbody.gravityScale = 1;
        _isMoving = true;
        ResetCoins();
    }

    private void ResetCoins()
    {
        _temporaryCoins = 0;
        OnCoinsChanged?.Invoke(_temporaryCoins);
    }
}
