using System.Collections.Generic;
using UnityEngine;

public static class PlayerCardsData
{
    public const int DefaultCardsCountInRow = 3;
    public const int DefaultBlessedCardsCount = 1;
    public const int DefaultCursedCardsCount = 1;
    public const int DefaultThrowCardsCount = 3;

    private static List<Vector2Int> _cardsPositions = new();
    private static int _blessedCardsCount = DefaultBlessedCardsCount;
    private static int _cursedCardsCount = DefaultCursedCardsCount;
    private static int _throwCardsCount = DefaultThrowCardsCount;
    private static int _throwCount;

    public static IReadOnlyList<Vector2Int> CardsPositions => _cardsPositions;
    public static int BlessedCardsCount => _blessedCardsCount;
    public static int CursedCardsCount => _cursedCardsCount;
    public static int ThrowCardsCount => _throwCardsCount;
    public static int ThrowCount => _throwCount;

    static PlayerCardsData()
    {
        for (int i = 0; i < DefaultCardsCountInRow; i++)
        {
            for (int j = 0; j < DefaultCardsCountInRow; j++)
            {
                _cardsPositions.Add(new Vector2Int(i, j));
            }
        }
    }

    public static void AddThrow(int count)
    {
        _throwCount += count;
    }

    public static void ResetTurns()
    {
        _throwCount = 0;
    }
}
