using McMaster.Extensions.CommandLineUtils;
using ClassLibrary1;
[Subcommand(typeof(VersionCommand), typeof(RunCommand), typeof(SetPathCommand))]
class Program
{
    static int Main(string[] args)
    {
        try
        {
            return CommandLineApplication.Execute<Program>(args);
        }
        catch (CommandParsingException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Use --help to see valid commands and options.");
            return 1;
        }
    }
}

[Command("version", Description = "Displays author and version information")]
public class VersionCommand
{
    private void OnExecute() => Console.WriteLine("Author: Kosenko Klym\nVersion: 1.0.0");
}

[Command("run", Description = "Runs the specified lab")]
public class RunCommand
{
    [Argument(0, Description = "Name of the lab to run (lab1, lab2, lab3)")]
    public string Lab { get; set; }

    [Option("-I|--input <INPUT>", Description = "Input file")]
    public string Input { get; set; }

    [Option("-o|--output <OUTPUT>", Description = "Output file")]
    public string Output { get; set; }

    private void OnExecute()
    {
        if (!string.IsNullOrEmpty(Input) && File.Exists(Input))
        {
            RunLab();
            return;
        }

        string labPath = File.Exists("config.txt") ? File.ReadAllText("config.txt") : "";  
        Console.WriteLine($"LAB_PATH={labPath} and path combine is {Path.Combine(labPath, Input)}");
        if (!string.IsNullOrEmpty(labPath) && !string.IsNullOrEmpty(Input))
        {
            Input = Path.Combine(labPath, Input);
            RunLab();
            return;
        }

        Console.WriteLine("Should not be here");
        string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        Console.WriteLine($"Home directory is {homeDirectory} and path combine is {Path.Combine(homeDirectory, Input)}");
        if (File.Exists(Path.Combine(homeDirectory, Input)))
        {
            Input = homeDirectory;
            RunLab();
            return;
        }

        Console.Error.WriteLine("Error: Cannot find the input file.");
    }

    private void RunLab()
    {
        switch (Lab)
        {
            case "First":
                ClassLibrary1.Lab1.Run(Input, Output);
                break;
            case "Second":
                ClassLibrary1.Lab2.Run(Input, Output);
                break;
            case "Third":
                ClassLibrary1.Lab3.Run(Input, Output);
                break;
            default:
                Console.Error.WriteLine($"Error: Unknown lab {Lab}.");
                return;
        }

        Console.WriteLine($"Running {Lab} with input={Input} and output={Output}");
    }
}

[Command("set-path", Description = "Sets the path to the folder with input and output files.")]
public class SetPathCommand
{
    [Option("-p|--path <PATH>", CommandOptionType.SingleValue, Description = "Path to the folder with input and output files.")]
    public string Path { get; set; }

    private void OnExecute()
    {
        try
        {
            File.WriteAllText("config.txt", Path);
            Console.WriteLine($"Successfully set LAB_PATH to {Path}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error setting LAB_PATH: {ex.Message}");
        }
    }
}

public class CommandBase
{
    [Option("-I|--input <INPUT>", CommandOptionType.SingleValue)]
    public string Input { get; set; }

    [Option("-o|--output <OUTPUT>", CommandOptionType.SingleValue)]
    public string Output { get; set; }
}