using System.Diagnostics;

namespace VirtualShellReader;
// Mostly a wrapper around `Process`
class Shell
{
    private string FileLocation;
    private Display display;
    private Process? shell;

    public Shell(string FileLocation, Display display)
    {
        this.FileLocation = FileLocation;
        this.display = display;
    }

    public async Task StartShell()
    {
        Console.WriteLine("Start Shell");
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = "/bin/bash";
        info.Arguments = $"-c \"dotnet run --project '{FileLocation}'\"";
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        Console.WriteLine($"-c \"dotnet run --project '{FileLocation}'\"");

        shell = Process.Start(info);
        if (shell == null)
        {
            throw new Exception("Shell is not connected");
        }
        await StreamOutput(shell);
        await shell.WaitForExitAsync(); // Make WaitForExit asynchronous
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
                    break;  // Exit the loop when end of stream is reached

                lineBuffer.Add(line);

                if (reader.Peek() == -1)
                {
                    display.Print(FileLocation, string.Join(Environment.NewLine, lineBuffer));
                    lineBuffer.Clear();
                }
            }
        }
    }
}
