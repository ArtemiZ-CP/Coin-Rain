using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private GameObject _dotPin;
    [SerializeField] private GameObject _linePin;
    [SerializeField] private SpriteRenderer _dotPinImage;
    [SerializeField] private SpriteRenderer _linePinImage;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _goldColor;
    [SerializeField] private Color _multiplyingColor;
    [SerializeField] private Color _bombColor;
    [SerializeField] private Color _touchedColor;

    private int _position;
    private bool _isTouched = false;
    private Type _pinType = Type.Base;

    public Type PinType => _pinType;

    public void ResetPin()
    {
        ResetPin(_position);
    }

    public void ResetPin(int position)
    {
        _isTouched = false;
        _position = position;
        SetPinType(Type.Base);

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

    public void SetPinType(Type pinType)
    {
        _pinType = pinType;
        UpdateColor();
    }

    public bool Touch(PlayerBall playerBall, out float coins)
    {
        if (_isTouched)
        {
            coins = 0;
            return false;
        }

        _isTouched = true;

        coins = _pinType switch
        {
            Type.Base => PlayerPinsData.RewardFromPin,
            Type.Gold => PlayerPinsData.GoldPinsValueUpgrade * PlayerPinsData.RewardFromPin,
            Type.Multiplying => TouchMultiplierPin(playerBall),
            Type.Bomb => TouchBombPin(playerBall),
            _ => throw new System.NotImplementedException(),
        };

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
            color = _pinType switch
            {
                Type.Base => _defaultColor,
                Type.Gold => _goldColor,
                Type.Multiplying => _multiplyingColor,
                Type.Bomb => _bombColor,
                _ => throw new System.NotImplementedException(),
            };
        }

        _dotPinImage.color = color;
        _linePinImage.color = color;
    }

    private float TouchMultiplierPin(PlayerBall playerBall)
    {
        float coins = PlayerPinsData.RewardFromPin;

        for (int i = 0; i < PlayerPinsData.MultiPinsValueUpgrade; i++)
        {
            PlayerBall newBall = BallsController.Instance.SpawnBall(playerBall.Position);
            newBall.SetRandomImpulse();
            newBall.AddCoins(playerBall.TemporaryCoins + coins);
        }

        return coins;
    }

    private float TouchBombPin(PlayerBall playerBall)
    {
        return BallsController.Instance.Blast(this, playerBall);
    }

    public enum Type
    {
        Base,
        Gold,
        Multiplying,
        Bomb,
    }
}
