using System.Runtime.Serialization;
using System.Text;

namespace Mustang.Logger;

public class LogEntity
{
    public Guid Id { get; set; }

    public string Source { get; set; }


    public string Content { get; set; }


    public string ServerIP { get; set; }


    public string ServerName { get; set; }


    public DateTime ServerTime { get; set; }


    public int ProcessId { get; set; }


    public string ProcessName { get; set; }


    public int ThreadID { get; set; }

    public string StackTrace { get; set; }

    public string GetStrEntity()
    {
        StringBuilder manualSerialized = new StringBuilder();
        manualSerialized.AppendLine($"[Time]: {ServerTime:yyyy-MM-dd HH:mm:ss.fff}");
        manualSerialized.AppendLine($"[ID]: {Id}");
        manualSerialized.AppendLine($"[Source]: {Source}");
        manualSerialized.AppendLine($"[ServerIP]: {ServerIP}");
        manualSerialized.AppendLine($"[ServerName]: {ServerName} ");
        manualSerialized.AppendLine($"[ProcessID]: {ProcessId}");
        manualSerialized.AppendLine($"ProcessName]: {ProcessName}");
        manualSerialized.AppendLine($"[ThreadID]: {ThreadID}");

        manualSerialized.AppendLine($"[Message]: {Content}");

        if (!string.IsNullOrWhiteSpace(StackTrace))
            manualSerialized.AppendLine($"[StackTrace]: {StackTrace}");

        return manualSerialized.ToString();
    }
}