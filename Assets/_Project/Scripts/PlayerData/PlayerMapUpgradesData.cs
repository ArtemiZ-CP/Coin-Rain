using UnityEngine;

public static class PlayerMapUpgradesData
{
    public const bool DefaultUpgradesUnlocked = true;
    public const int DefaultLevel = -1;
    public const float DefaultGameSpeed = 1f;
    public const int DefaultWidth = 1;
    public const int DefaultHeight = 3;
    public const int DefaultWinAreasUpgrade = 0;

    private static bool _isUpgradeUnlocked;
    private static float _gameSpeedUpgrade;
    private static int _widthUpgrade;
    private static int _heightUpgrade;
    private static int _winAreasUpgrade;

    public static bool IsUpgradeUnlocked => _isUpgradeUnlocked;
    public static float GameSpeedUpgrade => _gameSpeedUpgrade;
    public static int WidthUpgrade => _widthUpgrade;
    public static int HeightUpgrade => _heightUpgrade;
    public static int WinAreasUpgrade => _winAreasUpgrade;

    public static event System.Action OnMapUpdate;
    public static event System.Action OnFinishUpdate;
    public static event System.Action<Pin.Type> OnUpgradePin;

    public static void Reset()
    {
        _isUpgradeUnlocked = DefaultUpgradesUnlocked;
        _gameSpeedUpgrade = DefaultGameSpeed;
        _widthUpgrade = DefaultWidth;
        _heightUpgrade = DefaultHeight;
        _winAreasUpgrade = DefaultWinAreasUpgrade;

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
        _widthUpgrade++;
        OnMapUpdate?.Invoke();
    }

    public static void IncreaseHeight()
    {
        _heightUpgrade++;
        OnMapUpdate?.Invoke();
    }

    public static void SetWinAreasUpgrade(int value)
    {
        _winAreasUpgrade = value;
        OnFinishUpdate?.Invoke();
    }

    public static void IncreaseWinAreaMultiplier()
    {
        _winAreasUpgrade++;
        OnFinishUpdate?.Invoke();
    }
}
