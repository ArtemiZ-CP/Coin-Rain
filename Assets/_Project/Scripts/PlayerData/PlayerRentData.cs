public static class PlayerRentData
{
    private const int InitialRentCost = 50;
    private const int RentCostIncreaseFactor = 2;

    private static int _rentCost;

    public static int RentCost => _rentCost;

    public static event System.Action OnRentCostChanged;

    public static void Reset()
    {
        _rentCost = InitialRentCost;

        OnRentCostChanged?.Invoke();
    }

    public static void IncreaseRentCost()
    {
        _rentCost *= RentCostIncreaseFactor;
        OnRentCostChanged?.Invoke();
    }
}
