using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocketDebuggingProtocol;
using System.Net;
using System.Globalization;

namespace TestSDP
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketDebuggingClient SDC = new SocketDebuggingClient();
            string input = "";
            while (input != "quit")
            {
                Console.Write(">");
                input = Console.ReadLine();
                if (input.StartsWith("connect "))
                {
                    string endpoint = input.Substring("connect ".Length);
                    string[] parts = endpoint.Split(':');
                    IPEndPoint ep;
                    if (parts.Length == 1)
                        ep = new IPEndPoint(IPAddress.Parse(parts[0]), 22348);
                    else
                        ep = new IPEndPoint(IPAddress.Parse(parts[0]), int.Parse(parts[1]));
                    SDC.Connect(ep);
                }
                else if (input == "disconnect")
                    SDC.Disconnect();
                else if (input == "details")
                {
                    EmulatorDetails details = SDC.GetEmulatorDetails();
                    Console.WriteLine("Protocol Version: " + details.ProtocolVersion);
                    Console.WriteLine("Emulator Name: " + details.EmulatorName);
                    Console.WriteLine("Emulator Version: " + details.EmulatorVersion);
                }
                else if (input == "getstate" || input == "unstage")
                {
                    MachineState mstate = SDC.GetMachineState();
                    Console.WriteLine("Is Running:      " + mstate.IsRunning.ToString());
                    Console.WriteLine("Register A:      0x" + mstate.RegisterA.ToString("x").ToUpper());
                    Console.WriteLine("Register B:      0x" + mstate.RegisterB.ToString("x").ToUpper());
                    Console.WriteLine("Register C:      0x" + mstate.RegisterC.ToString("x").ToUpper());
                    Console.WriteLine("Register X:      0x" + mstate.RegisterX.ToString("x").ToUpper());
                    Console.WriteLine("Register Y:      0x" + mstate.RegisterY.ToString("x").ToUpper());
                    Console.WriteLine("Register Z:      0x" + mstate.RegisterZ.ToString("x").ToUpper());
                    Console.WriteLine("Register I:      0x" + mstate.RegisterI.ToString("x").ToUpper());
                    Console.WriteLine("Register J:      0x" + mstate.RegisterJ.ToString("x").ToUpper());
                    Console.WriteLine("Register PC:     0x" + mstate.RegisterPC.ToString("x").ToUpper());
                    Console.WriteLine("Register SP:     0x" + mstate.RegisterSP.ToString("x").ToUpper());
                    Console.WriteLine("Register EX:     0x" + mstate.RegisterEX.ToString("x").ToUpper());
                    Console.WriteLine("Register IA:     0x" + mstate.RegisterIA.ToString("x").ToUpper());
                    Console.WriteLine("Clock Speed:     " + mstate.ClockSpeed + " Hz");
                    Console.WriteLine("Total Cycles:    " + mstate.CyclesSinceReset);
                    Console.WriteLine("Interrupt Queue: " + mstate.QueuedInterrupts);
                }
                else if (input == "getstage")
                {
                    Console.WriteLine("Is Running:      " + SDC.LastKnownMachineState.IsRunning.ToString());
                    Console.WriteLine("Register A:      0x" + SDC.LastKnownMachineState.RegisterA.ToString("x").ToUpper());
                    Console.WriteLine("Register B:      0x" + SDC.LastKnownMachineState.RegisterB.ToString("x").ToUpper());
                    Console.WriteLine("Register C:      0x" + SDC.LastKnownMachineState.RegisterC.ToString("x").ToUpper());
                    Console.WriteLine("Register X:      0x" + SDC.LastKnownMachineState.RegisterX.ToString("x").ToUpper());
                    Console.WriteLine("Register Y:      0x" + SDC.LastKnownMachineState.RegisterY.ToString("x").ToUpper());
                    Console.WriteLine("Register Z:      0x" + SDC.LastKnownMachineState.RegisterZ.ToString("x").ToUpper());
                    Console.WriteLine("Register I:      0x" + SDC.LastKnownMachineState.RegisterI.ToString("x").ToUpper());
                    Console.WriteLine("Register J:      0x" + SDC.LastKnownMachineState.RegisterJ.ToString("x").ToUpper());
                    Console.WriteLine("Register PC:     0x" + SDC.LastKnownMachineState.RegisterPC.ToString("x").ToUpper());
                    Console.WriteLine("Register SP:     0x" + SDC.LastKnownMachineState.RegisterSP.ToString("x").ToUpper());
                    Console.WriteLine("Register EX:     0x" + SDC.LastKnownMachineState.RegisterEX.ToString("x").ToUpper());
                    Console.WriteLine("Register IA:     0x" + SDC.LastKnownMachineState.RegisterIA.ToString("x").ToUpper());
                    Console.WriteLine("Clock Speed:     " + SDC.LastKnownMachineState.ClockSpeed + " Hz");
                    Console.WriteLine("\nUse setstate to apply staged changes to remote server.");
                }
                else if (input.StartsWith("setregister "))
                {
                    string[] parts = input.Split(' ');
                    typeof(MachineState).GetProperty("Register" + parts[1]).SetValue(SDC.LastKnownMachineState,
                        ushort.Parse(parts[2], NumberStyles.HexNumber), null);
                    Console.WriteLine("Change staged.  Use setstate to apply changes to server.");
                }
                else if (input == "stop")
                {
                    SDC.LastKnownMachineState.IsRunning = false;
                    Console.WriteLine("Change staged.  Use setstate to apply changes to server.");
                }
                else if (input == "start")
                {
                    SDC.LastKnownMachineState.IsRunning = true;
                    Console.WriteLine("Change staged.  Use setstate to apply changes to server.");
                }
                else if (input.StartsWith("clock "))
                {
                    string[] parts = input.Split(' ');
                    SDC.LastKnownMachineState.ClockSpeed = ushort.Parse(parts[1]);
                    Console.WriteLine("Change staged.  Use setstate to apply changes to server.");
                }
                else if (input == "setstate")
                {
                    try
                    {
                        SDC.SetMachineState(SDC.LastKnownMachineState);
                        Console.WriteLine("State changes applied.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
                else if (input == "quit")
                    break;
                else
                {
                    Console.WriteLine("Unknown Command.");
                }
            }
        }
    }
}
