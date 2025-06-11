using UnityEngine;

public class PinObject : MonoBehaviour
{
    [SerializeField] private GameObject _dotPin;
    [SerializeField] private GameObject _linePin;
    [SerializeField] private SpriteRenderer _dotPinImage;
    [SerializeField] private SpriteRenderer _linePinImage;
    [SerializeField] private Color _touchedColor;

    private int _position;
    private int _durability;
    private PinItem _pinItem;

    public PinItem PinItem => _pinItem;

    public void ResetPin(int position)
    {
        _position = position;

        if (_position == 0)
        {
            _dotPin.SetActive(true);
            _linePin.SetActive(false);
        }
        else
        {
            _dotPin.SetActive(false);
            _linePin.SetActive(true);

            if (_position == -1)
            {
                _linePin.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                _linePin.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (_pinItem != null)
        {
            _durability = Pin.Get(_pinItem.Type).Durability + PlayerMapData.MapDurability;
        }
    }

    public void SetPin(PinItem pinItem)
    {
        _pinItem = pinItem;
        ResetPin();
        UpdateColor();
    }

    public bool TryTouch(PlayerBall playerBall, out float coins)
    {
        if (_durability <= 0)
        {
            coins = 0;
            return false;
        }

        _durability--;

        Pin pin = Pin.Get(_pinItem.Type);
        coins = pin.Touch(this, playerBall);
        UpdateColor();

        return true;
    }

    private void ResetPin()
    {
        ResetPin(_position);
    }

    private void UpdateColor()
    {
        Color color;

        if (_durability <= 0)
        {
            color = _touchedColor;
        }
        else
        {
            color = _pinItem.Color;
        }

        _dotPinImage.color = color;
        _linePinImage.color = color;
    }
}