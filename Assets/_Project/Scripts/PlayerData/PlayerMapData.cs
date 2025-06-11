using UnityEngine;

public static class PlayerMapData
{
    private const int DefaultMapWidth = 5;
    private const int DefaultMapHeight = 5;
    private const int DefaultMapDurability = 1;
    private const float DefaultBounce = 0.5f;
    private const float DefaultGravity = 10f;

    private static int _mapWidth;
    private static int _mapHeight;
    private static int _mapDurability;
    private static float _bounce;
    private static float _gravity;

    public static int LinesCount => _mapHeight;
    public static int MapDurability => _mapDurability;
    public static float Bounce => _bounce;
    public static float Gravity => _gravity;

    public static event System.Action OnMapUpdate;
    public static event System.Action OnMaterialUpdate;

    public static void Reset()
    {
        _mapWidth = DefaultMapWidth;
        _mapHeight = DefaultMapHeight;
        _mapDurability = DefaultMapDurability;
        _bounce = DefaultBounce;
        _gravity = DefaultGravity;

        OnMapUpdate?.Invoke(); 
        OnMaterialUpdate?.Invoke();
    }

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

    public static void IncreaseDurability(int value = 1)
    {
        _mapDurability += value;
        OnMapUpdate?.Invoke();
    }

    public static void IncreaseWidthAndHeight(int widht = 1, int height = 1)
    {
        _mapWidth += widht;
        _mapHeight += height;
        OnMapUpdate?.Invoke();
    }

    public static void IncreaseBounce(float value = 0.1f)
    {
        _bounce += value;

        if (_bounce > 1f)
        {
            _bounce = 1f;
        }
        else if (_bounce < 0f)
        {
            _bounce = 0f;
        }

        OnMaterialUpdate?.Invoke();
    }

    public static void ReduceGravity(float value = 1)
    {
        _gravity -= value;

        if (_gravity < 0f)
        {
            _gravity = 0f;
        }

        Physics2D.gravity = _gravity * Vector2.down;
    }
}
