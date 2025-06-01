public static class PlayerData
{
    public static void Reset()
    {
        PlayerCoinsData.Reset();
        PlayerFinishData.Reset();
        PlayerMapData.Reset();
        PlayerShopData.Reset();
        PlayerRentData.Reset();
        Pin.ResetAll();
        BallsController.Instance.Reset();
    }
}
