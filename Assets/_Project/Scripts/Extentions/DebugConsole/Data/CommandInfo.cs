using System.Reflection;

/// <summary>
/// Contains information about a registered debug command.
/// </summary>
public class CommandInfo
{
    public MethodInfo Method { get; }
    public object Target { get; }
    public string Description { get; }
    public string OriginalName { get; }

    public CommandInfo(MethodInfo method, object target, string description, string originalName = null)
    {
        Method = method;
        Target = target;
        Description = description;
        OriginalName = originalName;
    }
}
