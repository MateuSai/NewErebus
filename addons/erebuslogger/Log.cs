using Godot;
using System;

namespace ErebusLogger;

public static class Log
{
    private enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal,
    }

    private static string GetLogLevelName(LogLevel level)
    {
        return level switch
        {
            LogLevel.Debug => "DEBUG",
            LogLevel.Info => "INFO",
            LogLevel.Warn => "WARN",
            LogLevel.Error => "ERROR",
            LogLevel.Fatal => "FATAL",
            _ => "UNKNOWN_LEVEL",
        };
    }

    private static void PrintLog(string mes, LogLevel level)
    {
        GD.PrintRich($"{GetLogLevelName(level)}: {mes}");
    }

    public static void Debug(string mes)
    {
        PrintLog(mes, LogLevel.Debug);
    }

    public static void Info(string mes)
    {
        PrintLog(mes, LogLevel.Info);
    }

    public static void Warn(string mes)
    {
        PrintLog(mes, LogLevel.Warn);
    }

    public static void Error(string mes)
    {
        PrintLog(mes, LogLevel.Error);
    }

    public static void Fatal(string mes, SceneTree tree)
    {
        PrintLog($"[color=red]{mes}[/color]", LogLevel.Fatal);
        tree.Quit(1);
    }
}
