using System.IO;
using System.Text;
using Mustang.Logger;

namespace Mustang.Logger;

public class TextEmitter : ILogEmitter
{
    public async Task EmitLog(LogEntity log, string? customFilePath = null)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory() + "\\logs\\");

        if (!string.IsNullOrWhiteSpace(customFilePath))
            filePath = Path.Combine(customFilePath + "\\logs\\");

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        await WriteToFile(log.GetStrEntity(), Path.Combine(filePath, DateTime.Now.ToString("yyyy-MM-dd") + ".txt"));
    }

    private async Task WriteToFile(string log, string filePath)
    {
        DateTime now = DateTime.Now;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"** [{now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] - Begin **************************************************************");
        sb.AppendLine(log);
        sb.AppendLine($"** [{now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] - End ****************************************************************");

        var textByte = Encoding.UTF8.GetBytes(sb.ToString());
        await using var logStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Write);
        await logStream.WriteAsync(textByte, 0, textByte.Length);
        logStream.Close();
    }
}