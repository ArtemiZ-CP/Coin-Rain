/// <summary>
/// Contains all message and text constants for the Debug Console system.
/// Defines error messages, format strings, and localized text content.
/// </summary>
public static class DebugConsoleMessageConstants
{
    #region Error Messages
    
    /// <summary>
    /// Error message when DebugConsole instance is not found
    /// </summary>
    public const string InstanceNotFoundError = "DebugConsole instance not found!";
    
    /// <summary>
    /// Format string for command execution confirmation
    /// </summary>
    public const string CommandExecutedFormat = "Command executed: {0}";
    
    /// <summary>
    /// Format string for unknown command error
    /// </summary>
    public const string UnknownCommandFormat = "Unknown command: {0}";
    
    /// <summary>
    /// Format string for command execution error
    /// </summary>
    public const string ErrorExecutingCommandFormat = "Error executing command: {0}";
    
    /// <summary>
    /// Warning message for duplicate command registration
    /// </summary>
    public const string DuplicateCommandWarningFormat = "Command '{0}' from {1} is already registered. Skipping.";
    
    /// <summary>
    /// Warning message for programmatic duplicate command registration
    /// </summary>
    public const string ProgrammaticDuplicateCommandWarningFormat = "Command '{0}' is already registered. Skipping registration.";
    
    /// <summary>
    /// Success message for programmatic command registration
    /// </summary>
    public const string CommandRegisteredFormat = "Command '{0}' registered programmatically.";
    
    /// <summary>
    /// Success message for command unregistration
    /// </summary>
    public const string CommandUnregisteredFormat = "Command '{0}' unregistered.";
    
    /// <summary>
    /// Warning message when trying to unregister non-existent command
    /// </summary>
    public const string CommandNotFoundForUnregistrationFormat = "Command '{0}' not found for unregistration.";
    
    /// <summary>
    /// Error message for invalid enum values
    /// </summary>
    public const string InvalidEnumValueFormat = "'{0}' is not a valid value for enum {1}. Valid values: {2}";
    
    #endregion
    
    #region Default Commands Messages
    
    /// <summary>
    /// Header text for help command output
    /// </summary>
    public const string CommandsHeader = "Available commands:\n";
    
    /// <summary>
    /// Format string for displaying current FPS
    /// </summary>
    public const string CurrentFpsFormat = "Current FPS: {0:F1}";
    
    /// <summary>
    /// Message displayed when quitting application
    /// </summary>
    public const string QuittingApplicationMessage = "Quitting application...";
    
    #endregion
    
    #region Suggestion Messages
    
    /// <summary>
    /// Header text for similar commands suggestions
    /// </summary>
    public const string SimilarCommandsHeader = "Возможно, вы имели в виду:\n";
    
    /// <summary>
    /// Bullet point prefix for command suggestions
    /// </summary>
    public const string CommandSuggestionPrefix = "  • ";
    
    /// <summary>
    /// Label text for parameter suggestions
    /// </summary>
    public const string ParameterLabelFormat = "Параметр: {0} ({1})";
    
    /// <summary>
    /// Label text for generic parameter values
    /// </summary>
    public const string PossibleValuesLabel = "Возможные значения:";
    
    /// <summary>
    /// Label text for similar commands
    /// </summary>
    public const string SimilarCommandsLabel = "Похожие команды:";
    
    /// <summary>
    /// Prefix for suggestion buttons
    /// </summary>
    public const string SuggestionButtonPrefix = "  ";
    
    #endregion
}
