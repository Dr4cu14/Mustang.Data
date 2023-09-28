using System.Diagnostics;
using System.Net;

namespace Mustang.Logger;

public class Logger : ILogger
{
    public static readonly ILogger Instance = new Logger();

    public async Task WriteLog(string content, string? exFilePath = null)
    {
        LogEntity log = new LogEntity
        {
            Id = Guid.NewGuid(),
            ServerTime = DateTime.Now,
            Source = "插件系统",
            ServerIP = GetServerIp(),
            ServerName = Dns.GetHostName()
        };
        var process = Process.GetCurrentProcess();
        log.ProcessId = process.Id;
        log.ProcessName = process.ProcessName;
        log.ThreadID = Thread.CurrentThread.ManagedThreadId;
        log.Content = content;

        await EmitterFactory.GetLogEmitter().EmitLog(log, exFilePath);
    }

    public async Task WriteLog(Exception ex, string? exFilePath = null)
    {
        LogEntity log = new LogEntity
        {
            Id = Guid.NewGuid(),
            ServerTime = DateTime.Now,
            Source = "插件系统",
            ServerIP = GetServerIp(),
            ServerName = Dns.GetHostName()
        };
        var process = Process.GetCurrentProcess();
        log.ProcessId = process.Id;
        log.ProcessName = process.ProcessName;
        log.ThreadID = Thread.CurrentThread.ManagedThreadId;
        log.Content = ex.Message;
        log.StackTrace = ex.StackTrace ?? string.Empty;
        await EmitterFactory.GetLogEmitter().EmitLog(log, exFilePath);
    }

    public async Task WriteLog(string content, Exception ex = null, string? exFilePath = null)
    {
        var log = new LogEntity
        {
            Id = Guid.NewGuid(),
            ServerTime = DateTime.Now,
            Source = "插件系统",
            ServerIP = GetServerIp(),
            ServerName = Dns.GetHostName()
        };
        var process = Process.GetCurrentProcess();
        log.ProcessId = process.Id;
        log.ProcessName = process.ProcessName;
        log.ThreadID = Thread.CurrentThread.ManagedThreadId;
        log.Content = $"{content}, Message：{ex.Message}";
        log.StackTrace = ex.StackTrace ?? string.Empty;

        await new TextEmitter().EmitLog(log, exFilePath);
    }


    private static string serverIp;
    private string GetServerIp()
    {
        if (string.IsNullOrEmpty(serverIp))
        {
            IPAddress[] address = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (IPAddress addr in address)
            {
                string tmp = addr.ToString().Trim();
                //过滤IPv6的地址信息
                if (tmp.Length <= 16 && tmp.Length > 5)
                {
                    serverIp = tmp;
                    break;
                }
            }
        }

        if (string.IsNullOrEmpty(serverIp))
        {
            return string.Empty;
        }
        return serverIp;
    }
}