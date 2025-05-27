public static class PlayerPinsData
{
    public const int DefaultRewardFromPin = 1;
    public const int DefaultPinsCount = 0;
    public const int DefaultGoldPinRewardMultiplier = 2;
    public const int DefaultMultiPinsValue = 1;
    public const int DefaultBombPinValue = 1;

    private static float _rewardFromPin;
    private static int _goldPinsCount;
    private static int _goldPinsValue;
    private static int _multiPinsCount;
    private static int _multiPinsValue;
    private static int _bombPinsCount;
    private static int _bombPinsValue;

    public static float RewardFromPin => _rewardFromPin;
    public static int GoldPinsCount => _goldPinsCount;
    public static int GoldPinsValue => _goldPinsValue;
    public static int MultiPinsCount => _multiPinsCount;
    public static int MultiPinsValue => _multiPinsValue;
    public static int BombPinsCount => _bombPinsCount;
    public static int BombPinsValue => _bombPinsValue;

    public static event System.Action OnPinsUpdate;

    public static void Reset()
    {
        _rewardFromPin = DefaultRewardFromPin;
        _goldPinsCount = DefaultPinsCount;
        _goldPinsValue = DefaultGoldPinRewardMultiplier;
        _multiPinsCount = DefaultPinsCount;
        _multiPinsValue = DefaultMultiPinsValue;
        _bombPinsCount = DefaultPinsCount;
        _bombPinsValue = DefaultBombPinValue;

        OnPinsUpdate?.Invoke();
    }

    public static void SetRewardFromPin(float value)
    {
        if (value < 0)
        {
            return;
        }

        _rewardFromPin = value;
    }

    public static void AddGoldPin()
    {
        _goldPinsCount++;
        OnPinsUpdate?.Invoke();
    }

    public static void SetGoldPinsValue(int value)
    {
        _goldPinsValue = value;
    }

    public static void AddMultiplyingPin()
    {
        _multiPinsCount++;
        OnPinsUpdate?.Invoke();
    }

    public static void SetMultiPinsValue(int value)
    {
        _multiPinsValue = value;
    }

    public static void AddBombPin()
    {
        _bombPinsCount++;
        OnPinsUpdate?.Invoke();
    }

    public static void SetBombPinsValue(int value)
    {
        _bombPinsValue = value;
    }
}
