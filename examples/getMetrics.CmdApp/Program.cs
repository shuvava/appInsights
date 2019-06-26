using System;
using System.Linq;
using System.Threading;

using Shuvava.Extensions.Metrics;


namespace getMetrics.CmdApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Application started!");
            var listener0 = new MetricListener(true);
//            var listener1 = new MetricListener(false);
//            GcFinalizersEventListener listener2 = new GcFinalizersEventListener();

            var test = Enumerable.Repeat((long)10, 1_000_000).ToArray();

            var gen0 = DotNetGC.CollectionCount(0);
            var gen1 = DotNetGC.CollectionCount(1);
            var gen2 = DotNetGC.CollectionCount(2);
            var mem = DotNetGC.GetTotalMemory();
            Console.WriteLine($"used memory {mem};");
            Console.WriteLine($"GC gen0 count of objects {gen0}");
            Console.WriteLine($"GC gen0 count of objects {gen1}");
            Console.WriteLine($"GC gen0 count of objects {gen2}");

            test = Enumerable.Repeat((long)100, 1_000_000).ToArray();
            Console.WriteLine("\nPress ENTER to trigger a few finalizers...");
            Console.ReadLine();

            for (int i = 0; i < 4; i++)
            {
                Thread t = new Thread(()=> {});
            }
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
            GC.Collect(2, GCCollectionMode.Forced, true, true);
//            var key = "x";
//
//            while (!string.IsNullOrEmpty(key))
//            {
//                test = Enumerable.Repeat((long)100, 1_000_000).ToArray();
//                var r = Console.ReadKey();
//                key = r.KeyChar.ToString();
//            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
