public static class PlayerCoinsData
{
    private const float InitialCoins = 50f;

    private static float _coins;

    public static float Coins => _coins;

    public static event System.Action OnCurrencyChanged;

    public static void Reset()
    {
        _coins = InitialCoins;

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
}
