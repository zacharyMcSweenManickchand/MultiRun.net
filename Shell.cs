using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VirtualShellReader;
class Shell
{
    private string FileLocation;
    private Display display;
    private Process? shell;
    private bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows); 

    public Shell(string FileLocation, Display display)
    {
        this.FileLocation = FileLocation;
        this.display = display;
    }

    public async Task StartShell()
    {
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = "/bin/bash";
        if (this.isWindows){
            info.FileName = "cmd.exe";
        }
        info.Arguments = $"-c \"dotnet run --project '{FileLocation}'\"";
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;

        shell = Process.Start(info);
        if (shell == null)
        {
            throw new Exception("Shell is not connected");
        }
        await StreamOutput(shell);
        await shell.WaitForExitAsync();
    }

    public async Task StreamOutput(Process process)
    {
        using (StreamReader reader = process.StandardOutput)
        {
            List<string?> lineBuffer = new List<string?>();

            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null)
                    break;

                lineBuffer.Add(line);

                if (reader.Peek() == -1)
                {
                    display.Print(Path.GetFileNameWithoutExtension(FileLocation), string.Join(Environment.NewLine, lineBuffer));
                    lineBuffer.Clear();
                }
            }
        }
    }
}
