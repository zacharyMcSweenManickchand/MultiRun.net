using System.Text.RegularExpressions;
namespace VirtualShellReader;
public class Display
{
    public enum AnsiEscape : int
    {
        reset = 0,
        bold = 1,
        disable = 2,
        underline = 3,
        reverse = 4,
        strikethrough = 5,
        invisible = 6
    }

    public enum AnsiForeground : int
    {
        black = 30,
        red = 31,
        green = 32,
        orange = 33,
        blue = 34,
        purple = 35,
        cyan = 36,
        lightgrey = 37,
        darkgrey = 90,
        lightred = 91,
        lightgreen = 92,
        yellow = 93,
        lightblue = 94,
        pink = 95,
        lightcyan = 96,
        white = 97
    }

    public enum AnsiBackground : int
    {
        black = 40,
        red = 41,
        green = 42,
        orange = 43,
        blue = 44,
        purple = 45,
        cyan = 46,
        lightgrey = 47
    }
    public AnsiForeground? Foreground { get; set; }
    public AnsiBackground? Background { get; set; }

    public Display(AnsiForeground foreground, AnsiBackground background)
    {
        this.Foreground = foreground;
        this.Background = background;
    }

    private string highlight(string text)
    {
        string backg = Background != null ? $"{(int)Background};" : "";
        string forg = Foreground != null ? $"{(int)Foreground}" : "97";
        return $"\u001b[{backg}{forg}m{text}\u001b[0m";
    }

    private string[] splitLongLines(string line, int maxLength)
    {
        if (line.Length <= maxLength)
            return new[] { line };

        var lines = new System.Collections.Generic.List<string>();

        for (int i = 0; i < line.Length; i += maxLength)
        {
            lines.Add(line.Substring(i, Math.Min(maxLength, line.Length - i)));
        }

        return lines.ToArray();
    }

    private void printBox(string text)
    {
        int maxLength = Console.WindowWidth - 4; // Adjusting for box borders
        string[] lines = text.Split('\n');

        // Calculate width of the box
        int boxWidth = Math.Min(maxLength, lines.Max(line => line.Length) + 4); // Adjusting for box borders

        // Top border
        Console.WriteLine(highlight(" ╔" + new string('═', boxWidth - 2) + "╗ "));

        // Content
        foreach (string line in lines)
        {
            // Split long lines into multiple lines
            var splittedLines = splitLongLines(line, boxWidth - 4);
            foreach (var splitLine in splittedLines)
            {
                string removedExtra = RemoveColorCodes(splitLine);
                int extra = splitLine.Length - removedExtra.Length;
                Console.Write(highlight(" ║ ") + splitLine.PadRight(boxWidth - 4 + extra) + highlight(" ║ ") + "\n");
            }
        }

        // Bottom border
        Console.WriteLine(highlight(" ╚" + new string('═', boxWidth - 2) + "╝ "));
    }

    private string RemoveColorCodes(string input)
    {
        string pattern = @"\u001b\[[0-9;]*m";
        return Regex.Replace(input, pattern, string.Empty);
    }

    public void Print(string title, string output)
    {
        Console.WriteLine("\n" + highlight($" {title} "));
        printBox(output);
    }

}
