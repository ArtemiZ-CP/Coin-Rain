public static class PlayerMapData
{
    public const int DefaultWidth = 1;
    public const int DefaultHeight = 3;
    public const int DefaultMinFinishMultiplier = 0;

    private static int _mapWidth;
    private static int _mapHeight;
    private static int _minFinishMultiplier;

    public static int MapWidth => _mapWidth;
    public static int MapHeight => _mapHeight;
    public static int MinFinishMultiplier => _minFinishMultiplier;

    public static event System.Action OnMapUpdate;
    public static event System.Action OnFinishUpdate;

    public static void Reset()
    {
        _mapWidth = DefaultWidth;
        _mapHeight = DefaultHeight;
        _minFinishMultiplier = DefaultMinFinishMultiplier;

        OnMapUpdate?.Invoke();
        OnFinishUpdate?.Invoke();
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
