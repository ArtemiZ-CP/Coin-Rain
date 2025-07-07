using UnityEngine;

public class ThrowConsoleCommands : MonoBehaviour
{
    [DebugCommand("AddThrow", "Add a single throw")]
    public static void AddThrow()
    {
        PlayerCardsData.AddThrow(1);
    }

    [DebugCommand("AddThrows", "Add multiple throws")]
    public static void AddThrows(int count)
    {
        PlayerCardsData.AddThrow(count);
    }
}
