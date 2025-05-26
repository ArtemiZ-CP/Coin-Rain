public static class ObjectiveGenerator
{
    public static int[] GetHitFinishObjectiveData()
    {
        return new[] { 1 };
    }

    public static int[] GetHitPinsObjectiveData()
    {
        return new[] { 20, (int)Pin.Type.Base };
    }

    public static float[] GetEarnCoinsByOneBallObjectiveData()
    {
        return new[] { 30f };
    }

    public static float[] GetEarnCoinsByAllBallsObjectiveData()
    {
        return new[] { 30f };
    }
    
    public static float[] GetFallTimeObjectiveData()
    {
        return new[] { 3f };
    }
}
