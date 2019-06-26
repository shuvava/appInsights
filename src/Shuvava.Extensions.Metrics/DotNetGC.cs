using System;
using System.Runtime;

using Shuvava.Extensions.Metrics.Models;


// ReSharper disable InconsistentNaming



namespace Shuvava.Extensions.Metrics
{
    public static class DotNetGC
    {

        /// <summary>Returns the number of times garbage collection has occurred for the specified generation of objects.</summary>
        /// <param name="generation">The generation of objects for which the garbage collection count is to be determined.(Valid range: 0-2)</param>
        /// <returns>The number of times garbage collection has occurred for the specified generation since the process was started.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="generation">generation</paramref> is less than 0.</exception>
        public static int CollectionCount(int generation)
        {
            return GC.CollectionCount(generation);
        }


        public static long GetTotalMemory()
        {
            return GC.GetTotalMemory(false);
        }


        public static GCConfiguration GetConfig()
        {
            return new GCConfiguration
            {
                IsServerGC = GCSettings.IsServerGC,
                LargeObjectHeapCompactionMode = GCSettings.LargeObjectHeapCompactionMode
            };
        }
    }
}
