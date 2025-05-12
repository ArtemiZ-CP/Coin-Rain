using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public const float DefaultCoins = 0;
    public const float DefaultDiamonds = 0;
    public const int DefaultLevel = -1;
    public const int DefaultPinsCount = 0;
    public const int DefaultRewardFromPin = 1;
    public const float DefaultGameSpeed = 1f;
    public const int DefaultWidth = 1;
    public const int DefaultHeight = 3;
    public const float DefaultBallSize = 0.5f;
    public const int DefaultWinAreasUpgrade = 0;
    public const int DefaultGoldPinRewardMultiplier = 2;
    public const int DefaultMultiPinsValue = 1;
    public const int DefaultBombPinValue = 1;

    private static readonly List<PlayerQuests.QuestData> _completedQuests = new();

    private static float _coins;
    private static float _diamonds;
    private static float _rewardFromPin;
    private static bool _isUpgradeUnlocked;
    private static float _gameSpeedUpgrade;
    private static int _widthUpgrade;
    private static int _heightUpgrade;
    private static float _playerBallSize;
    private static int _winAreasUpgrade;
    private static int _goldPinsCountUpgrade;
    private static int _goldPinsValueUpgrade;
    private static int _multiPinsCountUpgrade;
    private static int _multiPinsValueUpgrade;
    private static int _bombPinsCountUpgrade;
    private static int _bombPinsValueUpgrade;

    public static IReadOnlyList<PlayerQuests.QuestData> CompletedQuests => _completedQuests.AsReadOnly();
    public static float Coins => _coins;
    public static float Diamonds => _diamonds;
    public static float RewardFromPin => _rewardFromPin;
    public static bool IsUpgradeUnlocked => _isUpgradeUnlocked;
    public static float GameSpeedUpgrade => _gameSpeedUpgrade;
    public static int WidthUpgrade => _widthUpgrade;
    public static int HeightUpgrade => _heightUpgrade;
    public static float PlayerBallSize => _playerBallSize;
    public static int WinAreasUpgrade => _winAreasUpgrade;
    public static int GoldPinsCountUpgrade => _goldPinsCountUpgrade;
    public static int GoldPinsValueUpgrade => _goldPinsValueUpgrade;
    public static int MultiPinsCountUpgrade => _multiPinsCountUpgrade;
    public static int MultiPinsValueUpgrade => _multiPinsValueUpgrade;
    public static int BombPinsCountUpgrade => _bombPinsCountUpgrade;
    public static int BombPinsValueUpgrade => _bombPinsValueUpgrade;

    public static event System.Action OnCoinsChanged;
    public static event System.Action OnMapUpdate;
    public static event System.Action OnPinsUpdate;
    public static event System.Action OnFinishUpdate;
    public static event System.Action OnGameSpeedUpdate;
    public static event System.Action OnPlayerBallSizeUpdate;
    public static event System.Action<Pin.Type> OnUpgradeUnlock;

    static PlayerData()
    {
        Reset();
    }
    
    public static void Reset()
    {
        _coins = DefaultCoins;
        _diamonds = DefaultDiamonds;
        _rewardFromPin = DefaultRewardFromPin;
        _isUpgradeUnlocked = false;
        _gameSpeedUpgrade = DefaultGameSpeed;
        _widthUpgrade = DefaultWidth;
        _heightUpgrade = DefaultHeight;
        _playerBallSize = DefaultBallSize;
        _winAreasUpgrade = DefaultWinAreasUpgrade;
        _goldPinsCountUpgrade = DefaultPinsCount;
        _goldPinsValueUpgrade = DefaultGoldPinRewardMultiplier;
        _multiPinsCountUpgrade = DefaultPinsCount;
        _multiPinsValueUpgrade = DefaultMultiPinsValue;
        _bombPinsCountUpgrade = DefaultPinsCount;
        _bombPinsValueUpgrade = DefaultBombPinValue;

        OnCoinsChanged?.Invoke();
        OnMapUpdate?.Invoke();
        OnPinsUpdate?.Invoke();
        OnFinishUpdate?.Invoke();
        OnGameSpeedUpdate?.Invoke();
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

    public static void CompleteQuest(Quest quest)
    {
        _completedQuests.Add(PlayerQuests.GetQuestData(quest));
    }

    public static void UnlockUpgrade(Pin.Type upgradeType)
    {
        if (upgradeType == Pin.Type.Base)
        {
            _isUpgradeUnlocked = true;
        }
        else if (upgradeType == Pin.Type.Gold)
        {
            AddGoldPin();
        }
        else if (upgradeType == Pin.Type.Multiplying)
        {
            AddMultiplyingPin();
        }
        else if (upgradeType == Pin.Type.Bomb)
        {
            AddBombPin();
        }

        OnUpgradeUnlock?.Invoke(upgradeType);
    }

    public static void SetRewardFromPin(float value)
    {
        if (value < 0)
        {
            return;
        }

        _rewardFromPin = value;
        OnMapUpdate?.Invoke();
    }

    public static void SetGameSpeed(float value)
    {
        if (value < 0)
        {
            return;
        }

        _gameSpeedUpgrade = value;
        Time.timeScale = value;
        OnGameSpeedUpdate?.Invoke();
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

    public static void IncreaseBallSize(float value)
    {
        if (value < 0)
        {
            return;
        }

        _playerBallSize += value;
        OnPlayerBallSizeUpdate?.Invoke();
    }

    public static void SetWinAreasUpgrade(int value)
    {
        _winAreasUpgrade = value;
        OnFinishUpdate?.Invoke();
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
        OnPinsUpdate?.Invoke();
    }

    public static void AddBombPin()
    {
        _bombPinsCountUpgrade++;
        OnPinsUpdate?.Invoke();
    }

    public static void SetBombPinsValue(int value)
    {
        _bombPinsValueUpgrade = value;
        OnPinsUpdate?.Invoke();
    }

    public static void IncreaseWinAreaMultiplier()
    {
        _winAreasUpgrade++;
        OnFinishUpdate?.Invoke();
    }
}
