using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BallsController : MonoBehaviour
{
    public static BallsController Instance { get; private set; }

    private readonly List<PlayerBall> _activeBalls = new();

    [SerializeField] private PlayerBall _ballPrefab;
    [SerializeField] private PinsMap _pinsMap;
    [SerializeField] private GameZone _gameZone;
    [SerializeField] private float _height = 1.0f;

    private PinConstants _pinConstants;
    private PlayerBall _targetBall;
    private ObjectPool<PlayerBall> _pool;
    private bool _controllable = false;
    private bool _isStarting = false;
    private bool _isMooving = true;
    private float _maxX;
    private float _currentCoins;

    public static event System.Action<PlayerBall> OnBallDropped;
    public static event System.Action<PlayerBall, int, float> OnBallFinished;
    public static event System.Action<float> OnAllBallsFinished;
    public static event System.Action OnFixedUpdate;
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

        _pinConstants = GameConstants.Instance.PinConstants;
    }

    private void Update()
    {
        if (_isStarting || _controllable == false)
        {
            return;
        }

        if (_isMooving)
        {
            Move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void FixedUpdate()
    {
        if (_isStarting == false)
        {
            return;
        }

        OnFixedUpdate?.Invoke();
    }

    public void PointerDown()
    {
        if (_isStarting || _controllable == false)
        {
            return;
        }

        _isMooving = false;
        _isStarting = true;
        _targetBall.Drop();
        _targetBall.SetImpulse(Vector2.down, _pinConstants.StartBallImpulse);
        OnBallDropped?.Invoke(_targetBall);
    }

    public void SetControllable(bool value)
    {
        _controllable = value;
    }

    public void Reset()
    {
        _currentCoins = 0;
        _isMooving = true;
        _isStarting = false;
        _targetBall = _pool.Get();
        _targetBall.Spawn();
        _pinsMap.Reset();
        _maxX = (PlayerMapData.GetMapWidth() - _targetBall.Scale.x) / 2;
        Move(Vector2.zero);
        OnReset?.Invoke();
    }

    public PlayerBall SpawnBall(Vector3 position)
    {
        PlayerBall newPlayerBall = _pool.Get();
        newPlayerBall.SetPosition(position);
        newPlayerBall.Drop();
        return newPlayerBall;
    }

    public float Blast(PinObject pin, PlayerBall playerBall, int range, float blastImpulse, float coins)
    {
        coins = _pinsMap.Blast(pin, playerBall, range, coins);

        float blastRange = _pinConstants.OffsetBetweenPinsInLine * range;
        Vector3 blastCenter = pin.transform.position;

        foreach (PlayerBall ball in _activeBalls)
        {
            Vector3 blastDirection = (ball.Position - blastCenter).normalized;
            float distance = Vector3.Distance(blastCenter, ball.Position);
            float forceMultiplier = Mathf.Clamp01(1f - (distance / blastRange));

            ball.SetImpulse(blastDirection, blastImpulse * forceMultiplier);
        }

        return coins;
    }

    private void ActionOnGetBall(PlayerBall ball)
    {
        ball.gameObject.SetActive(true);
        ball.OnBallFinished += BallFinished;
        _activeBalls.Add(ball);
    }

    private void ActionOnRelease(PlayerBall ball)
    {
        ball.gameObject.SetActive(false);
        ball.OnBallFinished -= BallFinished;
        _activeBalls.Remove(ball);
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
            OnAllBallsFinished?.Invoke(_currentCoins);
            Reset();
        }
    }

    private void Move(Vector2 position)
    {
        float x = Mathf.Clamp(position.x, -_maxX, _maxX);
        _targetBall.SetPosition(new Vector3(x, _height, _targetBall.transform.position.z));
    }
}
