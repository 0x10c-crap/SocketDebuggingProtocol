using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketDebuggingProtocol
{
    public class MachineState
    {
        public bool IsRunning { get; set; }
        public ushort RegisterA { get; set; }
        public ushort RegisterB { get; set; }
        public ushort RegisterC { get; set; }
        public ushort RegisterX { get; set; }
        public ushort RegisterY { get; set; }
        public ushort RegisterZ { get; set; }
        public ushort RegisterI { get; set; }
        public ushort RegisterJ { get; set; }
        public ushort RegisterPC { get; set; }
        public ushort RegisterSP { get; set; }
        public ushort RegisterEX { get; set; }
        public ushort RegisterIA { get; set; }
        /// <summary>
        /// In hertz
        /// </summary>
        public ushort ClockSpeed { get; set; }
        public uint CyclesSinceReset { get; set; }
        public byte QueuedInterrupts { get; set; }
    }
}
