using System.Threading.Tasks;
using BepInEx.Logging;

namespace Wetstone.Network.Logger.Handler;

interface ILogHandler
{
    bool Accept(LogLevel level);

    Task Handle(LogLevel level, string message);
}