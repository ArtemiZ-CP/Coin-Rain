using UnityEngine;

/// <summary>
/// Contains default debug commands that are available out of the box.
/// </summary>
public class DefaultConsoleCommands : MonoBehaviour
{
    [DebugCommand("Help", DebugConsoleCommandConstants.HelpCommandDescription)]
    public static void ShowHelp()
    {
        var instance = DebugConsole.Instance;
        if (instance == null)
        {
            Debug.LogError(DebugConsoleMessageConstants.InstanceNotFoundError);
            return;
        }

        var helpText = DebugConsoleMessageConstants.CommandsHeader;

        foreach (var command in instance.Commands)
        {
            var displayName = command.Value.OriginalName ?? command.Key;
            helpText += $"{displayName}: {command.Value.Description}\n";
        }

        if (helpText.EndsWith("\n"))
        {
            helpText = helpText[..^1];
        }

        Debug.Log(helpText);
    }

    [DebugCommand("Clear", DebugConsoleCommandConstants.ClearCommandDescription)]
    public static void Clear()
    {
        var instance = DebugConsole.Instance;
        if (instance == null)
        {
            Debug.LogError(DebugConsoleMessageConstants.InstanceNotFoundError);
            return;
        }

        instance.ClearLogs();
    }

    [DebugCommand("FPS", DebugConsoleCommandConstants.FpsCommandDescription)]
    public static void ShowFPS()
    {
        float fps = 1.0f / Time.deltaTime;
        Debug.Log(string.Format(DebugConsoleMessageConstants.CurrentFpsFormat, fps));
    }

    [DebugCommand("Quit", DebugConsoleCommandConstants.QuitCommandDescription)]
    public static void QuitApplication()
    {
        Debug.Log(DebugConsoleMessageConstants.QuittingApplicationMessage);
        Application.Quit();
    }
}
