using ConsolePro8;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ThreadMateLib.Domain;

namespace ThreadMateCmd;

internal class Program
{
    private const string ExitCommand = "exit";
    private const string ListCommand = "list";
    private const string ClearCommand = "clear";
    private static ProcessManager _threadMate;
    static void Main(string[] args)
    {
        _threadMate = new();
        ConsolePro.SetDimensions(50, 120);
        ConsolePro.DrawTitle("ThreadMate Command Line");
        PrintMenu();
        MainLoop();
    }

    private static void MainLoop()
    {
        ListProcesses();
        do
        {
            ConsolePro.Write("ThreadMate>> ");
            MatchAndInvoke(Console.ReadLine());
        }
        while (true);
    }

    private static void PrintMenu()
    {
        ConsolePro.WriteLine("Options" +
            "\nlist [filter] - lists all processes, with optional filter" +
            "\n#,#,#... clear - clears the affinity for the process (available for all cores)" +
            "\n#,#,#... #,#,#... - sets affinity for the process for comma delimited core list" +
            "\n#,#,#... %m# - sets the affinity for the process based on a mask" +
            "\nexit - exits the application");
    }

    private static void PrintError(string message)
    {
        ConsolePro.WriteLine($"Error: {message}", ConsoleColor.Red);
    }

    private static void MatchAndInvoke(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return;
        }
        var tokens = input.Split(' ');
        switch (tokens[0].ToLower())
        {
            case ListCommand:
                if (tokens.Length > 1)
                {
                    ListProcesses(tokens[1]);
                }
                else
                {
                    ListProcesses();
                }
                break;
            case ExitCommand:
                Exit();
                break;
            default:
                if (tokens.Length == 1)
                {
                    PrintError("Unable to understand command");
                    PrintMenu();
                    break;
                }
                PerformAffinityTask(tokens);
                break;
        }
    }

    private static Task ListProcessesAsync(string? filter = null)
    {
        return new Task(() =>
        {
            _threadMate.ReadCurrentProcesses();
            ConsoleProTable table = new(new ConsoleProTableHeader("Id", 25),
                new ConsoleProTableHeader("Process Name", 75));

            for (int i = 0; i < _threadMate.Processes.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(filter)
                    || _threadMate.Processes[i].Process.ProcessName.ToLower()
                        .Contains(filter.ToLower()))
                {
                    table.Rows.Add(new ConsoleProTableRow(i.ToString(),
                        _threadMate.Processes[i].ToString()));
                    //ConsolePro.WriteLine($"{i} {_threadMate.Processes[i]}");
                }

            }
            table.WriteToConsole();
            ConsolePro.WriteLine();
        });
    }

    private static void ListProcesses(string? filter = null)
    {
        ConsolePro.ShowTwirlerWhileTaskRuns(ListProcessesAsync(filter));
    }

    private static void Exit()
    {
        Environment.Exit(0);
    }

    private static void PerformAffinityTask(string[] tokens)
    {
        bool success = false;
        string[] procNos = tokens[0].Split(',');
        for (int i = 0; i < procNos.Length; i++)
        {
            if (int.TryParse(procNos[0], out int selection))
            {
                if (Regex.IsMatch(tokens[1], "%m[0-9]+$"))
                {
                    int maskNo = int.Parse(tokens[1].Substring(2));
                    PrintError("Not yet supported");
                    //_threadMate.Processes[selection].SetAffinity()
                    success = true;
                }
                else if (tokens[1] == ClearCommand)
                {
                    _threadMate.Processes[selection].ResetAffinityToDefault();
                    success = true;
                }
                else
                {
                    var cores = tokens[1].Split(',');
                    List<Processor> procs = new List<Processor>();
                    foreach (string core in cores)
                    {
                        if (Regex.IsMatch(core, "[0-9]+$"))
                        {
                            procs.Add(new Processor(int.Parse(core)));
                        }
                    }
                    if (procs.Count == 0)
                    {
                        PrintError("Need at least one core to set affinity on.");
                    }
                    else
                    {
                        _threadMate.Processes[selection].SetAffinity(procs);
                        success = true;
                    }
                }
            }
        }
        if (!success)
        {
            PrintError("Command not recognized.");
            PrintMenu();
        }
    }
}
