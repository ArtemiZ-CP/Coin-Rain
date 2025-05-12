using UnityEngine;

public class GameplayMap : MonoBehaviour
{
    [SerializeField] private PinsMap _pinsMap;
    [SerializeField] private GameZone _gameZone;
    [SerializeField] private WinAreas _winAreas;
    [SerializeField] private Borders _borders;
    [SerializeField] private CameraSize _cameraSize;

    private void Start()
    {
        PlayerData.Reset();
    }

    private void OnEnable()
    {
        PlayerData.OnMapUpdate += UpdateMap;
        PlayerData.OnPinsUpdate += UpdatePins;
    }

    private void OnDisable()
    {
        PlayerData.OnMapUpdate -= UpdateMap;
        PlayerData.OnPinsUpdate -= UpdatePins;
    }

    [ContextMenu("Update Map")]
    private void UpdateMap()
    {
        _pinsMap.UpdatePins();
        _winAreas.UpdateAreas();
        _borders.UpdateBorders();
        _cameraSize.UpdateSize();
        _gameZone.UpdateZone();
    }

    private void UpdatePins()
    {
        _pinsMap.Reset();
    }
}
