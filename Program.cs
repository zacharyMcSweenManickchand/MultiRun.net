namespace VirtualShellReader;
class Program
{

    static async Task Main(string[] args)
    {
        string directoryPath;
        if (args.Length > 0)
            directoryPath = args[0];
        else
            directoryPath = ".";

        List<string> projs = SearchProjectsFiles(directoryPath);
        List<Func<Task>> tasks = new List<Func<Task>>();
        foreach (var item in LoadProjects(projs))
        {
            tasks.Add(async () => await item.StartShell());
        }

        await Task.WhenAll(tasks.Select(task => Task.Run(task)));
    }

    static IEnumerable<Shell> LoadProjects(List<string> projects)
    {
        var displays = new[]{
            new Display(Display.AnsiForeground.white, Display.AnsiBackground.blue),
            new Display(Display.AnsiForeground.white, Display.AnsiBackground.red),
            new Display(Display.AnsiForeground.white, Display.AnsiBackground.green),
            new Display(Display.AnsiForeground.white, Display.AnsiBackground.purple),
            new Display(Display.AnsiForeground.white, Display.AnsiBackground.black),
        };
        for (int i = 0; i < projects.Count; i++)
        {
            yield return new Shell(projects.ElementAt(i), displays.ElementAt(i));
        }
    }

    static List<string> SearchProjectsFiles(string directory)
    {
        List<string> txtFilePaths = new List<string>();

        try
        {
            foreach (string file in Directory.GetFiles(directory, "*.csproj"))
            {
                txtFilePaths.Add(file);
            }

            foreach (string subdirectory in Directory.GetDirectories(directory))
            {
                List<string> subdirectoryFiles = SearchProjectsFiles(subdirectory);
                txtFilePaths.AddRange(subdirectoryFiles);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error accessing directory {directory}: {e.Message}");
        }

        return txtFilePaths;
    }
}
