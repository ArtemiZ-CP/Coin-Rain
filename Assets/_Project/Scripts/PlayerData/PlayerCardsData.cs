using System.Collections.Generic;
using UnityEngine;

public static class PlayerCardsData
{
    public const int DefaultCardsCountInRow = 3;
    public const int DefaultBlessedCardsCount = 1;
    public const int DefaultCursedCardsCount = 1;
    public const int DefaultThrowCardsCount = 3;

    private static List<Vector2Int> _cardsPositions = new();
    private static int _blessedCardsCount;
    private static int _cursedCardsCount;
    private static int _turnsCount;

    public static IReadOnlyList<Vector2Int> CardsPositions => _cardsPositions;
    public static int BlessedCardsCount => _blessedCardsCount;
    public static int CursedCardsCount => _cursedCardsCount;
    public static int TurnsCount => _turnsCount;

    static PlayerCardsData()
    {
        for (int i = 0; i < DefaultCardsCountInRow; i++)
        {
            for (int j = 0; j < DefaultCardsCountInRow; j++)
            {
                _cardsPositions.Add(new Vector2Int(i, j));
            }
        }

        _blessedCardsCount = DefaultBlessedCardsCount;
        _cursedCardsCount = DefaultCursedCardsCount;
    }

    public static int GetTurnsCount(Card.Type type)
    {
        return type switch
        {
            Card.Type.Blessed => 2,
            Card.Type.Base => 1,
            Card.Type.Cursed => 0,
            _ => throw new System.ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static void AddTurns(Card.Type type)
    {
        _turnsCount += GetTurnsCount(type);
    }

    public static void ResetTurns()
    {
        _turnsCount = 0;
    }
}
