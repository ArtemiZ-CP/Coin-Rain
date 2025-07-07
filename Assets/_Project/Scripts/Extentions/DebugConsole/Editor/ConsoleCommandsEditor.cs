using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Base editor class for displaying and executing console commands in the inspector.
/// </summary>
[CustomEditor(typeof(MonoBehaviour), true)]
public class ConsoleCommandsEditor : Editor
{
    private readonly Dictionary<MethodInfo, object[]> _commandParameters = new();
    private readonly Dictionary<MethodInfo, bool> _commandFoldouts = new();
    private bool _showCommands = true;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var commandMethods = GetDebugCommandMethods();
        if (!commandMethods.Any()) return;

        EditorGUILayout.Space(ConsoleCommandsEditorConstants.MAIN_SECTION_SPACING);
        
        DrawCommandsHeader(commandMethods.Count);
        
        if (_showCommands)
        {
            DrawToolbar(commandMethods.Count);
            DrawCommandsList(commandMethods);
        }
    }

    private List<MethodInfo> GetDebugCommandMethods()
    {
        var targetType = target.GetType();
        var methods = targetType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        
        return methods.Where(method => method.GetCustomAttribute<DebugCommandAttribute>() != null).ToList();
    }

    #region UI Drawing Methods

    private void DrawCommandsHeader(int commandCount)
    {
        var newShowCommands = EditorGUILayout.Foldout(_showCommands, ConsoleCommandsEditorConstants.UI_DEBUG_COMMANDS, true, EditorStyles.foldoutHeader);
        
        if (newShowCommands != _showCommands)
        {
            _showCommands = newShowCommands;
            Repaint();
        }
    }

    private void DrawToolbar(int commandCount)
    {
        EditorGUILayout.Space(ConsoleCommandsEditorConstants.COMMANDS_SECTION_SPACING);
        
        // Компактная информационная панель
        var infoStyle = CreateInfoStyle();
        EditorGUILayout.LabelField(string.Format(ConsoleCommandsEditorConstants.UI_COMMANDS_COUNT, commandCount), infoStyle);
        
        // Панель инструментов
        EditorGUILayout.BeginHorizontal();
        
        DrawToolbarButton(ConsoleCommandsEditorConstants.UI_RESET_BUTTON, Color.yellow, ResetAllParameters);
        DrawToolbarButton(ConsoleCommandsEditorConstants.UI_EXECUTE_ALL_BUTTON, Color.cyan, () => ExecuteAllCommandsWithConfirmation(commandCount));
        
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(ConsoleCommandsEditorConstants.TOOLBAR_SPACING);
    }

    private void DrawCommandsList(List<MethodInfo> commandMethods)
    {
        foreach (var method in commandMethods)
        {
            DrawCommandGUI(method);
            EditorGUILayout.Space(ConsoleCommandsEditorConstants.COMMAND_ITEM_SPACING);
        }
    }

    #endregion

    #region UI Helper Methods

    private GUIStyle CreateInfoStyle()
    {
        var style = new GUIStyle(EditorStyles.helpBox);
        style.fontSize = ConsoleCommandsEditorConstants.INFO_FONT_SIZE;
        style.padding = ConsoleCommandsEditorConstants.INFO_PADDING;
        return style;
    }

    private void DrawToolbarButton(string text, Color backgroundColor, System.Action onClickAction)
    {
        GUI.backgroundColor = backgroundColor;
        if (GUILayout.Button(text, GUILayout.Height(ConsoleCommandsEditorConstants.TOOLBAR_BUTTON_HEIGHT)))
        {
            onClickAction?.Invoke();
        }
    }

    private void ExecuteAllCommandsWithConfirmation(int commandCount)
    {
        if (EditorUtility.DisplayDialog(
            ConsoleCommandsEditorConstants.DIALOG_EXECUTE_ALL_TITLE,
            string.Format(ConsoleCommandsEditorConstants.DIALOG_EXECUTE_ALL_MESSAGE, commandCount),
            ConsoleCommandsEditorConstants.DIALOG_YES,
            ConsoleCommandsEditorConstants.DIALOG_CANCEL))
        {
            ExecuteAllCommands();
        }
    }

    #endregion

    private void DrawCommandGUI(MethodInfo method)
    {
        var attribute = method.GetCustomAttribute<DebugCommandAttribute>();
        var parameters = method.GetParameters();

        InitializeFoldoutState(method);

        var boxStyle = CreateCommandBoxStyle();
        EditorGUILayout.BeginVertical(boxStyle);
        
        DrawCommandHeader(method, attribute, parameters);
        
        if (_commandFoldouts[method])
        {
            DrawCommandContent(method, attribute, parameters);
        }

        EditorGUILayout.EndVertical();
    }

    #region Command Drawing Helper Methods

    private void InitializeFoldoutState(MethodInfo method)
    {
        if (!_commandFoldouts.ContainsKey(method))
        {
            _commandFoldouts[method] = false;
        }
    }

    private GUIStyle CreateCommandBoxStyle()
    {
        var boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.padding = ConsoleCommandsEditorConstants.BOX_PADDING;
        boxStyle.margin = ConsoleCommandsEditorConstants.BOX_MARGIN;
        return boxStyle;
    }

    private void DrawCommandHeader(MethodInfo method, DebugCommandAttribute attribute, ParameterInfo[] parameters)
    {
        EditorGUILayout.BeginHorizontal();
        
        var headerStyle = CreateHeaderStyle();
        var newFoldoutState = EditorGUILayout.Foldout(_commandFoldouts[method], attribute.CommandName, true, headerStyle);
        
        if (newFoldoutState != _commandFoldouts[method])
        {
            _commandFoldouts[method] = newFoldoutState;
            Repaint();
        }
        
        if (parameters.Length > 0)
        {
            EditorGUILayout.LabelField(string.Format(ConsoleCommandsEditorConstants.UI_PARAMETER_COUNT, parameters.Length), EditorStyles.miniLabel, GUILayout.Width(ConsoleCommandsEditorConstants.PARAMETER_COUNT_WIDTH));
        }
        
        DrawExecuteButton(method);
        
        EditorGUILayout.EndHorizontal();
    }

    private GUIStyle CreateHeaderStyle()
    {
        var headerStyle = new GUIStyle(EditorStyles.foldout);
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.fontSize = ConsoleCommandsEditorConstants.HEADER_FONT_SIZE;
        return headerStyle;
    }

    private void DrawExecuteButton(MethodInfo method)
    {
        var compactButtonStyle = CreateExecuteButtonStyle();
        
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button(ConsoleCommandsEditorConstants.UI_EXECUTE_BUTTON, compactButtonStyle, GUILayout.Width(ConsoleCommandsEditorConstants.EXECUTE_BUTTON_WIDTH), GUILayout.Height(ConsoleCommandsEditorConstants.BASE_COMMAND_HEIGHT)))
        {
            ExecuteCommand(method);
        }
        GUI.backgroundColor = Color.white;
    }

    private GUIStyle CreateExecuteButtonStyle()
    {
        var style = new GUIStyle(GUI.skin.button);
        style.fontSize = ConsoleCommandsEditorConstants.BUTTON_FONT_SIZE;
        style.padding = ConsoleCommandsEditorConstants.BUTTON_PADDING;
        return style;
    }

    private void DrawCommandContent(MethodInfo method, DebugCommandAttribute attribute, ParameterInfo[] parameters)
    {
        DrawDescription(attribute);
        DrawParameters(method, parameters);
    }

    private void DrawDescription(DebugCommandAttribute attribute)
    {
        if (string.IsNullOrEmpty(attribute.Description)) return;

        var descStyle = new GUIStyle(EditorStyles.miniLabel);
        descStyle.wordWrap = true;
        descStyle.fontSize = ConsoleCommandsEditorConstants.DESCRIPTION_FONT_SIZE;
        EditorGUILayout.LabelField(attribute.Description, descStyle);
    }

    private void DrawParameters(MethodInfo method, ParameterInfo[] parameters)
    {
        if (parameters.Length == 0) return;

        EnsureParametersInitialized(method, parameters);
        var paramValues = _commandParameters[method];

        if (!ValidateParameterArray(method, paramValues, parameters)) return;

        FixNullParameters(paramValues, parameters);

        for (int i = 0; i < parameters.Length; i++)
        {
            DrawParameterRow(parameters[i], ref paramValues[i]);
        }
    }

    private void EnsureParametersInitialized(MethodInfo method, ParameterInfo[] parameters)
    {
        if (!_commandParameters.ContainsKey(method) || 
            _commandParameters[method] == null || 
            _commandParameters[method].Length != parameters.Length)
        {
            _commandParameters[method] = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                _commandParameters[method][i] = GetDefaultValue(parameters[i].ParameterType);
            }
        }
    }

    private bool ValidateParameterArray(MethodInfo method, object[] paramValues, ParameterInfo[] parameters)
    {
        if (paramValues.Length != parameters.Length)
        {
            var attribute = method.GetCustomAttribute<DebugCommandAttribute>();
            Debug.LogError(string.Format(ConsoleCommandsEditorConstants.ERROR_CRITICAL_SIZE_MISMATCH, attribute.CommandName));
            return false;
        }
        return true;
    }

    private void FixNullParameters(object[] paramValues, ParameterInfo[] parameters)
    {
        for (int i = 0; i < parameters.Length; i++)
        {
            if (paramValues[i] == null)
            {
                paramValues[i] = GetDefaultValue(parameters[i].ParameterType);
            }
        }
    }

    private void DrawParameterRow(ParameterInfo param, ref object value)
    {
        EditorGUILayout.BeginHorizontal();
        
        var paramLabel = string.Format(ConsoleCommandsEditorConstants.UI_PARAMETER_LABEL, param.Name);
        var tooltip = string.Format(ConsoleCommandsEditorConstants.UI_TYPE_TOOLTIP, GetTypeFriendlyName(param.ParameterType));
        var labelContent = new GUIContent(paramLabel, tooltip);
        EditorGUILayout.LabelField(labelContent, EditorStyles.miniLabel, GUILayout.Width(ConsoleCommandsEditorConstants.PARAMETER_LABEL_WIDTH));

        DrawParameterField(param.ParameterType, ref value);

        EditorGUILayout.EndHorizontal();
    }

    #endregion

    #region Command Execution

    private void ExecuteCommand(MethodInfo method)
    {
        try
        {
            var attribute = method.GetCustomAttribute<DebugCommandAttribute>();
            var methodParams = method.GetParameters();
            object[] parameters = PrepareParameters(method, methodParams, attribute);

            object targetInstance = method.IsStatic ? null : target;
            method.Invoke(targetInstance, parameters);
        }
        catch (TargetParameterCountException e)
        {
            Debug.LogError(string.Format(ConsoleCommandsEditorConstants.ERROR_PARAM_COUNT_MISMATCH, method.Name, e.Message));
        }
        catch (ArgumentException e)
        {
            Debug.LogError(string.Format(ConsoleCommandsEditorConstants.ERROR_INVALID_PARAMETER, method.Name, e.Message));
        }
        catch (TargetInvocationException e)
        {
            Debug.LogError(string.Format(ConsoleCommandsEditorConstants.ERROR_EXECUTION_ERROR, method.Name, e.InnerException?.Message ?? e.Message));
        }
        catch (Exception e)
        {
            Debug.LogError(string.Format(ConsoleCommandsEditorConstants.ERROR_UNEXPECTED, method.Name, e.Message));
        }
    }

    private object[] PrepareParameters(MethodInfo method, ParameterInfo[] methodParams, DebugCommandAttribute attribute)
    {
        if (methodParams.Length == 0) return null;

        EnsureParametersExist(method, methodParams, attribute);
        var parameters = _commandParameters[method];
        
        ValidateAndFixParameters(parameters, methodParams, attribute);
        
        return parameters;
    }

    private void EnsureParametersExist(MethodInfo method, ParameterInfo[] methodParams, DebugCommandAttribute attribute)
    {
        if (!_commandParameters.ContainsKey(method))
        {
            _commandParameters[method] = new object[methodParams.Length];
            for (int i = 0; i < methodParams.Length; i++)
            {
                _commandParameters[method][i] = GetDefaultValue(methodParams[i].ParameterType);
            }
        }
    }

    private void ValidateAndFixParameters(object[] parameters, ParameterInfo[] methodParams, DebugCommandAttribute attribute)
    {
        if (parameters == null || parameters.Length != methodParams.Length)
        {
            Debug.LogError(string.Format(ConsoleCommandsEditorConstants.ERROR_PARAM_ARRAY_MISMATCH, attribute.CommandName, methodParams.Length, parameters?.Length ?? 0));
            return;
        }

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i] == null)
            {
                var paramType = methodParams[i].ParameterType;
                parameters[i] = GetDefaultValue(paramType);
                Debug.LogWarning(string.Format(ConsoleCommandsEditorConstants.LOG_FIXED_NULL_PARAM, methodParams[i].Name, paramType.Name, attribute.CommandName));
            }
        }
    }

    #endregion

    #region Parameter Field Drawing

    private void DrawParameterField(Type paramType, ref object value)
    {
        if (paramType == typeof(int))
        {
            value = EditorGUILayout.IntField((int)(value ?? 0));
        }
        else if (paramType == typeof(float))
        {
            value = EditorGUILayout.FloatField((float)(value ?? 0f));
        }
        else if (paramType == typeof(double))
        {
            value = EditorGUILayout.DoubleField((double)(value ?? 0.0));
        }
        else if (paramType == typeof(bool))
        {
            value = EditorGUILayout.Toggle((bool)(value ?? false));
        }
        else if (paramType == typeof(string))
        {
            value = EditorGUILayout.TextField((string)(value ?? ""));
        }
        else if (paramType.IsEnum)
        {
            DrawEnumField(paramType, ref value);
        }
        else if (paramType == typeof(Vector2))
        {
            value = EditorGUILayout.Vector2Field("", (Vector2)(value ?? Vector2.zero));
        }
        else if (paramType == typeof(Vector3))
        {
            value = EditorGUILayout.Vector3Field("", (Vector3)(value ?? Vector3.zero));
        }
        else if (paramType == typeof(Color))
        {
            value = EditorGUILayout.ColorField((Color)(value ?? Color.white));
        }
        else
        {
            EditorGUILayout.LabelField(string.Format(ConsoleCommandsEditorConstants.UI_UNSUPPORTED_TYPE, paramType.Name));
        }
    }

    private void DrawEnumField(Type paramType, ref object value)
    {
        if (value == null)
        {
            var enumValues = Enum.GetValues(paramType);
            value = enumValues.Length > 0 ? enumValues.GetValue(0) : null;
        }
        
        if (value != null)
        {
            value = EditorGUILayout.EnumPopup((Enum)value);
        }
        else
        {
            EditorGUILayout.LabelField(string.Format(ConsoleCommandsEditorConstants.UI_NO_ENUM_VALUES, paramType.Name));
        }
    }

    #endregion

    #region Utility Methods

    private string GetTypeFriendlyName(Type type)
    {
        if (type == typeof(int)) return "int";
        if (type == typeof(float)) return "float";
        if (type == typeof(double)) return "double";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(string)) return "string";
        if (type == typeof(Vector2)) return "Vector2";
        if (type == typeof(Vector3)) return "Vector3";
        if (type == typeof(Color)) return "Color";
        if (type.IsEnum) return $"enum {type.Name}";
        
        return type.Name;
    }

    private object GetDefaultValue(Type type)
    {
        if (type == typeof(int)) return 0;
        if (type == typeof(float)) return 0f;
        if (type == typeof(double)) return 0.0;
        if (type == typeof(bool)) return false;
        if (type == typeof(string)) return "";
        if (type == typeof(Vector2)) return Vector2.zero;
        if (type == typeof(Vector3)) return Vector3.zero;
        if (type == typeof(Color)) return Color.white;
        if (type.IsEnum) 
        {
            var enumValues = Enum.GetValues(type);
            return enumValues.Length > 0 ? enumValues.GetValue(0) : null;
        }
        
        return null;
    }

    #endregion

    #region Command Management

    private void ResetAllParameters()
    {
        var commandMethods = GetDebugCommandMethods();
        
        foreach (var method in commandMethods)
        {
            var parameters = method.GetParameters();
            _commandParameters[method] = new object[parameters.Length];
            
            for (int i = 0; i < parameters.Length; i++)
            {
                _commandParameters[method][i] = GetDefaultValue(parameters[i].ParameterType);
            }
        }
        
        Debug.Log(ConsoleCommandsEditorConstants.LOG_RESET_PARAMETERS);
    }

    private void ExecuteAllCommands()
    {
        var commandMethods = GetDebugCommandMethods();
        
        EditorUtility.DisplayProgressBar(ConsoleCommandsEditorConstants.PROGRESS_EXECUTING_COMMANDS, ConsoleCommandsEditorConstants.PROGRESS_RUNNING_COMMANDS, 0f);
        
        try
        {
            for (int i = 0; i < commandMethods.Count; i++)
            {
                var method = commandMethods[i];
                var attribute = method.GetCustomAttribute<DebugCommandAttribute>();
                
                EditorUtility.DisplayProgressBar(ConsoleCommandsEditorConstants.PROGRESS_EXECUTING_COMMANDS, 
                    string.Format(ConsoleCommandsEditorConstants.PROGRESS_EXECUTING_COMMAND, attribute.CommandName), 
                    (float)i / commandMethods.Count);
                
                ExecuteCommand(method);
                
                System.Threading.Thread.Sleep(ConsoleCommandsEditorConstants.PROGRESS_DELAY_MS);
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    #endregion
}
