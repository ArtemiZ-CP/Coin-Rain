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

    [ContextMenu("Update Zone")]
    public void UpdateZone()
    {
        PinConstants pinConstants = GameConstants.Instance.PinConstants;

        float upBorder = -1 * (PlayerMapUpgradesData.HeightUpgrade - 1) * pinConstants.OffsetBetweenLines;
        float height = 1;
        float width = (PlayerMapUpgradesData.WidthUpgrade + 1) * pinConstants.OffsetBetweenPinsInLine * 2;

        transform.localScale = new Vector3(width, height, 1);
        transform.position = new Vector3(0, upBorder - height, -10);
    }
}
