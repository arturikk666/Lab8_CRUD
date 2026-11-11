using System;
using System.IO;

namespace Lab8_CRUD
{
    public static class Logger
    {
        private static readonly string logFilePath = "errors.log";

        public static void LogError(string message, Exception ex = null)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | ERROR | {message}";
                if (ex != null)
                {
                    logEntry += $" | {ex.GetType().Name}: {ex.Message}";
                }
                logEntry += Environment.NewLine;
                File.AppendAllText(logFilePath, logEntry);
            }
            catch
            {
                // Если не удалось записать лог — не падаем
            }
        }

        public static void LogInfo(string message)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | INFO | {message}{Environment.NewLine}";
                File.AppendAllText(logFilePath, logEntry);
            }
            catch { }
        }
    }
}