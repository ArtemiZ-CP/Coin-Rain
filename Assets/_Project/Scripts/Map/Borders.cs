using UnityEngine;

public class Borders : MonoBehaviour
{
    [SerializeField] private GameObject _leftBorder;
    [SerializeField] private GameObject _rightBorder;
    [SerializeField] private float _boardMaxHeight;
    [SerializeField] private float _boardWidth;

    [ContextMenu("Update Borders")]
    public void UpdateBorders()
    {
        PinConstants pinConstants = GameConstants.Instance.PinConstants;

        float positionX = (PlayerMapData.MapWidth + 1) * pinConstants.OffsetBetweenPinsInLine + _boardWidth / 2;

        _leftBorder.transform.localPosition = -1 * positionX * Vector3.right;
        _rightBorder.transform.localPosition = positionX * Vector3.right;

        float minHeight = -1 * (PlayerMapData.MapHeight - 1) * pinConstants.OffsetBetweenLines - 0.5f;

        _leftBorder.transform.localScale = new Vector3(_boardWidth, _boardMaxHeight - minHeight, 1);
        _rightBorder.transform.localScale = new Vector3(_boardWidth, _boardMaxHeight - minHeight, 1);
        transform.position = (minHeight + _boardMaxHeight) / 2 * Vector3.up;
    }
}
