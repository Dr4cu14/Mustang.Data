using System.Collections.Concurrent;

namespace Mustang.Logger;

public class EmitterFactory
{
    private static readonly ConcurrentDictionary<string, ILogEmitter> LogEmitters = new();

    static EmitterFactory()
    {
        LogEmitters.TryAdd(nameof(TextEmitter).ToString(), new TextEmitter());
    }

    public static ILogEmitter GetLogEmitter()
    {
        if (LogEmitters.TryGetValue(nameof(TextEmitter), out var logEmitter))
        {
            return logEmitter;
        }

        ;
        throw new Exception("ILogEmitter not found");
    }
}