public static class PlayerMapData
{
    public const int DefaultWidth = 5;
    public const int DefaultHeight = 5;

    private static int _mapWidth;
    private static int _mapHeight;

    public static int LinesCount => _mapHeight;

    public static event System.Action OnMapUpdate;

    public static float GetMapWidth()
    {
        PinConstants pinConstants = GameConstants.Instance.PinConstants;
        return _mapWidth * pinConstants.OffsetBetweenPinsInLine;
    }

    public static float GetMapHeight()
    {
        PinConstants pinConstants = GameConstants.Instance.PinConstants;
        return LinesCount * pinConstants.OffsetBetweenLines;
    }

    public static int GetPinsCount(bool firstLine)
    {
        int pinsCount = firstLine ? _mapWidth - 1 : _mapWidth;
        return pinsCount;
    }

    public static void Reset()
    {
        _mapWidth = DefaultWidth;
        _mapHeight = DefaultHeight;

        OnMapUpdate?.Invoke();
    }

    public static void IncreaseWidth(int value = 1)
    {
        _mapWidth += value;
        OnMapUpdate?.Invoke();
    }

    public static void IncreaseHeight(int value = 1)
    {
        _mapHeight += value;
        OnMapUpdate?.Invoke();
    }

    public static void IncreaseWidthAndHeight(int widht = 1, int height = 1)
    {
        _mapWidth += widht;
        _mapHeight += height;
        OnMapUpdate?.Invoke();
    }
}
