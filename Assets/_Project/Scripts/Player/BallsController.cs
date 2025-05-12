using UnityEngine;
using UnityEngine.Pool;

public class BallsController : MonoBehaviour
{
    public static BallsController Instance { get; private set; }

    [SerializeField] private PlayerBall _ballPrefab;
    [SerializeField] private PinsMap _pinsMap;
    [SerializeField] private GameZone _gameZone;
    [SerializeField] private float _height = 1.0f;

    private PlayerBall _targetBall;
    private ObjectPool<PlayerBall> _pool;
    private bool _isStarting = false;
    private bool _isMooving = false;
    private float _maxX;
    private float _currentCoins;

    public static event System.Action<PlayerBall> OnBallDropped;
    public static event System.Action<PlayerBall, int, float> OnBallFinished;
    public static event System.Action<PlayerBall, Pin.Type> OnBallHitPin;
    public static event System.Action<PlayerBall, float> OnAllBallsFinished;
    public static event System.Action OnReset;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _pool = new ObjectPool<PlayerBall>(
            createFunc: () => Instantiate(_ballPrefab, Vector3.zero, Quaternion.identity, transform),
            actionOnGet: ball => ActionOnGetBall(ball),
            actionOnRelease: ball => ActionOnRelease(ball),
            actionOnDestroy: ball => Destroy(ball.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );
    }

    private void Start()
    {
        Reset();
    }

    private void OnEnable()
    {
        PlayerData.OnPlayerBallSizeUpdate += UpdateBallSize;
    }

    private void OnDisable()
    {
        PlayerData.OnPlayerBallSizeUpdate -= UpdateBallSize;
    }

    private void Update()
    {
        if (_isStarting)
        {
            return;
        }

        if (_isMooving)
        {
            Move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void PointerDown()
    {
        if (_isStarting)
        {
            return;
        }

        _isMooving = true;
        _maxX = (PlayerData.WidthUpgrade + 0.5f) * GameConstants.Instance.PinConstants.OffsetBetweenPinsInLine;
    }

    public void PointerUp()
    {
        if (_isStarting)
        {
            return;
        }

        _isMooving = false;
        _isStarting = true;
        _targetBall.Drop();
        OnBallDropped?.Invoke(_targetBall);
    }

    public PlayerBall SpawnBall(Vector3 position)
    {
        PlayerBall newPlayerBall = _pool.Get();
        newPlayerBall.SetPosition(position);
        newPlayerBall.Drop();
        return newPlayerBall;
    }

    public float Blast(Pin pin, PlayerBall playerBall) => _pinsMap.Blast(pin, playerBall);

    private void UpdateBallSize()
    {
        if (_targetBall == null)
        {
            return;
        }

        _targetBall.SetScale(PlayerData.PlayerBallSize);
    }

    private void ActionOnGetBall(PlayerBall ball)
    {
        ball.SetScale(PlayerData.PlayerBallSize);
        ball.gameObject.SetActive(true);
        ball.OnBallFinished += BallFinished;
        ball.OnBallHitPin += OnBallHitPin;
    }

    private void ActionOnRelease(PlayerBall ball)
    {
        ball.gameObject.SetActive(false);
        ball.OnBallFinished -= BallFinished;
        ball.OnBallHitPin -= OnBallHitPin;
    }

    private void BallFinished(PlayerBall playerBall, int multiplier, float coins)
    {
        if (playerBall == null || playerBall.gameObject == null)
        {
            return;
        }

        OnBallFinished?.Invoke(playerBall, multiplier, coins);
        _currentCoins += coins;

        _pool.Release(playerBall);

        if (_pool.CountActive == 0)
        {
            OnAllBallsFinished?.Invoke(playerBall, _currentCoins);
            Reset();
        }
    }

    private void Reset()
    {
        _currentCoins = 0;
        _isStarting = false;
        _targetBall = _pool.Get();
        _targetBall.Spawn();
        _pinsMap.Reset();
        Move(Vector2.zero);
        OnReset?.Invoke();
    }

    private void Move(Vector2 position)
    {
        float x = Mathf.Clamp(position.x, -_maxX, _maxX);
        _targetBall.SetPosition(new Vector3(x, _height, _targetBall.transform.position.z));
    }
}
