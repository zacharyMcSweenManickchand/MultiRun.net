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

    public async Task StartShell(int num)
    {
        await Task.Delay(num * 1000);
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = "/bin/bash";
        info.Arguments = $"-c \"./script.sh {num+1} {2}\"";
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;

        shell = Process.Start(info);
        if (shell == null)
        {
            throw new Exception("Shell is not connected");
        }
        await StreamOutput(shell, num);
        await shell.WaitForExitAsync(); // Make WaitForExit asynchronous
    }

    public async Task StreamOutput(Process process, int num)
    {
        using (StreamReader reader = process.StandardOutput)
        {
            List<string?> lineBuffer = new List<string?>();
            while (!reader.EndOfStream)
            {
                Task<string?> readLineTask = reader.ReadLineAsync();

                Task completedTask = await Task.WhenAny(readLineTask, Task.Delay(TimeSpan.FromMilliseconds(500)));

                if (completedTask == readLineTask)
                {
                    string? line = await readLineTask;
                    if (line == null)
                        break;  // Exit the loop when end of stream is reached

                    // If the line is empty, flush the buffer
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        // Timeout occurred, flush the buffer
                        display.Print($"Process {num}", string.Join(Environment.NewLine, lineBuffer));
                        lineBuffer.Clear();
                    }
                    else
                    {
                        lineBuffer.Add(line);
                    }
                }
                else
                {
                    // Timeout occurred, flush the buffer
                    display.Print($"Process {num}", string.Join(Environment.NewLine, lineBuffer));
                    lineBuffer.Clear();
                }
            }
        }
    }
}
