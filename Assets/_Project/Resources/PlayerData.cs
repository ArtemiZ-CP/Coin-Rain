using UnityEngine;

public static class PlayerData
{
    public const int DefaultLevel = -1;
    public const float DefaultCoins = 10000;
    public const int DefaultPinsCount = 0;

    #region Private fields
    private static float _coins;
    private static UpgradeData<float> _baseUpgrade;
    private static UpgradeData<float> _gameSpeedUpgrade;
    private static UpgradeData<int> _widthUpgrade;
    private static UpgradeData<int> _heightUpgrade;
    private static UpgradeData<int> _winAreasUpgrade;
    private static UpgradeData<int>  _goldPinsCountUpgrade;
    private static UpgradeData<int>  _goldPinsValueUpgrade;
    private static UpgradeData<int> _multiPinsCountUpgrade;
    private static UpgradeData<int> _multiPinsValueUpgrade;
    private static UpgradeData<int> _bombPinsCountUpgrade;
    private static UpgradeData<int> _bombPinsValueUpgrade;
    #endregion

    #region Public properties
    public static float Coins => _coins;

    public static UpgradeData<float> BaseUpgrade => _baseUpgrade;
    public static UpgradeData<float> GameSpeedUpgrade => _gameSpeedUpgrade;
    public static UpgradeData<int> WidthUpgrade => _widthUpgrade;
    public static UpgradeData<int> HeightUpgrade => _heightUpgrade;
    public static UpgradeData<int> WinAreasUpgrade => _winAreasUpgrade;
    public static UpgradeData<int> GoldPinsCountUpgrade => _goldPinsCountUpgrade;
    public static UpgradeData<int> GoldPinsValueUpgrade => _goldPinsValueUpgrade;
    public static UpgradeData<int> MultiPinsCountUpgrade => _multiPinsCountUpgrade;
    public static UpgradeData<int> MultiPinsValueUpgrade => _multiPinsValueUpgrade;
    public static UpgradeData<int> BombPinsCountUpgrade => _bombPinsCountUpgrade;
    public static UpgradeData<int> BombPinsValueUpgrade => _bombPinsValueUpgrade;
    #endregion

    public static event System.Action OnCoinsChanged;
    public static event System.Action OnMapUpdate;
    public static event System.Action OnPinsUpdate;
    public static event System.Action OnFinishUpdate;
    public static event System.Action OnGameSpeedUpdate;

    static PlayerData()
    {
        Reset();
    }

    public static void Reset()
    {
        _coins = DefaultCoins;

        _baseUpgrade = new UpgradeData<float>(BaseUpgradeButton.DefaultBaseCoinsRewardFromPin);
        _gameSpeedUpgrade = new UpgradeData<float>(GameSpeedUpgradeButton.DefaultGameSpeed);
        _widthUpgrade = new UpgradeData<int>(WidthUpgradeButton.DefaultWidth);
        _heightUpgrade = new UpgradeData<int>(HeightUpgradeButton.DefaultHeight);
        _winAreasUpgrade = new UpgradeData<int>(WinAreaUpgadeButton.DefaultWinAreasUpgrade);
        _goldPinsCountUpgrade = new UpgradeData<int>(DefaultPinsCount);
        _goldPinsValueUpgrade = new UpgradeData<int>(GoldPinUpgradeAmountButton.DefaultGoldPinRewardMultiplier);
        _multiPinsCountUpgrade = new UpgradeData<int>(DefaultPinsCount);
        _multiPinsValueUpgrade = new UpgradeData<int>(MultiPinUpgradeAmountButton.DefaultMultiPinsValue);
        _bombPinsCountUpgrade = new UpgradeData<int>(DefaultPinsCount);
        _bombPinsValueUpgrade = new UpgradeData<int>(BombPinUpgradeAmountButton.DefaultBombPinValue);

        OnCoinsChanged?.Invoke();
        OnMapUpdate?.Invoke();
        OnPinsUpdate?.Invoke();
    }

    public static bool TryToBuy(float cost)
    {
        if (cost < 0)
        {
            return false;
        }

        if (Coins >= cost)
        {
            _coins -= cost;
            OnCoinsChanged?.Invoke();
            return true;
        }

        return false;
    }

    public static void AddCoins(float amount)
    {
        _coins += amount;
        OnCoinsChanged?.Invoke();
    }

    public static void SetBaseCoinsRewardFromPin(float value)
    {
        if (value < 0)
        {
            return;
        }

        _baseUpgrade.Value = value;
        OnMapUpdate?.Invoke();
    }

    public static int BaseUpgradeLevelUp() => _baseUpgrade.LevelUp();

    public static void SetGameSpeed(float value)
    {
        if (value < 0)
        {
            return;
        }

        _gameSpeedUpgrade.Value = value;
        Time.timeScale = value;
        OnGameSpeedUpdate?.Invoke();
    }

    public static int GameSpeedUpgradeLevelUp() => _gameSpeedUpgrade.LevelUp();

    public static void SetWidth(int value)
    {
        _widthUpgrade.Value = value;
        OnMapUpdate?.Invoke();
    }

    public static int WidthUpgradeLevelUp() => _widthUpgrade.LevelUp();

    public static void SetHeight(int value)
    {
        _heightUpgrade.Value = value;
        OnMapUpdate?.Invoke();
    }

    public static int HeightUpgradeLevelUp() => _heightUpgrade.LevelUp();

    public static void SetWinAreasUpgrade(int value)
    {
        _winAreasUpgrade.Value = value;
        OnFinishUpdate?.Invoke();
    }

    public static int WinAreasUpgradeLevelUp() => _winAreasUpgrade.LevelUp();

    public static void SetGoldPinsCount(int value)
    {
        _goldPinsCountUpgrade.Value = value;
        OnPinsUpdate?.Invoke();
    }

    public static int GoldPinCountUpgradeLevelUp() => _goldPinsCountUpgrade.LevelUp();

    public static void SetGoldPinsValue(int value)
    {
        _goldPinsValueUpgrade.Value = value;
    }

    public static int GoldPinValueUpgradeLevelUp() => _goldPinsValueUpgrade.LevelUp();

    public static void SetMultiPinsCount(int value)
    {
        _multiPinsCountUpgrade.Value = value;
        OnPinsUpdate?.Invoke();
    }

    public static int MultiPinCountUpgradeLevelUp() => _multiPinsCountUpgrade.LevelUp();

    public static void SetMultiPinsValue(int value)
    {
        _multiPinsValueUpgrade.Value = value;
        OnPinsUpdate?.Invoke();
    }

    public static int MultiPinValueUpgradeLevelUp() => _multiPinsValueUpgrade.LevelUp();

    public static void SetBombPinsCount(int value)
    {
        _bombPinsCountUpgrade.Value = value;
        OnPinsUpdate?.Invoke();
    }

    public static int BombPinCountUpgradeLevelUp() => _bombPinsCountUpgrade.LevelUp();

    public static void SetBombPinsValue(int value)
    {
        _bombPinsValueUpgrade.Value = value;
        OnPinsUpdate?.Invoke();
    }

    public static int BombPinValueUpgradeLevelUp() => _bombPinsValueUpgrade.LevelUp();

    public struct UpgradeData<T>
    {
        public T Value;
        private int _level;

        public readonly int Level => _level;

        public UpgradeData(T value)
        {
            Value = value;
            _level = DefaultLevel;
        }

        public int LevelUp()
        {
            return ++_level;
        }
    }
}
