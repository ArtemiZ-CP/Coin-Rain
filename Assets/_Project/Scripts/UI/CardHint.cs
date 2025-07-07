using TMPro;
using UnityEngine;

public class CardHint : MonoBehaviour
{
    [SerializeField] private TMP_Text _cardsCountText;
    [SerializeField] private Card _card;

    public Card.Type CardType => _card.CardType;

    public void Initialize(Card.Type type)
    {
        _card.SetType(type);
    }

    public void UpdateCardsCount(int count)
    {
        _cardsCountText.text = count.ToString();
    }
}
