using UnityEngine;

/// <summary>
/// Example class to demonstrate how to use DebugCommand attribute.
/// </summary>
public class PinConsoleCommands : MonoBehaviour
{
    [DebugCommand("PinList", "List all available pins")]
    public static void PinList()
    {
        var pins = Pin.GetAllTypes();
        if (pins == null || pins.Length == 0)
        {
            Debug.Log("No pins available.");
            return;
        }

        Debug.Log("Available Pins:");
        foreach (var pinType in pins)
        {
            Debug.Log(pinType);
        }
    }

    [DebugCommand("AddPin", "Add a pin of a specific type")]
    public static void AddPin(Pin.Type pinType, int count)
    {
        Pin pin = Pin.Get(pinType);

        if (pin == null)
        {
            Debug.LogError($"Pin of type {pinType} does not exist.");
            return;
        }

        pin.Add(count);
        Debug.Log($"Added pin of type: {pinType}");
    }
}
