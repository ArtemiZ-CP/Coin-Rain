public static class PlayerPinsData
{
    public const int DefaultRewardFromPin = 1;
    public const int DefaultPinsCount = 0;
    public const int DefaultGoldPinRewardMultiplier = 2;
    public const int DefaultMultiPinsValue = 1;
    public const int DefaultBombPinValue = 1;

    private static float _rewardFromPin;
    private static int _goldPinsCountUpgrade;
    private static int _goldPinsValueUpgrade;
    private static int _multiPinsCountUpgrade;
    private static int _multiPinsValueUpgrade;
    private static int _bombPinsCountUpgrade;
    private static int _bombPinsValueUpgrade;

    public static float RewardFromPin => _rewardFromPin;
    public static int GoldPinsCountUpgrade => _goldPinsCountUpgrade;
    public static int GoldPinsValueUpgrade => _goldPinsValueUpgrade;
    public static int MultiPinsCountUpgrade => _multiPinsCountUpgrade;
    public static int MultiPinsValueUpgrade => _multiPinsValueUpgrade;
    public static int BombPinsCountUpgrade => _bombPinsCountUpgrade;
    public static int BombPinsValueUpgrade => _bombPinsValueUpgrade;

    public static event System.Action OnPinsUpdate;

    public static void Reset()
    {
        _rewardFromPin = DefaultRewardFromPin;
        _goldPinsCountUpgrade = DefaultPinsCount;
        _goldPinsValueUpgrade = DefaultGoldPinRewardMultiplier;
        _multiPinsCountUpgrade = DefaultPinsCount;
        _multiPinsValueUpgrade = DefaultMultiPinsValue;
        _bombPinsCountUpgrade = DefaultPinsCount;
        _bombPinsValueUpgrade = DefaultBombPinValue;

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
        _goldPinsCountUpgrade++;
        OnPinsUpdate?.Invoke();
    }

    public static void SetGoldPinsValue(int value)
    {
        _goldPinsValueUpgrade = value;
    }

    public static void AddMultiplyingPin()
    {
        _multiPinsCountUpgrade++;
        OnPinsUpdate?.Invoke();
    }

    public static void SetMultiPinsValue(int value)
    {
        _multiPinsValueUpgrade = value;
    }

    public static void AddBombPin()
    {
        _bombPinsCountUpgrade++;
        OnPinsUpdate?.Invoke();
    }

    public static void SetBombPinsValue(int value)
    {
        _bombPinsValueUpgrade = value;
    }
}
