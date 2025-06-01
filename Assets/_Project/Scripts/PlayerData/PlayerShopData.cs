public static class PlayerShopData
{
    private const int StartCapacity = 3;

    private static int _capacity;

    public static int Capacity => _capacity;

    public static void Reset()
    {
        _capacity = StartCapacity;
    }

    public static void IncreaseCapacity(int amount = 1)
    {
        _capacity += amount;
    }
}
