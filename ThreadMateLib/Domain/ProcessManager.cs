using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMateLib.Domain
{
    public class ProcessManager
    {
        
        public List<ThreadMateProcess> Processes = [];

        public void ReadCurrentProcesses(bool silentError = true)
        {
            Process[] procs = Process.GetProcesses();

            var procsList = procs.ToList();

            List<ThreadMateProcess> tmprocs = [];
            foreach (var proc in procs)
            {
                try
                {
                    tmprocs.Add(new ThreadMateProcess(proc));
                }
                catch (Win32Exception w32ex)
                {
                    if (!silentError)
                    {
                        Console.WriteLine($"Skipping {proc.ProcessName} ({w32ex.Message})");
                    }
                }
                catch (InvalidOperationException ioex)
                {
                    if (!silentError)
                    {
                        Console.WriteLine($"Skipping {proc.ProcessName} ({ioex.Message})");
                    }
                }
            }
            // TODO - update list, dont just blow it out
            //var tmprocs = procsList.Select(x => new ThreadMateProcess(x)).ToList();

            
            var diffProcs = tmprocs.FindAll(x =>
                !Processes.Exists(y => y.Process.ProcessName == x.Process.ProcessName));

            // TODO - add diff procs to Processes. It contains all the new ones.
            Processes = tmprocs;
        }

    }
}
