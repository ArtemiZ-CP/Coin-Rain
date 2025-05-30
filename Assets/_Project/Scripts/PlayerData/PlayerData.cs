public static class PlayerData
{
    public static void Reset()
    {
        PlayerCoinsData.Reset();
        Pin.ResetAll();
        PlayerMapData.Reset();
    }
}
