using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private readonly List<Card> _cards = new();

    [SerializeField] private Card _cardPrefab;
    [SerializeField] private Transform _cardsParent;

    public event Action<Card> OnCardClick;

    public int GetActiveCardCount()
    {
        int count = 0;

        foreach (Card card in _cards)
        {
            if (card.IsActive)
            {
                count++;
            }
        }

        return count;
    }

    public void CreateCards(int cardCount)
    {
        Show();
        ClearCards();

        for (int i = 0; i < cardCount; i++)
        {
            Card card = Instantiate(_cardPrefab, _cardsParent);
            _cards.Add(card);
            InitializeCard(card);
            card.OnCardClick += HandleCardClick;
        }
    }

    public void ClearCards()
    {
        foreach (Card card in _cards)
        {
            card.OnCardClick -= HandleCardClick;
            Destroy(card.gameObject);
        }

        _cards.Clear();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void InitializeCard(Card card)
    {
        Card.Type type = (Card.Type)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Card.Type)).Length);
        card.Initialize((int)type, type);
    }

    private void HandleCardClick(Card card)
    {
        OnCardClick?.Invoke(card);
    }
}
