using UnityEngine;

/// <summary>
/// Contains all color-related constants for the Debug Console system.
/// Defines color schemes, themes, and visual styling parameters.
/// </summary>
public static class DebugConsoleColorConstants
{
    #region Colors
    
    /// <summary>
    /// Color for error log messages
    /// </summary>
    public static readonly Color ErrorLogColor = Color.red;
    
    /// <summary>
    /// Color for warning log messages
    /// </summary>
    public static readonly Color WarningLogColor = Color.yellow;
    
    /// <summary>
    /// Color for normal log messages
    /// </summary>
    public static readonly Color NormalLogColor = Color.white;
    
    /// <summary>
    /// Color for suggestion text
    /// </summary>
    public static readonly Color SuggestionTextColor = Color.cyan;
    
    /// <summary>
    /// Color for selected suggestion
    /// </summary>
    public static readonly Color SelectedSuggestionColor = Color.yellow;
    
    /// <summary>
    /// Color for parameter info labels
    /// </summary>
    public static readonly Color ParameterInfoColor = Color.yellow;
    
    #endregion
    
    #region Theme Color Schemes
    
    /// <summary>
    /// Predefined color themes for the debug console
    /// </summary>
    public static class ThemeColors
    {
        public static readonly (Color background, Color text) Dark = 
            (new Color(0.1f, 0.1f, 0.1f, 0.9f), new Color(0.9f, 0.9f, 0.9f, 1f));
            
        public static readonly (Color background, Color text) Light = 
            (new Color(0.95f, 0.95f, 0.95f, 0.9f), new Color(0.1f, 0.1f, 0.1f, 1f));
            
        public static readonly (Color background, Color text) Matrix = 
            (new Color(0.05f, 0.15f, 0.05f, 0.95f), new Color(0.2f, 0.8f, 0.2f, 1f));
            
        public static readonly (Color background, Color text) Alert = 
            (new Color(0.3f, 0.1f, 0.1f, 0.9f), new Color(1f, 0.6f, 0.6f, 1f));
            
        public static readonly (Color background, Color text) Retro = 
            (new Color(0.2f, 0.15f, 0.05f, 0.9f), new Color(0.9f, 0.8f, 0.5f, 1f));
            
        public static readonly (Color background, Color text) Performance = 
            (new Color(0.1f, 0.1f, 0.1f, 0.8f), new Color(0.5f, 0.9f, 1f, 1f));
            
        public static readonly (Color background, Color text) Ocean = 
            (new Color(0.05f, 0.1f, 0.2f, 0.9f), new Color(0.6f, 0.8f, 1f, 1f));
            
        public static readonly (Color background, Color text) Sakura = 
            (new Color(0.2f, 0.1f, 0.15f, 0.9f), new Color(1f, 0.7f, 0.8f, 1f));
            
        public static readonly (Color background, Color text) Nature = 
            (new Color(0.1f, 0.15f, 0.05f, 0.9f), new Color(0.7f, 0.9f, 0.6f, 1f));
    }

    #endregion
}
