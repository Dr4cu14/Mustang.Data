namespace Mustang.Logger;

public interface ILogger
{
    Task WriteLog(string content, string? exFilePath = null);

    Task WriteLog(Exception ex, string? exFilePath = null);

    Task WriteLog(string content, Exception? ex = null, string? exFilePath = null);
}