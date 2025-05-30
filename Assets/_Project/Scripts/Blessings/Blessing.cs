public class Blessing
{
    public static IBlessing GetBlessing(Type type)
    {
        return type switch
        {
            Type.GoldenTouch => new GoldenTouchBlessing(),
            _ => throw new System.NotImplementedException(),
        };
    }

    public enum Type
    {
        GoldenTouch,
    }
}

public interface IBlessing
{
    void OnGet();
    void Upgrade();
}