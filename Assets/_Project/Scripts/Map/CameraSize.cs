using UnityEngine;

public class CameraSize : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _upOffset = 5;
    [SerializeField] private float _downOffset = 2;

    public void UpdateSize()
    {
        float downBorder = -1 * PlayerMapData.GetMapHeight() - _downOffset;
        float height = _upOffset - downBorder;
        float sizeByHeight = height / 2f;
        float sizeByWidth = PlayerMapData.GetMapWidth() / _camera.aspect;

        _camera.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
        _camera.transform.position = new Vector3(0, (_upOffset + downBorder) / 2, -10);
    }
}