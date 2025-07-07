using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Handles command suggestions and auto-completion for the debug console.
/// </summary>
public static class DebugCommandSuggestions
{
    /// <summary>
    /// Updates command suggestions based on user input.
    /// </summary>
    public static List<string> GetCommandSuggestions(string input, Dictionary<string, CommandInfo> commands)
    {
        var suggestions = new List<string>();
        
        if (input.Length < DebugConsoleCommandConstants.MinInputLengthForSuggestions) return suggestions;

        var commandSuggestions = new List<(string originalCommand, int distance)>();

        foreach (var kvp in commands)
        {
            var commandKey = kvp.Key;
            var commandInfo = kvp.Value;
            var originalCommand = commandInfo.OriginalName ?? commandKey;
            
            if (commandKey.StartsWith(input))
            {
                commandSuggestions.Add((originalCommand, 0));
            }
            else
            {
                int distance = CalculateLevenshteinDistance(input, commandKey);
                int maxDistance = Mathf.Max(DebugConsoleCommandConstants.MinSimilarCommandDistance, input.Length / DebugConsoleCommandConstants.SimilarCommandDistanceDivisor);

                if (distance <= maxDistance)
                {
                    commandSuggestions.Add((originalCommand, distance));
                }
            }
        }

        commandSuggestions.Sort((a, b) => a.distance.CompareTo(b.distance));

        return commandSuggestions
            .Take(DebugConsoleCommandConstants.MaxSuggestions)
            .Select(x => x.originalCommand)
            .ToList();
    }

    /// <summary>
    /// Gets parameter suggestions for a specific command.
    /// </summary>
    public static List<string> GetParameterSuggestions(string commandName, string[] parts, 
        Dictionary<string, CommandInfo> commands, bool inputEndsWithSpace)
    {
        var suggestions = new List<string>();
        
        if (!commands.ContainsKey(commandName))
            return suggestions;

        var commandInfo = commands[commandName];
        var parameters = commandInfo.Method.GetParameters();
        
        int currentParameterIndex;
        
        if (inputEndsWithSpace)
        {
            currentParameterIndex = parts.Length - 1;
        }
        else
        {
            currentParameterIndex = parts.Length - 2;
        }
        
        if (currentParameterIndex >= parameters.Length || currentParameterIndex < 0)
        {
            return suggestions;
        }

        var currentParam = parameters[currentParameterIndex];
        var currentInput = "";
        
        if (!inputEndsWithSpace && parts.Length > 1)
        {
            currentInput = parts[^1];
        }

        return GetParameterSuggestionsForType(currentParam, currentInput);
    }

    /// <summary>
    /// Shows similar commands when an unknown command is entered.
    /// </summary>
    public static void ShowSimilarCommands(string inputCommand, Dictionary<string, CommandInfo> commands)
    {
        var similarCommands = new List<(string originalCommand, int distance)>();

        foreach (var kvp in commands)
        {
            var commandKey = kvp.Key;
            var commandInfo = kvp.Value;
            var originalCommand = commandInfo.OriginalName ?? commandKey;
            
            int distance = CalculateLevenshteinDistance(inputCommand, commandKey);
            int maxDistance = Mathf.Max(DebugConsoleCommandConstants.MinSimilarCommandDistance, inputCommand.Length / DebugConsoleCommandConstants.SimilarCommandDistanceDivisor);

            if (distance <= maxDistance)
            {
                similarCommands.Add((originalCommand, distance));
            }
        }

        if (similarCommands.Count > 0)
        {
            similarCommands.Sort((a, b) => a.distance.CompareTo(b.distance));

            var suggestionsText = DebugConsoleMessageConstants.SimilarCommandsHeader;
            foreach (var (originalCommand, _) in similarCommands.Take(DebugConsoleCommandConstants.MaxSimilarCommandsToShow))
            {
                suggestionsText += $"{DebugConsoleMessageConstants.CommandSuggestionPrefix}{originalCommand}\n";
            }

            Debug.Log(suggestionsText.TrimEnd('\n'));
        }
    }

    /// <summary>
    /// Gets parameter information for display purposes.
    /// </summary>
    public static ParameterInfo GetParameterInfo(string commandName, int parameterIndex, 
        Dictionary<string, CommandInfo> commands)
    {
        if (string.IsNullOrEmpty(commandName) || parameterIndex < 0) 
            return null;
            
        if (!commands.ContainsKey(commandName))
            return null;
            
        var parameters = commands[commandName].Method.GetParameters();
        
        if (parameterIndex >= parameters.Length)
            return null;
            
        return parameters[parameterIndex];
    }

    /// <summary>
    /// Gets a display-friendly parameter type name.
    /// </summary>
    public static string GetParameterTypeDisplayName(Type type)
    {
        if (type.IsEnum)
            return string.Format(DebugConsoleCommandConstants.EnumParameterFormat, string.Join(DebugConsoleCommandConstants.EnumValueSeparator, Enum.GetNames(type)));
        else if (type == typeof(bool))
            return DebugConsoleCommandConstants.BoolParameterDisplay;
        else if (type == typeof(int))
            return DebugConsoleCommandConstants.IntParameterDisplay;
        else if (type == typeof(float))
            return DebugConsoleCommandConstants.FloatParameterDisplay;
        else if (type == typeof(string))
            return DebugConsoleCommandConstants.StringParameterDisplay;
        else
            return type.Name.ToLower();
    }

    private static List<string> GetParameterSuggestionsForType(ParameterInfo parameter, string currentInput)
    {
        var suggestions = new List<string>();
        var paramType = parameter.ParameterType;

        if (paramType.IsEnum)
        {
            var enumValues = Enum.GetNames(paramType);
            
            if (string.IsNullOrEmpty(currentInput))
            {
                suggestions.AddRange(enumValues.Take(DebugConsoleCommandConstants.MaxSuggestions));
            }
            else
            {
                var filtered = enumValues
                    .Where(x => x.ToLower().StartsWith(currentInput.ToLower()))
                    .Take(DebugConsoleCommandConstants.MaxSuggestions);
                suggestions.AddRange(filtered);
            }
        }
        else if (paramType == typeof(bool))
        {
            if (string.IsNullOrEmpty(currentInput))
            {
                suggestions.AddRange(DebugConsoleCommandConstants.BooleanValues);
            }
            else
            {
                var filtered = DebugConsoleCommandConstants.BooleanValues
                    .Where(x => x.StartsWith(currentInput.ToLower()))
                    .Take(DebugConsoleCommandConstants.MaxSuggestions);
                suggestions.AddRange(filtered);
            }
        }
        else if (IsNumericType(paramType))
        {
            if (string.IsNullOrEmpty(currentInput))
            {
                suggestions.AddRange(GetNumericExamples(paramType));
            }
        }
        else if (paramType == typeof(string))
        {
            if (string.IsNullOrEmpty(currentInput))
            {
                suggestions.Add(string.Format(DebugConsoleCommandConstants.UnnamedParameterFormat, parameter.Name));
            }
        }

        return suggestions;
    }

    private static bool IsNumericType(Type type)
    {
        return type == typeof(int) || type == typeof(float) || type == typeof(double) || 
               type == typeof(long) || type == typeof(short) || type == typeof(byte) ||
               type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort) ||
               type == typeof(sbyte) || type == typeof(decimal);
    }

    private static string[] GetNumericExamples(Type type)
    {
        if (type == typeof(int) || type == typeof(uint) || type == typeof(short) || 
            type == typeof(ushort) || type == typeof(byte) || type == typeof(sbyte) ||
            type == typeof(long) || type == typeof(ulong))
        {
            return DebugConsoleCommandConstants.IntegerExamples;
        }
        else if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
        {
            return DebugConsoleCommandConstants.FloatExamples;
        }
        
        return DebugConsoleCommandConstants.DefaultNumericExample;
    }

    /// <summary>
    /// Вычисляет расстояние Левенштейна между двумя строками
    /// </summary>
    private static int CalculateLevenshteinDistance(string source, string target)
    {
        if (string.IsNullOrEmpty(source))
            return string.IsNullOrEmpty(target) ? 0 : target.Length;

        if (string.IsNullOrEmpty(target))
            return source.Length;

        int sourceLength = source.Length;
        int targetLength = target.Length;
        int[,] matrix = new int[sourceLength + 1, targetLength + 1];

        for (int i = 0; i <= sourceLength; i++)
            matrix[i, 0] = i;

        for (int j = 0; j <= targetLength; j++)
            matrix[0, j] = j;

        for (int i = 1; i <= sourceLength; i++)
        {
            for (int j = 1; j <= targetLength; j++)
            {
                int cost = source[i - 1] == target[j - 1] ? 0 : 1;

                matrix[i, j] = Mathf.Min(
                    Mathf.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost
                );
            }
        }

        return matrix[sourceLength, targetLength];
    }
}
