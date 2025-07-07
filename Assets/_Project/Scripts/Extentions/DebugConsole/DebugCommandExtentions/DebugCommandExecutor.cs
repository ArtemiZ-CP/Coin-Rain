using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Handles command execution and parameter conversion for the debug console.
/// </summary>
public static class DebugCommandExecutor
{
    /// <summary>
    /// Executes a command with the given input string.
    /// </summary>
    public static void ExecuteCommand(string input, Dictionary<string, CommandInfo> commands)
    {
        if (string.IsNullOrEmpty(input)) return;

        var args = input.Split(DebugConsoleCommandConstants.CommandSeparator);
        var commandName = args[0];

        var commandInfo = FindCommandByName(commandName, commands);
        if (commandInfo != null)
        {
            try
            {
                var parameters = args.Skip(1).ToArray();
                var methodParams = commandInfo.Method.GetParameters();
                var convertedParams = ConvertParameters(parameters, methodParams);

                Debug.Log(string.Format(DebugConsoleMessageConstants.CommandExecutedFormat, input));
                commandInfo.Method.Invoke(commandInfo.Target, convertedParams);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format(DebugConsoleMessageConstants.ErrorExecutingCommandFormat, e.Message));
            }
        }
        else
        {
            Debug.LogWarning(string.Format(DebugConsoleMessageConstants.UnknownCommandFormat, commandName));
            DebugCommandSuggestions.ShowSimilarCommands(commandName.ToLower(), commands);
        }
    }

    /// <summary>
    /// Converts string parameters to the required method parameter types.
    /// </summary>
    private static object[] ConvertParameters(string[] inputParams, ParameterInfo[] methodParams)
    {
        var result = new object[methodParams.Length];

        for (int i = 0; i < methodParams.Length; i++)
        {
            if (i >= inputParams.Length)
            {
                if (methodParams[i].HasDefaultValue)
                {
                    result[i] = methodParams[i].DefaultValue;
                }

                continue;
            }

            var paramType = methodParams[i].ParameterType;
            var inputValue = inputParams[i];

            // Специальная обработка для enum'ов
            if (paramType.IsEnum)
            {
                try
                {
                    result[i] = Enum.Parse(paramType, inputValue, true); // true для игнорирования регистра
                }
                catch (ArgumentException)
                {
                    throw new ArgumentException(string.Format(DebugConsoleMessageConstants.InvalidEnumValueFormat, inputValue, paramType.Name, string.Join(DebugConsoleCommandConstants.EnumValueListSeparator, Enum.GetNames(paramType))));
                }
            }
            else
            {
                result[i] = Convert.ChangeType(inputValue, paramType);
            }
        }

        return result;
    }

    /// <summary>
    /// Находит команду по оригинальному имени или ключу
    /// </summary>
    private static CommandInfo FindCommandByName(string inputName, Dictionary<string, CommandInfo> commands)
    {
        // Сначала ищем по нижнему регистру
        var lowerName = inputName.ToLower();
        if (commands.TryGetValue(lowerName, out CommandInfo commandInfo))
        {
            return commandInfo;
        }
        
        // Если не найдено, ищем по оригинальному имени (без учета регистра)
        foreach (var kvp in commands.Values)
        {
            if (kvp.OriginalName != null && kvp.OriginalName.Equals(inputName, System.StringComparison.OrdinalIgnoreCase))
            {
                return kvp;
            }
        }
        
        return null;
    }
}
