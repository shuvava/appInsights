﻿using System;
using System.Linq;
using System.Runtime;
using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using Shuvava.Extensions.Metrics;


namespace getMetrics.CmdApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Application started!");

            //change GC settings
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

            var gcConfig = DotNetGC.GetConfig();
            Print(gcConfig);

            var sysUsageCollector = new ProcessSystemUsageCollector();
            var stat = sysUsageCollector.CollectData();
            Print(stat);

            var test = Enumerable.Repeat((long) 10, 1_000_000).ToArray();

            //using (var listener1 = new GcFinalizersEventListener())
            using (var listener1 = new MetricListener())
            {
                PrintGcStat();

                test = Enumerable.Repeat((long) 100, 1_000_000).ToArray();
                Console.WriteLine("\nPress ENTER to trigger a few finalizers...");
                Console.ReadLine();

                for (var i = 0; i < 4; i++)
                {
                    var t = new Thread(() => { });
                }

//            GC.Collect();
//            GC.WaitForPendingFinalizers();
                GC.Collect(2, GCCollectionMode.Forced, true, true);
            }

            PrintGcStat();
            stat = sysUsageCollector.CollectData();
            Print(stat);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }


        private static void PrintGcStat()
        {
            var gen0 = DotNetGC.CollectionCount(0);
            var gen1 = DotNetGC.CollectionCount(1);
            var gen2 = DotNetGC.CollectionCount(2);
            var mem = DotNetGC.GetTotalMemory();
            Console.WriteLine($"used memory {mem};");
            Console.WriteLine($"GC gen0 count of objects {gen0}");
            Console.WriteLine($"GC gen1 count of objects {gen1}");
            Console.WriteLine($"GC gen2 count of objects {gen2}");
        }


        private static void Print<T>(T obj)
        {
            var _settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Formatting = Formatting.Indented
            };

            _settings.Converters.Add(new StringEnumConverter {NamingStrategy = new CamelCaseNamingStrategy()});

            var result = JsonConvert.SerializeObject(obj, _settings);
            Console.WriteLine($"{nameof(ProcessSystemUsageCollector)}: {result}");
        }
    }
}
