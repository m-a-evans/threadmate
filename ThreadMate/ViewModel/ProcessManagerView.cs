using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadMate.Model;

namespace ThreadMate.ViewModel
{
    public class ProcessManagerView
    {
        public ObservableCollection<ThreadMateProcess> Processes { get; set; } = [];

        public ObservableCollection<Processor> Processors { get; set; } = [];

        public ProcessManagerView()
        {
            for (int i = 1; i <= Environment.ProcessorCount; i++)
            {
                Processors.Add(new Processor(i));
            }

            Task.Run(GetAllProcesses);
        }

        public void GetAllProcesses()
        {
            Process[] processList = Process.GetProcesses();
            foreach (var process in processList)
            {
                Processes.Add(new ThreadMateProcess(process));
            }
        }

    }
}
