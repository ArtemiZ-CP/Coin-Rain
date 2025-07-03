using System.Collections.Generic;
using UnityEngine;

public static class PlayerCardsData
{
    public const int DefaultCardsCountInRow = 3;

    public static List<Vector2Int> _cardsPositions = new();

    public static IReadOnlyList<Vector2Int> CardsPositions => _cardsPositions;

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
}
