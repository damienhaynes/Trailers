using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using MediaPortal.Configuration;

namespace Trailers
{
    static class FileLog
    {
        private static string logFilename = Config.GetFile(Config.Dir.Log,"Trailers.log");
        private static string backupFilename = Config.GetFile(Config.Dir.Log, "Trailers.bak");
        private static Object lockObject = new object();

        static FileLog()
        {
            // default logging before we load settings
            PluginSettings.LogLevel = 2;

            if (File.Exists(logFilename))
            {
                if (File.Exists(backupFilename))
                {
                    try
                    {
                        File.Delete(backupFilename);
                    }
                    catch
                    {
                        Error("Failed to remove old backup log");
                    }
                }
                try
                {
                    File.Move(logFilename, backupFilename);
                }
                catch
                {
                    Error("Failed to move logfile to backup");
                }
            }
        }

        internal static void Info(String log)
        {
            if (PluginSettings.LogLevel >= 2)
                WriteToFile(String.Format(CreatePrefix(), "INFO", log));
        }

        internal static void Info(String format, params Object[] args)
        {
            Info(String.Format(format, args));
        }

        internal static void Debug(String log)
        {
            if (PluginSettings.LogLevel >= 3)
                WriteToFile(String.Format(CreatePrefix(), "DEBG", log));
        }

        internal static void Debug(String format, params Object[] args)
        {
            Debug(String.Format(format, args));
        }

        internal static void Error(String log)
        {
            if (PluginSettings.LogLevel >= 0)
                WriteToFile(String.Format(CreatePrefix(), "ERR ", log));
        }

        internal static void Error(String format, params Object[] args)
        {
            Error(String.Format(format, args));
        }

        internal static void Warning(String log)
        {
            if (PluginSettings.LogLevel >= 1)
                WriteToFile(String.Format(CreatePrefix(), "WARN", log));
        }

        internal static void Warning(String format, params Object[] args)
        {
            Warning(String.Format(format, args));
        }

        private static String CreatePrefix()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " [{0}] " + String.Format("[{0}][{1}]", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2,'0')) +  ": {1}";
        }

        private static void WriteToFile(String log)
        {
            try
            {
                lock (lockObject)
                {
                    StreamWriter sw = File.AppendText(logFilename);
                    sw.WriteLine(log);
                    sw.Close();
                }
            }
            catch
            {
                Error("Failed to write out to log");
            }
        }
    }
}
