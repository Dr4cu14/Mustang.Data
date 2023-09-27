using Mustang.Logger;

namespace Mustang.Logger;

public interface ILogEmitter
{
    Task EmitLog(LogEntity log, string? customFilePath = null);
}