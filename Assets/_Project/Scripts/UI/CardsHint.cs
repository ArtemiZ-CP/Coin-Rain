using System.Collections.Generic;
using UnityEngine;

public class CardsHint : MonoBehaviour
{
    private readonly List<CardHint> _cardHints = new();

    [SerializeField] private CardHint _cardHintPrefab;

    public void Initialize()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Card.Type type in System.Enum.GetValues(typeof(Card.Type)))
        {
            var cardHint = Instantiate(_cardHintPrefab, transform);
            cardHint.Initialize(type);
            _cardHints.Add(cardHint);
        }
    }

    public void SetCardsHint(List<(Card.Type type, int count)> cardTypes)
    {
        foreach ((Card.Type type, int count) in cardTypes)
        {
            UpdateCardsCount(type, count);
        }
    }

    public void UpdateCardsCount(Card.Type type, int count)
    {
        CardHint cardHint = _cardHints.Find(hint => hint.CardType == type);

        if (cardHint != null)
        {
            cardHint.UpdateCardsCount(count);
        }
    }
}
