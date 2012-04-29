using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketDebuggingProtocol
{
    public enum PacketID : byte
    {
        EmulatorInformation =       0x00,
        GetMachineState =           0x01,
        SetMachineState =           0x02,
        GetConnectedDevices =       0x03,
        GetDeviceState =            0x04,
        SetDeviceState =            0x05,
        GetMemorySector =           0x06,
        SetMemorySector =           0x07,
        EnableMemoryBroadcast =     0x08,
        DisableMemoryBroadcast =    0x09,
        SetEmulationState =         0x0A,
        Breakpoint =                0x0B,
        Watchpoint =                0x0C,
        StepInto =                  0x0D,
        StepOver =                  0x0E,
        TriggerInterrupt =          0x0F,
        RequestConfirmation =       0x20,
        ErrorProcessingRequest =    0xFF,
    }
}
