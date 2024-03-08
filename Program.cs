namespace VirtualShellReader;
class Program
{
    static async Task Main(string[] args)
    {
        var tasks = new[]
        {
                new Shell("", new Display(Display.AnsiForeground.white, Display.AnsiBackground.cyan)).StartShell(32),
                new Shell("", new Display(Display.AnsiForeground.white, Display.AnsiBackground.blue)).StartShell(2),
                new Shell("", new Display(Display.AnsiForeground.white, Display.AnsiBackground.black)).StartShell(6),
                new Shell("", new Display(Display.AnsiForeground.white, Display.AnsiBackground.purple)).StartShell(0),
                new Shell("", new Display(Display.AnsiForeground.white, Display.AnsiBackground.red)).StartShell(4)
            };

        await Task.WhenAll(tasks);
    }
}
