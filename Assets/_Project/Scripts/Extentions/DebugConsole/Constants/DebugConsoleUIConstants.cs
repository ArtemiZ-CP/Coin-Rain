using UnityEngine;

/// <summary>
/// Contains all UI-related constants for the Debug Console system.
/// Defines appearance, layout, and visual configuration parameters.
/// </summary>
public static class DebugConsoleUIConstants
{
    #region Default Settings
    
    /// <summary>
    /// Default console window height as percentage of screen height
    /// </summary>
    public const float DefaultWindowHeight = 0.4f;
    
    /// <summary>
    /// Default maximum number of log entries to keep
    /// </summary>
    public const int DefaultMaxLogs = 100;
    
    /// <summary>
    /// Default font size for console text
    /// </summary>
    public const int DefaultFontSize = 32;
    
    /// <summary>
    /// Default background color for console window
    /// </summary>
    public static readonly Color DefaultBackgroundColor = new(0.1f, 0.1f, 0.1f, 0.9f);
    
    /// <summary>
    /// Default text color for console text
    /// </summary>
    public static readonly Color DefaultTextColor = new(0.9f, 0.9f, 0.9f, 1f);
    
    #endregion
    
    #region GUI Constants
    
    /// <summary>
    /// Control name for command input field
    /// </summary>
    public const string CommandInputControlName = "CommandInput";
    
    /// <summary>
    /// Text displayed on submit button
    /// </summary>
    public const string SubmitButtonText = "Submit";
    
    /// <summary>
    /// Format string for displaying log timestamps
    /// </summary>
    public const string TimeCommandFormat = "[{0:HH:mm:ss}] {1}";
    
    /// <summary>
    /// Extra width added to buttons beyond text width
    /// </summary>
    public const float ButtonExtraWidth = 30f;
    
    /// <summary>
    /// Font size reduction for suggestion labels
    /// </summary>
    public const int SuggestionLabelFontReduction = 6;
    
    /// <summary>
    /// Font size reduction for suggestion buttons
    /// </summary>
    public const int SuggestionButtonFontReduction = 4;
    
    /// <summary>
    /// Height multiplier for input field based on font size
    /// </summary>
    public const float InputFieldHeightMultiplier = 1.5f;
    
    /// <summary>
    /// Height multiplier for suggestion buttons based on font size
    /// </summary>
    public const float SuggestionButtonHeightMultiplier = 1.2f;
    
    /// <summary>
    /// Spacing between suggestions section and input
    /// </summary>
    public const float SuggestionSpacing = 5f;
    
    #endregion
}
