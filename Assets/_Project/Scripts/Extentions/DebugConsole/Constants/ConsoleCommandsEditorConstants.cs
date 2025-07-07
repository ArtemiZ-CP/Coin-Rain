/// <summary>
/// Constants for ConsoleCommandsEditor UI layout and styling
/// </summary>
public static class ConsoleCommandsEditorConstants
{
    // UI Layout Constants
    public const float BASE_COMMAND_HEIGHT = 24f;

    // Button Dimensions
    public const float TOOLBAR_BUTTON_HEIGHT = 18f;
    public const float EXECUTE_BUTTON_WIDTH = 40f;
    public const float PARAMETER_COUNT_WIDTH = 30f;
    public const float PARAMETER_LABEL_WIDTH = 70f;
    
    // Spacing
    public const float MAIN_SECTION_SPACING = 20f;
    public const float COMMANDS_SECTION_SPACING = 5f;
    public const float TOOLBAR_SPACING = 3f;
    public const float COMMAND_ITEM_SPACING = 2f;
    
    // Font Sizes
    public const int HEADER_FONT_SIZE = 11;
    public const int INFO_FONT_SIZE = 10;
    public const int DESCRIPTION_FONT_SIZE = 10;
    public const int BUTTON_FONT_SIZE = 10;
    
    // Padding and Margins
    public static readonly UnityEngine.RectOffset BOX_PADDING = new UnityEngine.RectOffset(5, 5, 2, 2);
    public static readonly UnityEngine.RectOffset BOX_MARGIN = new UnityEngine.RectOffset(0, 0, 1, 1);
    public static readonly UnityEngine.RectOffset INFO_PADDING = new UnityEngine.RectOffset(5, 5, 3, 3);
    public static readonly UnityEngine.RectOffset BUTTON_PADDING = new UnityEngine.RectOffset(8, 8, 2, 2);
    
    // Log Messages
    public const string LOG_INITIALIZING_PARAMS = "🔧 Initializing parameters for {0}";
    public const string LOG_REINITIALIZING_PARAMS = "🔧 (Re)initializing parameter array for {0}: {1} parameters";
    public const string LOG_FIXING_PARAM_ARRAY = "🔧 Attempting to fix parameter array for {0}";
    public const string LOG_FIXED_NULL_PARAM = "⚠️ Fixed null parameter {0} ({1}) for command {2}";
    public const string LOG_FIXED_NULL_VALUE = "🔧 Fixed null value for parameter {0}";
    public const string LOG_RESET_PARAMETERS = "🔄 All command parameters have been reset to default values";
    
    // Error Messages
    public const string ERROR_PARAM_COUNT_MISMATCH = "❌ Parameter count mismatch for command {0}: {1}";
    public const string ERROR_PARAM_ARRAY_MISMATCH = "❌ Parameter array length mismatch for command {0}: expected {1}, got {2}";
    public const string ERROR_CRITICAL_SIZE_MISMATCH = "❌ Critical error: Parameter array size mismatch for {0}";
    public const string ERROR_INVALID_PARAMETER = "❌ Invalid parameter for command {0}: {1}";
    public const string ERROR_EXECUTION_ERROR = "❌ Error executing command {0}: {1}";
    public const string ERROR_UNEXPECTED = "❌ Unexpected error executing command {0}: {1}";
    
    // UI Text
    public const string UI_DEBUG_COMMANDS = "Debug Commands";
    public const string UI_COMMANDS_COUNT = "Commands: {0}";
    public const string UI_RESET_BUTTON = "🔄 Reset";
    public const string UI_EXECUTE_ALL_BUTTON = "⚡ Execute All";
    public const string UI_EXECUTE_BUTTON = "▶";
    public const string UI_PARAMETER_COUNT = "({0})";
    public const string UI_TYPE_TOOLTIP = "Type: {0}";
    public const string UI_PARAMETER_LABEL = "{0}:";
    public const string UI_NO_ENUM_VALUES = "No values in enum {0}";
    public const string UI_UNSUPPORTED_TYPE = "Unsupported type: {0}";
    
    // Dialog Messages
    public const string DIALOG_EXECUTE_ALL_TITLE = "Execute All Commands";
    public const string DIALOG_EXECUTE_ALL_MESSAGE = "Are you sure you want to execute all {0} commands?";
    public const string DIALOG_YES = "Yes";
    public const string DIALOG_CANCEL = "Cancel";
    
    // Progress Bar
    public const string PROGRESS_EXECUTING_COMMANDS = "Executing Commands";
    public const string PROGRESS_RUNNING_COMMANDS = "Running all debug commands...";
    public const string PROGRESS_EXECUTING_COMMAND = "Executing {0}...";
    public const int PROGRESS_DELAY_MS = 100;
}
