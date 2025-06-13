using Serilog.Events;

namespace CarteAuxTresors.Utils;

public static class LogHelper
{
    public static void LogEvent(LogEventLevel level, string template, ConsoleColor color, params object[] values)
    {
        // Affichage console coloré (avec interpolation)
        // Console.ForegroundColor = color;
        // Console.WriteLine(string.Format(template, values));
        // Console.ResetColor();

        // Logging structuré (template Serilog)
        switch (level)
        {
            case LogEventLevel.Information:
                Log.Information(template, values);
                break;
            case LogEventLevel.Warning:
                Log.Warning(template, values);
                break;
            case LogEventLevel.Error:
                Log.Error(template, values);
                break;
            case LogEventLevel.Debug:
                Log.Debug(template, values);
                break;
            case LogEventLevel.Verbose:
                Log.Verbose(template, values);
                break;
            case LogEventLevel.Fatal:
                Log.Fatal(template, values);
                break;
        }
    }

    // Méthodes simplifiées
    public static void Info(string template, params object[] values) =>
        LogEvent(LogEventLevel.Information, template, ConsoleColor.Gray, values);

    public static void Success(string template, params object[] values) =>
        LogEvent(LogEventLevel.Information, template, ConsoleColor.Green, values);

    public static void Warning(string template, params object[] values) =>
        LogEvent(LogEventLevel.Warning, template, ConsoleColor.Yellow, values);

    public static void Error(string template, params object[] values) =>
        LogEvent(LogEventLevel.Error, template, ConsoleColor.Red, values);
        
    public static void Fatal(string template, params object[] values) =>
    LogEvent(LogEventLevel.Fatal, template, ConsoleColor.DarkRed, values);
}