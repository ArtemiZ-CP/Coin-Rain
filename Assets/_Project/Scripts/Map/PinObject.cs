using UnityEngine;

public class PinObject : MonoBehaviour
{
    [SerializeField] private GameObject _dotPin;
    [SerializeField] private GameObject _linePin;
    [SerializeField] private SpriteRenderer _dotPinImage;
    [SerializeField] private SpriteRenderer _linePinImage;
    [SerializeField] private Color _touchedColor;

    private int _position;
    private bool _isTouched = false;
    private PinItem _pinItem;

    public PinItem PinItem => _pinItem;

    public void ResetPin()
    {
        ResetPin(_position);
    }

    public void ResetPin(int position)
    {
        _isTouched = false;
        _position = position;
        SetPin(_pinItem);

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
    }

    public void SetPin(PinItem pinItem)
    {
        _pinItem = pinItem;
        UpdateColor();
    }

    public bool TryTouch(PlayerBall playerBall, out float coins)
    {
        if (_isTouched)
        {
            coins = 0;
            return false;
        }

        _isTouched = true;

        Pin pin = Pin.Get(_pinItem.Type);
        coins = pin.Touch(this, playerBall);
        UpdateColor();

        return true;
    }

    private void UpdateColor()
    {
        Color color;

        if (_isTouched)
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