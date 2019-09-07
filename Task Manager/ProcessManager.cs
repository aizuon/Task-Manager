using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Task_Manager
{
    public static class ProcessManager
    {
        public static ushort Sleep = 1000;

        public static List<Process> GetProcesses()
        {
            var allProcesses = System.Diagnostics.Process.GetProcesses().ToList().Where(p => p.Id != 0);

            var cpulist = new List<PerformanceCounter>();

            foreach (var p in allProcesses)
            {
                try
                {
                    var cpucounter = new PerformanceCounter("Process", "% Processor Time", p.ProcessName, true);
                    cpucounter.NextValue();
                    cpulist.Add(cpucounter);
                }
                catch
                {
                    continue;
                }
            }

            Thread.Sleep(Sleep);

            var list = new List<Process>();

            foreach (var p in allProcesses)
            {
                try
                {
                    list.Add(new Process(p.ProcessName, cpulist.Find(prf => prf.InstanceName == p.ProcessName).NextValue(), Convert.ToSingle(p.PrivateMemorySize64 / (1000000))));
                }
                catch
                {
                    continue;
                }
            }

            return list;
        }

        public static void Terminate(string process)
        {
            var proc = System.Diagnostics.Process.GetProcessesByName(process);

            if (proc != null)
                proc[0].Kill();
        }
    }
}
