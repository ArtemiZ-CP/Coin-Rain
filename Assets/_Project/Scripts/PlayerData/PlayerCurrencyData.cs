public static class PlayerCurrencyData
{
    public const float DefaultCoins = 0;
    public const float DefaultDiamonds = 0;

    private static float _coins;
    private static float _diamonds;

    public static float Coins => _coins;
    public static float Diamonds => _diamonds;

    public static event System.Action OnCurrencyChanged;

    public static void Reset()
    {
        _coins = DefaultCoins;
        _diamonds = DefaultDiamonds;

        OnCurrencyChanged?.Invoke();
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
            OnCurrencyChanged?.Invoke();
            return true;
        }

        return false;
    }

    public static void AddCoins(float amount)
    {
        _coins += amount;
        OnCurrencyChanged?.Invoke();
    }

    public static void AddDiamonds(float amount)
    {
        _diamonds += amount;
        OnCurrencyChanged?.Invoke();
    }
}
