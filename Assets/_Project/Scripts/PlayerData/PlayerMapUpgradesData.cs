using UnityEngine;

public static class PlayerMapUpgradesData
{
    public const bool DefaultUpgradesUnlocked = true;
    public const int DefaultLevel = -1;
    public const float DefaultGameSpeed = 1f;
    public const int DefaultWidth = 1;
    public const int DefaultHeight = 3;
    public const int DefaultMinFinishMultiplier = 0;

    private static bool _isUpgradeUnlocked;
    private static float _gameSpeedUpgrade;
    private static int _mapWidth;
    private static int _mapHeight;
    private static int _minFinishMultiplier;

    public static bool IsUpgradeUnlocked => _isUpgradeUnlocked;
    public static float GameSpeedUpgrade => _gameSpeedUpgrade;
    public static int MapWidth => _mapWidth;
    public static int MapHeight => _mapHeight;
    public static int MinFinishMultiplier => _minFinishMultiplier;

    public static event System.Action OnMapUpdate;
    public static event System.Action OnFinishUpdate;
    public static event System.Action<Pin.Type> OnUpgradePin;

    public static void Reset()
    {
        _isUpgradeUnlocked = DefaultUpgradesUnlocked;
        _gameSpeedUpgrade = DefaultGameSpeed;
        _mapWidth = DefaultWidth;
        _mapHeight = DefaultHeight;
        _minFinishMultiplier = DefaultMinFinishMultiplier;

        OnMapUpdate?.Invoke();
        OnFinishUpdate?.Invoke();
    }

    public static void UnlockUpgrade(Pin.Type upgradeType)
    {
        if (upgradeType == Pin.Type.Base)
        {
            _isUpgradeUnlocked = true;
        }
        else if (upgradeType == Pin.Type.Gold)
        {
            PlayerPinsData.AddGoldPin();
        }
        else if (upgradeType == Pin.Type.Multiplying)
        {
            PlayerPinsData.AddMultiplyingPin();
        }
        else if (upgradeType == Pin.Type.Bomb)
        {
            PlayerPinsData.AddBombPin();
        }

        OnUpgradePin?.Invoke(upgradeType);
    }

    public static void SetGameSpeed(float value)
    {
        if (value < 0)
        {
            return;
        }

        _gameSpeedUpgrade = value;
        Time.timeScale = value;
    }

    public static void IncreaseWidth()
    {
        _mapWidth++;
        OnMapUpdate?.Invoke();
    }

    public static void IncreaseHeight()
    {
        _mapHeight++;
        OnMapUpdate?.Invoke();
    }

    public static void IncreaseFinishMultiplier(int value = 1)
    {
        if (value < 1)
        {
            return;
        }

        _minFinishMultiplier += value;
        OnFinishUpdate?.Invoke();
    }
}
