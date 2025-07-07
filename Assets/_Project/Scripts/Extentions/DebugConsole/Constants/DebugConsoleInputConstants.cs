using UnityEngine;

/// <summary>
/// Contains all input-related constants for the Debug Console system.
/// Defines key bindings and input handling parameters.
/// </summary>
public static class DebugConsoleInputConstants
{
    #region Input Keys
    
    /// <summary>
    /// Default key to toggle console visibility
    /// </summary>
    public const KeyCode DefaultToggleKey = KeyCode.Escape;
    
    /// <summary>
    /// Default key to submit commands
    /// </summary>
    public const KeyCode DefaultSubmitKey = KeyCode.Return;
    
    /// <summary>
    /// Key used for auto-completion of suggestions
    /// </summary>
    public const KeyCode TabKey = KeyCode.Tab;
    
    /// <summary>
    /// Key used to navigate up in suggestions list
    /// </summary>
    public const KeyCode UpArrowKey = KeyCode.UpArrow;
    
    /// <summary>
    /// Key used to navigate down in suggestions list
    /// </summary>
    public const KeyCode DownArrowKey = KeyCode.DownArrow;
    
    /// <summary>
    /// Key used to navigate left (cursor movement)
    /// </summary>
    public const KeyCode LeftArrowKey = KeyCode.LeftArrow;
    
    /// <summary>
    /// Key used to navigate right (cursor movement)
    /// </summary>
    public const KeyCode RightArrowKey = KeyCode.RightArrow;

    #endregion
}
