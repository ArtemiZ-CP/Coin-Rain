using System;
using UnityEngine;

public class GameZone : MonoBehaviour
{
    [SerializeField] private float _upBorder;

    public event Action<PlayerBall> OnBallFinished;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerBall playerBall))
        {
            OnBallFinished?.Invoke(playerBall);
        }
    }

    [ContextMenu("Update Zone")]
    public void UpdateZone()
    {
        PinConstants pinConstants = GameConstants.Instance.PinConstants;

        float downBorder = -1 * PlayerData.HeightUpgrade.Value * pinConstants.OffsetBetweenLines;

        float height = _upBorder - downBorder;
        float width = (PlayerData.WidthUpgrade.Value + 1) * pinConstants.OffsetBetweenPinsInLine * 2;

        transform.localScale = new Vector3(width, height, 1);
        transform.position = new Vector3(0, (_upBorder + downBorder) / 2, -10);
    }
}
