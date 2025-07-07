/// <summary>
/// Contains all command execution-related constants for the Debug Console system.
/// Defines parameters for command processing, suggestions, and execution behavior.
/// </summary>
public static class DebugConsoleCommandConstants
{
    #region Command Execution
    
    /// <summary>
    /// Character used to separate command arguments
    /// </summary>
    public const char CommandSeparator = ' ';
    
    /// <summary>
    /// Maximum number of suggestions to show
    /// </summary>
    public const int MaxSuggestions = 5;
    
    /// <summary>
    /// Maximum number of similar commands to show in error messages
    /// </summary>
    public const int MaxSimilarCommandsToShow = 3;
    
    /// <summary>
    /// Minimum input length required for suggestions
    /// </summary>
    public const int MinInputLengthForSuggestions = 1;
    
    /// <summary>
    /// Minimum distance allowed for similar commands
    /// </summary>
    public const int MinSimilarCommandDistance = 2;
    
    /// <summary>
    /// Distance divisor for calculating maximum allowed distance based on input length
    /// </summary>
    public const int SimilarCommandDistanceDivisor = 2;
    
    #endregion
    
    #region Parameter Type Display Names
    
    /// <summary>
    /// Display format for enum parameter types
    /// </summary>
    public const string EnumParameterFormat = "enum: {0}";
    
    /// <summary>
    /// Display text for boolean parameter types
    /// </summary>
    public const string BoolParameterDisplay = "bool: true|false";
    
    /// <summary>
    /// Display text for integer parameter types
    /// </summary>
    public const string IntParameterDisplay = "int";
    
    /// <summary>
    /// Display text for float parameter types
    /// </summary>
    public const string FloatParameterDisplay = "float";
    
    /// <summary>
    /// Display text for string parameter types
    /// </summary>
    public const string StringParameterDisplay = "string";
    
    /// <summary>
    /// Format for unnamed string parameters
    /// </summary>
    public const string UnnamedParameterFormat = "<{0}>";
    
    /// <summary>
    /// Separator for enum values display
    /// </summary>
    public const string EnumValueSeparator = "|";
    
    /// <summary>
    /// Separator for enum values in error messages
    /// </summary>
    public const string EnumValueListSeparator = ", ";
    
    #endregion
    
    #region Numeric Examples
    
    /// <summary>
    /// Example values for integer parameters
    /// </summary>
    public static readonly string[] IntegerExamples = { "0", "1", "10", "100" };
    
    /// <summary>
    /// Example values for floating-point parameters
    /// </summary>
    public static readonly string[] FloatExamples = { "0.0", "1.0", "0.5", "10.5" };
    
    /// <summary>
    /// Default numeric example
    /// </summary>
    public static readonly string[] DefaultNumericExample = { "0" };
    
    /// <summary>
    /// Boolean value options
    /// </summary>
    public static readonly string[] BooleanValues = { "true", "false" };
    
    #endregion
    
    #region Default Command Descriptions
    
    /// <summary>
    /// Description for the help command
    /// </summary>
    public const string HelpCommandDescription = "Shows available commands";
    
    /// <summary>
    /// Description for the clear command
    /// </summary>
    public const string ClearCommandDescription = "Clears the console";
    
    /// <summary>
    /// Description for the fps command
    /// </summary>
    public const string FpsCommandDescription = "Shows current FPS";
    
    /// <summary>
    /// Description for the quit command
    /// </summary>
    public const string QuitCommandDescription = "Quits the application";
    
    #endregion
    
    #region Reflection Binding Flags
    
    /// <summary>
    /// Binding flags used for finding debug command methods via reflection
    /// </summary>
    public const System.Reflection.BindingFlags CommandMethodBindingFlags = 
        System.Reflection.BindingFlags.Instance | 
        System.Reflection.BindingFlags.Static | 
        System.Reflection.BindingFlags.Public | 
        System.Reflection.BindingFlags.NonPublic;
    
    #endregion
}
