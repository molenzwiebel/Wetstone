using System;
using System.IO;
using System.Threading.Tasks;
using BepInEx.Logging;

namespace Wetstone.Network.Logger.Handler;

class FileLogHandler : ILogHandler
{
    private static readonly string LogFilePath = $"logs/{PluginInfo.PLUGIN_GUID}.log";

    public FileLogHandler()
    {
        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(LogFilePath));
    }

    public bool Accept(LogLevel level)
    {
        return level <= LogLevel.All;
    }

    public Task Handle(LogLevel level, string message)
    {
        using (FileStream fs = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write, FileShare.None))
        {
            fs.Write(System.Text.Encoding.UTF8.GetBytes(this.FormatLog(level, message)));
        }
        return Task.CompletedTask;
    }

    private string FormatLog(LogLevel level, string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        return $"[{timestamp}] [{level}] {message}\r\n";
    }
}
