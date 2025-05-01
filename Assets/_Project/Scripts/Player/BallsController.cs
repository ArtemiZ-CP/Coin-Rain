using UnityEngine;
using UnityEngine.Pool;

public class BallsController : MonoBehaviour
{
    public static BallsController Instance { get; private set; }

    [SerializeField] private PlayerBall _ballPrefab;
    [SerializeField] private PinsMap _pinsMap;
    [SerializeField] private GameZone _gameZone;
    [SerializeField] private float _height = 1.0f;

    private PlayerBall _target;
    private ObjectPool<PlayerBall> _pool;
    private bool _isStarting = false;
    private bool _isMooving = false;
    private float _maxX;

    public event System.Action<PlayerBall> OnBallDropped;
    public event System.Action<PlayerBall, int, float> OnBallFinished;
    public event System.Action<PlayerBall> OnBallHitPin;
    public event System.Action OnReset;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _pool = new ObjectPool<PlayerBall>(
            createFunc: () => Instantiate(_ballPrefab, transform),
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
        _gameZone.OnBallFinished += BallFinished;
    }

    private void OnDisable()
    {
        _gameZone.OnBallFinished -= BallFinished;
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
        _maxX = PlayerData.WidthUpgrade * (GameConstants.Instance.PinConstants.OffsetBetweenPinsInLine + 1);
    }

    public void PointerUp()
    {
        if (_isStarting)
        {
            return;
        }

        _isMooving = false;
        _isStarting = true;
        _target.Spawn();
        OnBallDropped?.Invoke(_target);
    }

    public PlayerBall SpawnBall(Vector3 position)
    {
        PlayerBall newPlayerBall = _pool.Get();
        newPlayerBall.transform.position = position;
        newPlayerBall.Spawn();
        return newPlayerBall;
    }

    public float Blast(Pin pin, PlayerBall playerBall) => _pinsMap.Blast(pin, playerBall);

    private void ActionOnGetBall(PlayerBall ball)
    {
        ball.gameObject.SetActive(true);
        ball.OnBallFinished += OnBallFinished;
        ball.OnBallHitPin += OnBallHitPin;
    }

    private void ActionOnRelease(PlayerBall ball)
    {
        ball.gameObject.SetActive(false);
        ball.OnBallFinished -= OnBallFinished;
        ball.OnBallHitPin -= OnBallHitPin;
    }

    private void BallFinished(PlayerBall playerBall)
    {
        if (playerBall == null || playerBall.gameObject == null)
        {
            return;
        }

        _pool.Release(playerBall);

        if (_pool.CountActive == 0)
        {
            Reset();
        }
    }

    private void Reset()
    {
        _isStarting = false;
        _target = _pool.Get();
        _target.Stop();
        _pinsMap.Reset();
        Move(Vector2.zero);
        OnReset?.Invoke();
    }

    private void Move(Vector2 position)
    {
        float x = Mathf.Clamp(position.x, -_maxX, _maxX);
        _target.transform.position = new Vector3(x, _height, _target.transform.position.z);
    }
}
