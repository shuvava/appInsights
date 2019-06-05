using System;
using System.Runtime;

using Shuvava.Extensions.Metrics;


namespace getMetrics.CmdApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var gen0 = DotNetGC.CollectionCount(0);
            var mem = DotNetGC.GetTotalMemory();
            Console.WriteLine($"used memory {mem}; GC gen0 count of objects {gen0}");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
