using UnityEngine;

public class CameraSize : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RectTransform _uiPanel;
    [SerializeField] private float _upBorder;

    [ContextMenu("Update Size")]
    public void UpdateSize()
    {
        PinConstants pinConstants = GameConstants.Instance.PinConstants;

        float downBorder = -1 * PlayerMapUpgradesData.MapHeight * pinConstants.OffsetBetweenLines;
        float height = _upBorder - downBorder;
        float widthPaddingPersantage = (_uiPanel.rect.xMax + _uiPanel.position.x) * 2 / _camera.pixelWidth;
        float width = (PlayerMapUpgradesData.MapWidth + 1) * 2 * pinConstants.OffsetBetweenPinsInLine;
        float widthPadding = width / (1 - widthPaddingPersantage);
        float aspect = _camera.aspect;
        float sizeByHeight = height / 2f;
        float sizeByWidth = widthPadding / (2f * aspect);

        _camera.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
        _camera.transform.position = new Vector3(0, (_upBorder + downBorder) / 2, -10);
    }
}