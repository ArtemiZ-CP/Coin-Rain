using System;

/// <summary>
/// Attribute to mark methods as debug commands.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class DebugCommandAttribute : Attribute
{
    public string CommandName { get; }
    public string Description { get; }

    public DebugCommandAttribute(string commandName, string description = "")
    {
        CommandName = commandName;
        Description = description;
    }
}
