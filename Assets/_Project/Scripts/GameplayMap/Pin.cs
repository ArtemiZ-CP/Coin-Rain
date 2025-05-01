using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _image;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _goldColor;
    [SerializeField] private Color _multiplyingColor;
    [SerializeField] private Color _bombColor;
    [SerializeField] private Color _touchedColor;

    private bool _isTouched = false;
    private Type _pinType = Type.Base;

    public void Reset()
    {
        _isTouched = false;
        SetPinType(Type.Base);
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
            Type.Base => PlayerData.RewardFromPin,
            Type.Gold => PlayerData.GoldPinsValueUpgrade * PlayerData.RewardFromPin,
            Type.Multiplying => TouchMultiplierPin(playerBall),
            Type.Bomb => TouchBombPin(playerBall),
            _ => throw new System.NotImplementedException(),
        };

        UpdateColor();
        return true;
    }

    private void UpdateColor()
    {
        if (_isTouched)
        {
            _image.color = _touchedColor;
        }
        else
        {
            _image.color = _pinType switch
            {
                Type.Base => _defaultColor,
                Type.Gold => _goldColor,
                Type.Multiplying => _multiplyingColor,
                Type.Bomb => _bombColor,
                _ => throw new System.NotImplementedException(),
            };
        }
    }

    private float TouchMultiplierPin(PlayerBall playerBall)
    {
        float coins = PlayerData.RewardFromPin;

        for (int i = 0; i < PlayerData.MultiPinsValueUpgrade; i++)
        {
            PlayerBall newBall = BallsController.Instance.SpawnBall(playerBall.transform.position);
            newBall.SetRandomImpulse();
            newBall.AddCoins(coins);
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
