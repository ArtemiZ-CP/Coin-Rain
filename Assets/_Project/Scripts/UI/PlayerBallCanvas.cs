using UnityEngine;

public class PlayerBallCanvas : MonoBehaviour
{
    [SerializeField] private Transform _playerBall;

    private Vector3 _offset;

    private void Awake()
    {
        _offset = transform.position - _playerBall.position;
    }

    private void LateUpdate()
    {
        if (_playerBall != null)
        {
            transform.position = _playerBall.position + _offset;
        }
    }
}
