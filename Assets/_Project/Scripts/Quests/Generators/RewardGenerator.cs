public static class RewardGenerator
{
    public static int[] GetCoinsRewardData()
    {
        return new[] { 10 };
    }

    public static int[] GetDiamondsRewardData()
    {
        return new[] { 10 };
    }

    public static int[] GetUnlockUpgradeRewardData()
    {
        return new[] { (int)Pin.Type.Base };
    }
}
