using System;


namespace Shuvava.Extensions.Metrics.Models
{
    public struct ProcessSystemUsage
    {
        /// <summary>
        /// system.cpu: Total % Used
        /// </summary>
        public double TotalCpuUsed { get; set; }
        /// <summary>
        /// system.cpu: User % Used
        /// </summary>
        public double PrivilegedCpuUsed { get; set; }
        /// <summary>
        /// system.cpu: User % Used
        /// </summary>
        public double UserCpuUsed{ get; set; }
        public double TotalCpuUsedInMilliseconds { get; set; }
        /// <summary>
        /// system.memory: Working Set
        /// </summary>
        public long WorkingSet{ get; set; }
        /// <summary>
        /// system.memory: Non-Paged System Memory
        /// </summary>
        public long NonPagedSystemMemory{ get; set; }
        /// <summary>
        /// system.memory: Paged Memory
        /// </summary>
        public long PagedMemory{ get; set; }
        /// <summary>
        /// system.memory: System Memory
        /// </summary>
        public long PagedSystemMemory{ get; set; }
        /// <summary>
        /// system.memory: Private Memory
        /// </summary>
        public long PrivateMemory{ get; set; }
        /// <summary>
        /// system.memory: Virtual Memory
        /// </summary>
        public long VirtualMemoryMemory{ get; set; }

        public int HandleCount { get; set; }
        public int ThreadCount { get; set; }
        public DateTime ProcessStartTime { get; set; }
    }
}
