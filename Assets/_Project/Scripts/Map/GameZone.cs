using System;
using UnityEngine;

public class GameZone : MonoBehaviour
{
    public event Action<PlayerBall> OnBallFinished;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerBallCollider playerBallCollider))
        {
            OnBallFinished?.Invoke(playerBallCollider.PlayerBall);
        }
    }

    public void UpdateZone()
    {
        float upBorder = -1 * PlayerMapData.GetMapHeight();
        float height = 1;
        float width = PlayerMapData.GetMapWidth();

        transform.localScale = new Vector3(width, height, 1);
        transform.position = new Vector3(0, upBorder - height, -10);
    }
}
