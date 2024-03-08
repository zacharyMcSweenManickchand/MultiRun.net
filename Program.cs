namespace VirtualShellReader;
class Program
{

    static async Task Main(string[] args)
    {
        List<string> projs = SearchProjectsFiles("");
        List<Func<Task>> tasks = new List<Func<Task>>();
        foreach (var item in LoadProjects(projs))
        {
            tasks.Add(async() => await item.StartShell());
        }

        await Task.WhenAll(tasks.Select(task => Task.Run(task)));
    }

    static IEnumerable<Shell> LoadProjects(List<string> projects) {
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
            // Process all the files in the current directory
            foreach (string file in Directory.GetFiles(directory, "*.csproj"))
            {
                txtFilePaths.Add(file); // Add the file path to the list
                Console.WriteLine(file);
            }

            // Recursively search all subdirectories
            foreach (string subdirectory in Directory.GetDirectories(directory))
            {
                List<string> subdirectoryFiles = SearchProjectsFiles(subdirectory);
                txtFilePaths.AddRange(subdirectoryFiles); // Add files found in subdirectories
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error accessing directory {directory}: {e.Message}");
        }

        return txtFilePaths;
    }
}
