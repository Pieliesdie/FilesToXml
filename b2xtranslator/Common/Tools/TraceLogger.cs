using System;
using System.Diagnostics;

namespace b2xtranslator.Tools;

public static class TraceLogger
{
    public enum LoggingLevel
    {
        None = 0,
        Error,
        Warning,
        Info,
        Debug,
        DebugInternal
    }
    
    public static bool EnableTimeStamp = true;
    private static LoggingLevel _logLevel = LoggingLevel.Info;
    
    public static LoggingLevel LogLevel
    {
        get => _logLevel;
        set => _logLevel = value;
    }
    
    private static void WriteLine(string msg, LoggingLevel level)
    {
        if (_logLevel >= level && EnableTimeStamp)
        {
            try
            {
                Trace.WriteLine(string.Format("{0} " + msg, DateTime.Now));
            }
            catch (Exception)
            {
                Trace.WriteLine("The tracing of the folloging message throw an error: " + msg);
            }
        }
        else if (_logLevel >= level)
        {
            Trace.WriteLine(msg);
        }
    }
    
    /// <summary>
    ///     Write a line on error level (is written if level != none)
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="objs"></param>
    public static void Simple(string msg, params object[] objs)
    {
        if (msg == null || msg == "")
        {
            return;
        }
        
        WriteLine(string.Format(msg, objs), LoggingLevel.Error);
    }
    
    public static void DebugInternal(string msg, params object[] objs)
    {
        if (msg == null || msg == "")
        {
            return;
        }
        
        WriteLine("[D] " + string.Format(msg, objs), LoggingLevel.DebugInternal);
    }
    
    public static void Debug(string msg, params object[] objs)
    {
        if (msg == null || msg == "")
        {
            return;
        }
        
        WriteLine("[D] " + string.Format(msg, objs), LoggingLevel.Debug);
    }
    
    public static void Info(string msg, params object[] objs)
    {
        if (msg == null || msg == "")
        {
            return;
        }
        
        WriteLine("[I] " + string.Format(msg, objs), LoggingLevel.Info);
    }
    
    public static void Warning(string msg, params object[] objs)
    {
        if (msg == null || msg == "")
        {
            return;
        }
        
        WriteLine("[W] " + string.Format(msg, objs), LoggingLevel.Warning);
    }
    
    public static void Error(string msg, params object[] objs)
    {
        if (msg == null || msg == "")
        {
            return;
        }
        
        WriteLine("[E] " + string.Format(msg, objs), LoggingLevel.Error);
    }
}