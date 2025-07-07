using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Provides in-game debug console functionality with command support and auto-suggestions.
/// 
/// Features:
/// - Command auto-completion with Tab key
/// - Similar command suggestions based on Levenshtein distance
/// - Navigation through suggestions using Up/Down arrow keys
/// - Real-time suggestions as you type
/// - Beautiful custom inspector with themes and presets
/// </summary>
public class DebugConsole : MonoBehaviour
{
    [SerializeField] private float _windowHeight = DebugConsoleUIConstants.DefaultWindowHeight;
    [SerializeField] private int _maxLogs = DebugConsoleUIConstants.DefaultMaxLogs;
    [SerializeField, Range(12, 48)] private int _fontSize = DebugConsoleUIConstants.DefaultFontSize;
    [SerializeField] private Color _backgroundColor = DebugConsoleUIConstants.DefaultBackgroundColor;
    [SerializeField] private Color _textColor = DebugConsoleUIConstants.DefaultTextColor;

    private static DebugConsole _instance;

    private readonly List<DebugLog> _logs = new();
    private readonly Dictionary<string, CommandInfo> _commands = new();

    private bool _isVisible;
    private bool _scrollToBottom;
    private string _inputCommand = string.Empty;
    private Vector2 _scrollPosition;

    private List<string> _suggestions = new();
    private int _selectedSuggestionIndex = -1;
    private bool _showSuggestions = false;
    
    // Новые поля для работы с параметрами
    private bool _showingParameters = false;
    private string _currentCommand = string.Empty;
    private int _currentParameterIndex = -1;
    private int _cursorMoveFrame = -1;

    private DebugConsoleGUI _gui;

    // Public properties для доступа из GUI
    public static DebugConsole Instance => _instance;
    public bool IsVisible => _isVisible;
    public List<DebugLog> Logs => _logs;
    public Dictionary<string, CommandInfo> Commands => _commands;
    public bool ScrollToBottom => _scrollToBottom;
    public string InputCommand => _inputCommand;
    public Vector2 ScrollPosition => _scrollPosition;
    public int FontSize => _fontSize;
    public Color BackgroundColor => _backgroundColor;
    public Color TextColor => _textColor;
    public float WindowHeight => _windowHeight;
    public List<string> Suggestions => _suggestions;
    public int SelectedSuggestionIndex => _selectedSuggestionIndex;
    public bool ShowSuggestions => _showSuggestions;
    public bool ShowingParameters => _showingParameters;
    public int CursorMoveFrame => _cursorMoveFrame;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        _gui = new DebugConsoleGUI(this);

        DebugCommandRegistry.RegisterCommandsInScene(_commands);
        Application.logMessageReceived += HandleLog;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
        _instance = null;
    }

    private void Start()
    {
        DefaultConsoleCommands.Clear();
        DefaultConsoleCommands.ShowHelp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(DebugConsoleInputConstants.DefaultToggleKey))
        {
            ToggleConsole();
        }
    }

    private void OnGUI()
    {
        _gui.OnGUI();
    }

    #region Public API

    /// <summary>
    /// Registers a new debug command programmatically.
    /// </summary>
    public static void RegisterStaticCommand(string commandName, string method, string description)
    {
        if (_instance == null)
        {
            Debug.LogError(DebugConsoleMessageConstants.InstanceNotFoundError);
            return;
        }

        RegisterCommand(commandName, typeof(DefaultConsoleCommands).GetMethod(method), null, description);
    }

    /// <summary>
    /// Registers a new debug command programmatically.
    /// </summary>
    public static void RegisterCommand(string commandName, MethodInfo method, object target, string description)
    {
        if (_instance == null)
        {
            Debug.LogError(DebugConsoleMessageConstants.InstanceNotFoundError);
            return;
        }

        DebugCommandRegistry.RegisterCommand(commandName, method, target, description, _instance._commands);
    }

    /// <summary>
    /// Unregisters a debug command.
    /// </summary>
    public static bool UnregisterCommand(string commandName)
    {
        if (_instance == null)
        {
            Debug.LogError(DebugConsoleMessageConstants.InstanceNotFoundError);
            return false;
        }

        return DebugCommandRegistry.UnregisterCommand(commandName, _instance._commands);
    }

    /// <summary>
    /// Gets all registered command names.
    /// </summary>
    public static string[] GetRegisteredCommands()
    {
        if (_instance == null)
        {
            Debug.LogError(DebugConsoleMessageConstants.InstanceNotFoundError);
            return new string[0];
        }

        return DebugCommandRegistry.GetRegisteredCommands(_instance._commands);
    }

    /// <summary>
    /// Clears all logs from the console.
    /// </summary>
    public void ClearLogs()
    {
        _logs.Clear();
    }

    /// <summary>
    /// Toggles console visibility.
    /// </summary>
    public void ToggleConsole()
    {
        _isVisible = !_isVisible;
    }

    #endregion

    #region GUI Interface Methods

    public void SetScrollPosition(Vector2 position)
    {
        _scrollPosition = position;
    }

    public void SetScrollToBottom(bool value)
    {
        _scrollToBottom = value;
    }

    public void SetInputCommand(string command)
    {
        _inputCommand = command;
    }

    public void HandleTabCompletion()
    {
        if (_selectedSuggestionIndex >= 0)
        {
            if (_showingParameters)
            {
                var parts = _inputCommand.Split(' ');
                if (parts.Length > 1)
                {
                    parts[parts.Length - 1] = _suggestions[_selectedSuggestionIndex];
                    _inputCommand = string.Join(" ", parts) + " ";
                }
                else
                {
                    _inputCommand += _suggestions[_selectedSuggestionIndex] + " ";
                }
            }
            else
            {
                _inputCommand = _suggestions[_selectedSuggestionIndex] + " ";
            }
            _showSuggestions = false;
            _selectedSuggestionIndex = -1;
            _showingParameters = false;
            MoveCursorToEnd();
            UpdateSuggestions();
        }
    }

    public void MoveSuggestionSelection(int direction)
    {
        if (direction < 0)
        {
            _selectedSuggestionIndex = Mathf.Max(0, _selectedSuggestionIndex - 1);
        }
        else
        {
            _selectedSuggestionIndex = Mathf.Min(_suggestions.Count - 1, _selectedSuggestionIndex + 1);
        }
    }

    public void HandleSubmit()
    {
        if (!string.IsNullOrEmpty(_inputCommand))
        {
            DebugCommandExecutor.ExecuteCommand(_inputCommand, _commands);
            _inputCommand = string.Empty;
            _showSuggestions = false;
            _selectedSuggestionIndex = -1;
            _showingParameters = false;
            _currentCommand = string.Empty;
            _currentParameterIndex = -1;
        }
    }

    public void HandleSuggestionClick(int index)
    {
        if (_showingParameters)
        {
            var parts = _inputCommand.Split(' ');
            if (parts.Length > 1)
            {
                parts[^1] = _suggestions[index];
                _inputCommand = string.Join(" ", parts) + " ";
            }
            else
            {
                _inputCommand += _suggestions[index] + " ";
            }
        }
        else
        {
            _inputCommand = _suggestions[index] + " ";
        }
        _showSuggestions = false;
        _selectedSuggestionIndex = -1;
        _showingParameters = false;
        
        MoveCursorToEnd();
        UpdateSuggestions();
    }

    public void UpdateSuggestions()
    {
        _suggestions.Clear();
        _selectedSuggestionIndex = -1;
        _showSuggestions = false;
        _showingParameters = false;

        if (string.IsNullOrWhiteSpace(_inputCommand))
        {
            return;
        }

        var input = _inputCommand.Trim();
        var parts = input.Split(' ');
        
        if (parts.Length == 1 && !_inputCommand.EndsWith(" "))
        {
            _suggestions = DebugCommandSuggestions.GetCommandSuggestions(parts[0].ToLower(), _commands);
            
            if (_suggestions.Count > 0)
            {
                _showSuggestions = true;
                _selectedSuggestionIndex = 0;
            }
        }
        else if (parts.Length >= 1 && (_inputCommand.EndsWith(" ") || parts.Length > 1))
        {
            var commandName = FindCommandKeyByOriginalName(parts[0]);
            if (_commands.ContainsKey(commandName))
            {
                _currentCommand = commandName;
                
                if (_inputCommand.EndsWith(" "))
                {
                    _currentParameterIndex = parts.Length - 1;
                }
                else
                {
                    _currentParameterIndex = parts.Length - 2;
                }
                
                _suggestions = DebugCommandSuggestions.GetParameterSuggestions(
                    commandName, parts, _commands, _inputCommand.EndsWith(" "));
                
                if (_suggestions.Count > 0)
                {
                    _showSuggestions = true;
                    _showingParameters = true;
                    _selectedSuggestionIndex = 0;
                }
            }
        }
    }

    public ParameterInfo GetCurrentParameterInfo()
    {
        return DebugCommandSuggestions.GetParameterInfo(_currentCommand, _currentParameterIndex, _commands);
    }

    #endregion

    private void HandleLog(string message, string stackTrace, LogType type)
    {
        _logs.Add(new DebugLog(message, type));

        if (_logs.Count > _maxLogs)
        {
            _logs.RemoveAt(0);
        }

        _scrollToBottom = true;
    }

    /// <summary>
    /// Перемещает курсор в конец строки ввода команды
    /// </summary>
    private void MoveCursorToEnd()
    {
        _cursorMoveFrame = Time.frameCount;
    }

    /// <summary>
    /// Находит команду по оригинальному имени (для автодополнения)
    /// </summary>
    private string FindCommandKeyByOriginalName(string originalName)
    {
        foreach (var kvp in _commands)
        {
            if (kvp.Value.OriginalName != null && kvp.Value.OriginalName.Equals(originalName, System.StringComparison.OrdinalIgnoreCase))
            {
                return kvp.Key;
            }
            if (kvp.Value.OriginalName == null && kvp.Key.Equals(originalName, System.StringComparison.OrdinalIgnoreCase))
            {
                return kvp.Key;
            }
        }
        return originalName.ToLower();
    }
}
