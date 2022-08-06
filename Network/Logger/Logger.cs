using System.Collections.Generic;
using System.Threading.Tasks;
using BepInEx.Logging;
using Wetstone.Network.Logger.Handler;

namespace Wetstone.Network.Logger;

internal class Logger
{
    public List<ILogHandler> Handlers { get; } = new List<ILogHandler>();

    public void RegisterLogHandler(ILogHandler handler)
    {
        Handlers.Add(handler);
    }

    public async Task LogFatal(string message)
    {
        await Log(LogLevel.Fatal, message);
    }

    public async Task LogError(string message)
    {
        await Log(LogLevel.Error, message);
    }

    public async Task LogWarning(string message)
    {
        await Log(LogLevel.Warning, message);
    }

    public async Task LogMessage(string message)
    {
        await Log(LogLevel.Message, message);
    }

    public async Task LogInfo(string message)
    {
        await Log(LogLevel.Info, message);
    }

    public async Task LogDebug(string message)
    {
        await Log(LogLevel.Debug, message);
    }

    public async Task Log(LogLevel level, string message)
    {
        foreach (var handler in Handlers)
        {
            if (handler.Accept(level)) {
                await handler.Handle(level, message);
            }
        }
    }
}
