using System;

namespace Task_Manager
{
    public class Process
    {
        public Process(string name, float cpu, float memory)
        {
            Name = name;
            CPU = Math.Round((decimal)cpu, 1);
            Memory = Math.Round((decimal)memory, 1);
        }

        public string Name;
        public decimal CPU;
        public decimal Memory;
    }
}
