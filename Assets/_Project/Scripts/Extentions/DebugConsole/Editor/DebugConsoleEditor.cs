using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(DebugConsole))]
public class DebugConsoleEditor : Editor
{
    private SerializedProperty _windowHeight;
    private SerializedProperty _maxLogs;
    private SerializedProperty _fontSize;
    private SerializedProperty _backgroundColor;
    private SerializedProperty _textColor;

    private bool _showAppearanceSettings = true;
    private bool _showBehaviorSettings = true;
    private bool _showQuickActions = true;
    private bool _showThemes = true;

    private GUIStyle _headerStyle;
    private GUIStyle _boxStyle;
    private GUIStyle _buttonStyle;
    private GUIStyle _foldoutStyle;
    private GUIStyle _warningStyle;
    private GUIStyle _tipStyle;
    private bool _stylesInitialized = false;

    // Переменные для анимации
    private double _lastUpdateTime;
    private float _pulseAnimation;

    private void OnEnable()
    {
        _windowHeight = serializedObject.FindProperty("_windowHeight");
        _maxLogs = serializedObject.FindProperty("_maxLogs");
        _fontSize = serializedObject.FindProperty("_fontSize");
        _backgroundColor = serializedObject.FindProperty("_backgroundColor");
        _textColor = serializedObject.FindProperty("_textColor");
    }

    private void InitializeStyles()
    {
        if (_stylesInitialized) return;

        _headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14,
            normal = { textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black }
        };

        _boxStyle = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(10, 10, 10, 10),
            margin = new RectOffset(5, 5, 5, 5)
        };

        _buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 11,
            fontStyle = FontStyle.Bold
        };

        _foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold,
            normal = { textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black },
            onNormal = { textColor = Color.yellow }
        };

        _warningStyle = new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = Color.red },
            fontStyle = FontStyle.Bold
        };

        _tipStyle = new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = Color.green },
            fontStyle = FontStyle.Bold
        };

        _stylesInitialized = true;
    }

    public override void OnInspectorGUI()
    {
        InitializeStyles();
        serializedObject.Update();

        // Обновляем анимацию
        UpdateAnimation();

        var console = (DebugConsole)target;

        DrawConsoleHeader();
        DrawConsoleStatus(console);
        
        EditorGUILayout.Space(10);
        
        DrawQuickActions(console);
        DrawThemes();
        DrawAppearanceSettings();
        DrawBehaviorSettings();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawConsoleHeader()
    {
        EditorGUILayout.BeginVertical(_boxStyle);
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("🎯", GUILayout.Width(30));
        GUILayout.Label("DEBUG CONSOLE", _headerStyle);
        GUILayout.FlexibleSpace();
        
        // Индикатор состояния с анимацией
        var console = (DebugConsole)target;
        bool isActive = console != null && console.IsVisible;
        
        if (isActive)
        {
            // Анимированный индикатор для активной консоли
            var pulseColor = Color.Lerp(Color.green, Color.white, _pulseAnimation * 0.3f);
            var oldColor = GUI.color;
            GUI.color = pulseColor;
            GUILayout.Label("🟢 ACTIVE", EditorStyles.miniLabel);
            GUI.color = oldColor;
        }
        else
        {
            var oldColor = GUI.color;
            GUI.color = Color.gray;
            GUILayout.Label("⚪ INACTIVE", EditorStyles.miniLabel);
            GUI.color = oldColor;
        }
        
        EditorGUILayout.EndHorizontal();
        
        // Описание
        EditorGUILayout.LabelField("In-game debug console with command support and auto-suggestions", EditorStyles.helpBox);
        
        EditorGUILayout.EndVertical();
    }

    private void DrawConsoleStatus(DebugConsole console)
    {
        if (!Application.isPlaying || console == null) return;

        EditorGUILayout.BeginVertical(_boxStyle);
        
        var oldColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.8f, 0.9f, 1f, 0.3f);
        
        EditorGUILayout.LabelField("📊 RUNTIME STATUS", _headerStyle);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"📝 Logs Count: {console.Logs.Count}", EditorStyles.miniLabel);
        EditorGUILayout.LabelField($"⚡ Commands: {console.Commands.Count}", EditorStyles.miniLabel);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"👁️ Visible: {(console.IsVisible ? "Yes" : "No")}", EditorStyles.miniLabel);
        EditorGUILayout.LabelField($"💡 Suggestions: {(console.ShowSuggestions ? "Yes" : "No")}", EditorStyles.miniLabel);
        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = oldColor;
        EditorGUILayout.EndVertical();
    }

    private void DrawQuickActions(DebugConsole console)
    {
        _showQuickActions = EditorGUILayout.Foldout(_showQuickActions, "⚡ Quick Actions", true, _foldoutStyle);
        
        if (!_showQuickActions) return;

        EditorGUILayout.BeginVertical(_boxStyle);
        
        EditorGUILayout.BeginHorizontal();
        
        // Кнопка переключения консоли
        var oldColor = GUI.backgroundColor;
        GUI.backgroundColor = Application.isPlaying && console != null && console.IsVisible ? 
            new Color(1f, 0.5f, 0.5f) : new Color(0.5f, 1f, 0.5f);
        
        if (GUILayout.Button(Application.isPlaying && console != null && console.IsVisible ? 
            "🙈 Hide Console" : "👁️ Show Console", _buttonStyle))
        {
            if (Application.isPlaying && console != null)
            {
                console.ToggleConsole();
                Repaint();
            }
        }
        
        // Кнопка очистки логов
        GUI.backgroundColor = new Color(1f, 0.8f, 0.5f);
        if (GUILayout.Button("🗑️ Clear Logs", _buttonStyle))
        {
            if (Application.isPlaying && console != null)
            {
                console.ClearLogs();
            }
        }
        
        GUI.backgroundColor = oldColor;
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        
        // Кнопка создания тестовых логов
        if (GUILayout.Button("📝 Test Logs", _buttonStyle))
        {
            Debug.Log("🎯 Test Log Message");
            Debug.LogWarning("⚠️ Test Warning Message");
            Debug.LogError("❌ Test Error Message");
        }
        
        // Кнопка показа команд
        if (GUILayout.Button("📋 Show Commands", _buttonStyle))
        {
            if (Application.isPlaying && console != null)
            {
                var commands = console.Commands.Keys.ToArray();
                var commandList = string.Join(", ", commands);
                Debug.Log($"📋 Available Commands ({commands.Length}): {commandList}");
            }
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
    }

    private void DrawAppearanceSettings()
    {
        _showAppearanceSettings = EditorGUILayout.Foldout(_showAppearanceSettings, "🎨 Appearance Settings", true, _foldoutStyle);
        
        if (!_showAppearanceSettings) return;

        EditorGUILayout.BeginVertical(_boxStyle);
        
        // Размер окна
        EditorGUILayout.LabelField("📐 Window Size", EditorStyles.boldLabel);
        _windowHeight.floatValue = EditorGUILayout.Slider("Height", _windowHeight.floatValue, 0.1f, 1f);
        
        EditorGUILayout.Space(5);
        
        // Размер шрифта с превью
        EditorGUILayout.LabelField("🔤 Font Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        _fontSize.intValue = EditorGUILayout.IntSlider("Font Size", _fontSize.intValue, 8, 48);
        
        var previewStyle = new GUIStyle(EditorStyles.label) { fontSize = _fontSize.intValue };
        GUILayout.Label("Abc", previewStyle, GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // Цвет фона с превью
        EditorGUILayout.LabelField("🎨 Colors", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        _backgroundColor.colorValue = EditorGUILayout.ColorField("Background Color", _backgroundColor.colorValue);
        
        // Превью цвета фона
        var bgRect = GUILayoutUtility.GetRect(30, 18);
        EditorGUI.DrawRect(bgRect, _backgroundColor.colorValue);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        _textColor.colorValue = EditorGUILayout.ColorField("Text Color", _textColor.colorValue);
        
        // Превью цвета текста
        var textRect = GUILayoutUtility.GetRect(30, 18);
        EditorGUI.DrawRect(textRect, _textColor.colorValue);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
    }

    private void DrawBehaviorSettings()
    {
        _showBehaviorSettings = EditorGUILayout.Foldout(_showBehaviorSettings, "⚙️ Behavior Settings", true, _foldoutStyle);
        
        if (!_showBehaviorSettings) return;

        EditorGUILayout.BeginVertical(_boxStyle);
        
        // Максимальное количество логов
        EditorGUILayout.LabelField("📊 Log Management", EditorStyles.boldLabel);
        _maxLogs.intValue = EditorGUILayout.IntSlider("Max Logs", _maxLogs.intValue, 50, 1000);
        
        // Информация о производительности
        EditorGUILayout.HelpBox(
            $"💡 Performance Tip: {_maxLogs.intValue} logs will use approximately " +
            $"{(_maxLogs.intValue * 0.1f):F1} KB of memory.", 
            MessageType.Info);
        
        EditorGUILayout.Space(5);
        
        // Горячие клавиши
        EditorGUILayout.LabelField("⌨️ Hotkeys", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Toggle Console:", EditorStyles.miniLabel);
        EditorGUILayout.LabelField("Backspace", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Auto-complete:", EditorStyles.miniLabel);
        EditorGUILayout.LabelField("Tab", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Navigate suggestions:", EditorStyles.miniLabel);
        EditorGUILayout.LabelField("↑↓ Arrow Keys", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
    }

    private void DrawThemes()
    {
        _showThemes = EditorGUILayout.Foldout(_showThemes, "🎨 Themes", true, _foldoutStyle);
        
        if (!_showThemes) return;

        EditorGUILayout.BeginVertical(_boxStyle);
        
        // Показываем текущую тему
        EditorGUILayout.LabelField("🎨 Current Theme Preview", EditorStyles.boldLabel);
        var previewRect = GUILayoutUtility.GetRect(200, 40);
        EditorGUI.DrawRect(previewRect, _backgroundColor.colorValue);
        
        var oldColor = GUI.color;
        GUI.color = _textColor.colorValue;
        var textStyle = new GUIStyle(EditorStyles.label) 
        { 
            fontSize = 12, 
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = _textColor.colorValue }
        };
        GUI.Label(previewRect, "Sample Debug Text", textStyle);
        GUI.color = oldColor;
        
        EditorGUILayout.Space(10);
        
        EditorGUILayout.LabelField("🌈 Quick Themes", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("🌙 Dark", "Classic dark theme with light text"), _buttonStyle))
        {
            ApplyTheme(DebugConsoleColorConstants.ThemeColors.Dark);
        }
        if (GUILayout.Button(new GUIContent("☀️ Light", "Clean light theme with dark text"), _buttonStyle))
        {
            ApplyTheme(DebugConsoleColorConstants.ThemeColors.Light);
        }
        if (GUILayout.Button(new GUIContent("🔋 Matrix", "Green matrix-style hacker theme"), _buttonStyle))
        {
            ApplyTheme(DebugConsoleColorConstants.ThemeColors.Matrix);
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("🔴 Alert", "Red alert theme for critical debugging"), _buttonStyle))
        {
            ApplyTheme(DebugConsoleColorConstants.ThemeColors.Alert);
        }
        if (GUILayout.Button(new GUIContent("📜 Retro", "Vintage amber terminal theme"), _buttonStyle))
        {
            ApplyTheme(DebugConsoleColorConstants.ThemeColors.Retro);
        }
        if (GUILayout.Button(new GUIContent("⚡ Performance", "High-contrast performance monitoring theme"), _buttonStyle))
        {
            ApplyTheme(DebugConsoleColorConstants.ThemeColors.Performance);
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("🌊 Ocean", "Calm blue ocean depths theme"), _buttonStyle))
        {
            ApplyTheme(DebugConsoleColorConstants.ThemeColors.Ocean);
        }
        if (GUILayout.Button(new GUIContent("🌸 Sakura", "Soft pink cherry blossom theme"), _buttonStyle))
        {
            ApplyTheme(DebugConsoleColorConstants.ThemeColors.Sakura);
        }
        if (GUILayout.Button(new GUIContent("🍃 Nature", "Fresh green nature theme"), _buttonStyle))
        {
            ApplyTheme(DebugConsoleColorConstants.ThemeColors.Nature);
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
    }

    private void ApplyTheme((Color backgroundColor, Color textColor) theme)
    {
        _backgroundColor.colorValue = theme.backgroundColor;
        _textColor.colorValue = theme.textColor;

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateAnimation()
    {
        double currentTime = EditorApplication.timeSinceStartup;
        if (currentTime - _lastUpdateTime > 0.1) // Обновляем каждые 100ms
        {
            _pulseAnimation = Mathf.Sin((float)currentTime * 3f) * 0.5f + 0.5f; // Пульсация от 0 до 1
            _lastUpdateTime = currentTime;
            
            // Перерисовываем только если консоль активна
            var console = (DebugConsole)target;
            if (Application.isPlaying && console != null && console.IsVisible)
            {
                Repaint();
            }
        }
    }
}
