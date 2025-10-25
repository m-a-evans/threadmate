using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMate.Model
{
    public class ThreadMateProcess
    {
        public Process Process;
        public ThreadMateProcess(Process process)
        {
            Process = process;
        }
        public void SetAffinity(List<Processor> processors)
        {
            int affinity = 0;
            foreach (var processor in processors)
            {
                affinity = affinity | processor.Id;
            }
            Process.ProcessorAffinity = (IntPtr)affinity;
        }

        public void ResetAffinityToDefault(int maxProcessors)
        {
            Process.ProcessorAffinity = (IntPtr)(2 * maxProcessors - 1);
        }
        public void SetPriority(ProcessPriorityClass priority)
        {
            Process.PriorityClass = priority;
        }
    }
}
