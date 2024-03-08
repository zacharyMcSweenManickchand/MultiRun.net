namespace VirtualShellReader;
class Program
{
    static async Task Main(string[] args)
    {
        List<string> projs = SearchProjFiles("");
        var tasks = new[]
        {
                new Shell(projs.ElementAt(0), new Display(Display.AnsiForeground.white, Display.AnsiBackground.blue)).StartShell(),
                //new Shell(projs.ElementAt(2), new Display(Display.AnsiForeground.white, Display.AnsiBackground.red)).StartShell(),
                //new Shell(projs.ElementAt(3), new Display(Display.AnsiForeground.white, Display.AnsiBackground.purple)).StartShell(),
            };

        await Task.WhenAll(tasks);
    }
    static List<string> SearchProjFiles(string directory)
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
                List<string> subdirectoryFiles = SearchProjFiles(subdirectory);
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
