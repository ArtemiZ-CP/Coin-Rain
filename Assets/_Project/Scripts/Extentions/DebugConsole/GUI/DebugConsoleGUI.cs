using UnityEngine;

/// <summary>
/// Handles GUI rendering for the debug console.
/// </summary>
public class DebugConsoleGUI
{
    private readonly DebugConsole _console;
    private GUIStyle _logStyle;
    private GUIStyle _inputStyle;
    private GUIStyle _buttonStyle;
    private GUIStyle _backgroundStyle;
    private GUIStyle _suggestionStyle;

    public DebugConsoleGUI(DebugConsole console)
    {
        _console = console;
    }

    public void OnGUI()
    {
        if (!_console.IsVisible) return;

        HandleKeyboardEvents();
        SetupStyles();
        DrawConsoleWindow();
    }

    private void HandleKeyboardEvents()
    {
        var currentEvent = Event.current;

        if (currentEvent.type == EventType.KeyDown || currentEvent.type == EventType.KeyUp || currentEvent.type == EventType.Used)
        {
            if (currentEvent.keyCode == DebugConsoleInputConstants.DefaultToggleKey && currentEvent.type == EventType.KeyDown)
            {
                _console.ToggleConsole();
            }

            if (_console.ShowSuggestions && _console.Suggestions.Count > 0)
            {
                if (currentEvent.keyCode == DebugConsoleInputConstants.TabKey && currentEvent.type == EventType.KeyDown)
                {
                    _console.HandleTabCompletion();
                }
                else if (currentEvent.keyCode == DebugConsoleInputConstants.UpArrowKey)
                {
                    if (currentEvent.type == EventType.KeyDown)
                    {
                        _console.MoveSuggestionSelection(-1);
                    }
                }
                else if (currentEvent.keyCode == DebugConsoleInputConstants.DownArrowKey)
                {
                    if (currentEvent.type == EventType.KeyDown)
                    {
                        _console.MoveSuggestionSelection(1);
                    }
                }
            }
        }
    }

    private void SetupStyles()
    {
        _logStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = _console.FontSize,
            richText = true,
            normal = { textColor = _console.TextColor }
        };

        _inputStyle = new GUIStyle(GUI.skin.textField)
        {
            fontSize = _console.FontSize,
            fixedHeight = _console.FontSize * DebugConsoleUIConstants.InputFieldHeightMultiplier,
            normal = { textColor = _console.TextColor },
            focused = { textColor = _console.TextColor }
        };

        _buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = _console.FontSize,
            fixedHeight = _console.FontSize * DebugConsoleUIConstants.InputFieldHeightMultiplier,
            normal = { textColor = _console.TextColor }
        };

        _suggestionStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = _console.FontSize - DebugConsoleUIConstants.SuggestionButtonFontReduction,
            normal = { textColor = DebugConsoleColorConstants.SuggestionTextColor },
            fixedHeight = _console.FontSize * DebugConsoleUIConstants.SuggestionButtonHeightMultiplier
        };

        _backgroundStyle = new GUIStyle(GUI.skin.box);
        var backgroundTexture = new Texture2D(1, 1);
        backgroundTexture.SetPixel(0, 0, _console.BackgroundColor);
        backgroundTexture.Apply();
        _backgroundStyle.normal.background = backgroundTexture;
    }

    private void DrawConsoleWindow()
    {
        float yPos = Screen.height * (1 - _console.WindowHeight);
        var windowRect = new Rect(0, yPos, Screen.width, Screen.height * _console.WindowHeight);

        GUILayout.BeginArea(windowRect, _backgroundStyle);
        DrawLogs();
        DrawInputField();
        DrawSuggestions();
        GUILayout.EndArea();
    }

    private void DrawLogs()
    {
        if (_console.ScrollToBottom)
        {
            _console.SetScrollPosition(new Vector2(_console.ScrollPosition.x, float.MaxValue));
        }

        var scrollPosition = GUILayout.BeginScrollView(_console.ScrollPosition);
        _console.SetScrollPosition(scrollPosition);

        foreach (var log in _console.Logs)
        {
            GUI.color = GetLogColor(log.Type);
            GUILayout.Label(string.Format(DebugConsoleUIConstants.TimeCommandFormat, log.Timestamp, log.Message), _logStyle);
        }

        GUI.color = Color.white;
        GUILayout.EndScrollView();

        _console.SetScrollToBottom(false);
    }

    private void DrawInputField()
    {
        GUILayout.BeginHorizontal();
        GUI.SetNextControlName(DebugConsoleUIConstants.CommandInputControlName);

        string previousInput = _console.InputCommand;
        string newInput = GUILayout.TextField(_console.InputCommand, _inputStyle);
        _console.SetInputCommand(newInput);

        GUI.FocusControl(DebugConsoleUIConstants.CommandInputControlName);

        if (_console.CursorMoveFrame == Time.frameCount)
        {
            var textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
            if (textEditor != null && textEditor.text == _console.InputCommand)
            {
                textEditor.cursorIndex = _console.InputCommand.Length;
                textEditor.selectIndex = _console.InputCommand.Length;
                textEditor.scrollOffset = Vector2.zero; // Сбрасываем прокрутку
            }
        }

        var currentEvent = Event.current;

        if (currentEvent.type == EventType.KeyDown || currentEvent.type == EventType.KeyUp || currentEvent.type == EventType.Used)
        {
            if (_console.ShowSuggestions && _console.Suggestions.Count > 0)
            {
                bool userNavigating =
                    currentEvent.keyCode == DebugConsoleInputConstants.LeftArrowKey ||
                    currentEvent.keyCode == DebugConsoleInputConstants.RightArrowKey;

                var textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                if (textEditor != null && textEditor.text == _console.InputCommand)
                {
                    if (userNavigating == false)
                    {
                        textEditor.cursorIndex = _console.InputCommand.Length;
                        textEditor.selectIndex = _console.InputCommand.Length;
                    }
                }
            }
        }

        if (previousInput != _console.InputCommand)
        {
            _console.UpdateSuggestions();
        }

        float width = DebugConsoleUIConstants.ButtonExtraWidth + _console.FontSize * DebugConsoleUIConstants.SubmitButtonText.Length;

        if (GUILayout.Button(DebugConsoleUIConstants.SubmitButtonText, _buttonStyle, GUILayout.Width(width)) ||
            (Event.current.keyCode == DebugConsoleInputConstants.DefaultSubmitKey && Event.current.type == EventType.Used))
        {
            _console.HandleSubmit();
            Event.current.Use();
        }

        GUILayout.EndHorizontal();
    }

    private void DrawSuggestions()
    {
        if (!_console.ShowSuggestions || _console.Suggestions.Count == 0) return;

        GUILayout.Space(DebugConsoleUIConstants.SuggestionSpacing);

        if (_console.ShowingParameters)
        {
            var parameterInfo = _console.GetCurrentParameterInfo();
            if (parameterInfo != null)
            {
                GUILayout.Label(string.Format(DebugConsoleMessageConstants.ParameterLabelFormat, parameterInfo.Name, DebugCommandSuggestions.GetParameterTypeDisplayName(parameterInfo.ParameterType)),
                    new GUIStyle(GUI.skin.label) { fontSize = _console.FontSize - DebugConsoleUIConstants.SuggestionLabelFontReduction, normal = { textColor = DebugConsoleColorConstants.ParameterInfoColor } });
            }
            else
            {
                GUILayout.Label(DebugConsoleMessageConstants.PossibleValuesLabel,
                    new GUIStyle(GUI.skin.label) { fontSize = _console.FontSize - DebugConsoleUIConstants.SuggestionLabelFontReduction, normal = { textColor = DebugConsoleColorConstants.ParameterInfoColor } });
            }
        }
        else
        {
            GUILayout.Label(DebugConsoleMessageConstants.SimilarCommandsLabel,
                new GUIStyle(GUI.skin.label) { fontSize = _console.FontSize - DebugConsoleUIConstants.SuggestionLabelFontReduction, normal = { textColor = DebugConsoleColorConstants.ParameterInfoColor } });
        }

        for (int i = 0; i < _console.Suggestions.Count; i++)
        {
            GUI.color = i == _console.SelectedSuggestionIndex ? DebugConsoleColorConstants.SelectedSuggestionColor : DebugConsoleColorConstants.SuggestionTextColor;

            if (GUILayout.Button($"{DebugConsoleMessageConstants.SuggestionButtonPrefix}{_console.Suggestions[i]}", _suggestionStyle))
            {
                _console.HandleSuggestionClick(i);
            }
        }

        GUI.color = Color.white;
    }

    private Color GetLogColor(LogType type)
    {
        return type switch
        {
            LogType.Error => DebugConsoleColorConstants.ErrorLogColor,
            LogType.Warning => DebugConsoleColorConstants.WarningLogColor,
            LogType.Log => DebugConsoleColorConstants.NormalLogColor,
            _ => DebugConsoleColorConstants.NormalLogColor
        };
    }
}
