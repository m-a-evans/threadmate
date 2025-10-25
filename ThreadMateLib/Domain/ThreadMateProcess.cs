using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMateLib.Domain
{
    public class ThreadMateProcess
    {
        public Process Process;

        public nint AffinityBitmask;

        public ProcessPriorityClass Priority;

        public ThreadMateProcess(Process process)
        {
            Process = process;
            AffinityBitmask = process.ProcessorAffinity;
            Priority = process.PriorityClass;
        }

        public void SetAffinity(List<Processor> processors)
        {
            int affinity = 0;
            foreach (var processor in processors)
            {
                affinity = affinity | (int)Math.Pow(2, processor.Id);
            }
            AffinityBitmask = affinity;
            Process.ProcessorAffinity = AffinityBitmask;
        }

        public void ResetAffinityToDefault()
        {
            AffinityBitmask = 0;
            Process.ProcessorAffinity = 0;
        }
        public void SetPriority(ProcessPriorityClass priority)
        {
            Priority = priority;
            Process.PriorityClass = priority;
        }

        public override string ToString()
        {
            return $"{Process.ProcessName}:{AffinityBitmask}";
        }
    }
}
