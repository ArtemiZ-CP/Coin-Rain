using UnityEngine;

public class BlessingConsoleCommands : MonoBehaviour
{
    [DebugCommand("BlessingList", "List all available blessings")]
    public static void BlessingList()
    {
        var blessings = Blessing.GetAllTypes();
        if (blessings == null || blessings.Length == 0)
        {
            Debug.Log("No blessings available.");
            return;
        }

        Debug.Log("Available Blessings:");
        foreach (var blessingType in blessings)
        {
            Debug.Log(blessingType);
        }
    }

    [DebugCommand("AddBlessing", "Add a blessing of a specific type")]
    public static void AddBlessing(Blessing.Type blessingType)
    {
        Blessing blessing = Blessing.Get(blessingType);

        if (blessing == null)
        {
            Debug.LogError($"Blessing of type {blessingType} does not exist.");
            return;
        }

        blessing.Add();
        Debug.Log($"Added blessing of type: {blessingType}");
    }
}
