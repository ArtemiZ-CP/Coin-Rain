using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Handles command registration and management for the debug console.
/// </summary>
public static class DebugCommandRegistry
{
    /// <summary>
    /// Registers all commands found in the scene using reflection.
    /// </summary>
    public static void RegisterCommandsInScene(Dictionary<string, CommandInfo> commands)
    {
        var behaviors = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var behavior in behaviors)
        {
            RegisterCommandsInComponent(behavior, commands);
        }
    }

    /// <summary>
    /// Registers commands from a specific component.
    /// </summary>
    public static void RegisterCommandsInComponent(MonoBehaviour component, Dictionary<string, CommandInfo> commands)
    {
        var methods = component.GetType().GetMethods(DebugConsoleCommandConstants.CommandMethodBindingFlags);

        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<DebugCommandAttribute>();

            if (attribute != null)
            {
                string originalCommandName = attribute.CommandName;
                string commandName = originalCommandName.ToLower();
                
                // Проверяем дубликаты
                if (commands.ContainsKey(commandName))
                {
                    Debug.LogWarning(string.Format(DebugConsoleMessageConstants.DuplicateCommandWarningFormat, commandName, component.GetType().Name));
                    continue;
                }
                
                commands[commandName] = new CommandInfo(method, component, attribute.Description, originalCommandName);
            }
        }
    }

    /// <summary>
    /// Registers a command programmatically.
    /// </summary>
    public static bool RegisterCommand(string commandName, MethodInfo method, object target, 
        string description, Dictionary<string, CommandInfo> commands)
    {
        string originalCommandName = commandName;
        commandName = commandName.ToLower();
        
        // Проверяем, не зарегистрирована ли команда уже
        if (commands.ContainsKey(commandName))
        {
            Debug.LogWarning(string.Format(DebugConsoleMessageConstants.ProgrammaticDuplicateCommandWarningFormat, commandName));
            return false;
        }

        commands[commandName] = new CommandInfo(method, target, description, originalCommandName);
        Debug.Log(string.Format(DebugConsoleMessageConstants.CommandRegisteredFormat, commandName));
        return true;
    }

    /// <summary>
    /// Unregisters a command.
    /// </summary>
    public static bool UnregisterCommand(string commandName, Dictionary<string, CommandInfo> commands)
    {
        commandName = commandName.ToLower();
        
        if (commands.Remove(commandName))
        {
            Debug.Log(string.Format(DebugConsoleMessageConstants.CommandUnregisteredFormat, commandName));
            return true;
        }
        
        Debug.LogWarning(string.Format(DebugConsoleMessageConstants.CommandNotFoundForUnregistrationFormat, commandName));
        return false;
    }

    /// <summary>
    /// Gets all registered command names.
    /// </summary>
    public static string[] GetRegisteredCommands(Dictionary<string, CommandInfo> commands)
    {
        return commands.Keys.ToArray();
    }
}
