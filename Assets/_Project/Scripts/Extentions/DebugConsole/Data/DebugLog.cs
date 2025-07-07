using System;
using UnityEngine;

/// <summary>
/// Represents a single debug log entry.
/// </summary>
public class DebugLog
{
    public string Message { get; }
    public LogType Type { get; }
    public DateTime Timestamp { get; }

    public DebugLog(string message, LogType type)
    {
        Message = message;
        Type = type;
        Timestamp = DateTime.Now;
    }
}
