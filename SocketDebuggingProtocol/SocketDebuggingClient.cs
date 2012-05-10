using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace SocketDebuggingProtocol
{
    public partial class SocketDebuggingClient
    {
        private TcpClient TcpClient;
        private Timer Timer;
        public MachineState LastKnownMachineState;

        public void Connect(IPEndPoint EndPoint)
        {
            TcpClient = new TcpClient();
            TcpClient.Connect(EndPoint);
        }

        public void Disconnect()
        {
            if (TcpClient.Connected)
            {
                byte[] packet = new byte[] { (byte)PacketID.Disconnect, 0x00, 0x00 };
                TcpClient.GetStream().Write(packet, 0, packet.Length);
                TcpClient.Close();
            }
            else
                throw new InvalidOperationException("The socket is not connected.");
        }

        public EmulatorDetails GetEmulatorDetails()
        {
            if (!TcpClient.Connected)
                throw new InvalidOperationException("The socket is not connected.");

            SendPacket(new byte[0], PacketID.EmulatorInformation);

            byte b = ReadByte(TcpClient.GetStream());
            uint length = ReadUInt32(TcpClient.GetStream());

            byte protocolVersion =      ReadByte(TcpClient.GetStream());
            string emulatorName =       ReadString(TcpClient.GetStream());
            string emulatorVersion =    ReadString(TcpClient.GetStream());
            return new EmulatorDetails()
            {
                ProtocolVersion = protocolVersion,
                EmulatorName = emulatorName,
                EmulatorVersion = emulatorVersion
            };
        }

        public MachineState GetMachineState()
        {
            if (!TcpClient.Connected)
                throw new InvalidOperationException("The socket is not connected.");

            SendPacket(new byte[0], PacketID.GetMachineState);

            MachineState mstate = new MachineState();
            byte b = ReadByte(TcpClient.GetStream());
            uint length = ReadUInt32(TcpClient.GetStream());

            mstate.IsRunning = ReadBool(TcpClient.GetStream());
            mstate.RegisterA = ReadUInt16(TcpClient.GetStream());
            mstate.RegisterB = ReadUInt16(TcpClient.GetStream());
            mstate.RegisterC = ReadUInt16(TcpClient.GetStream());
            mstate.RegisterX = ReadUInt16(TcpClient.GetStream());
            mstate.RegisterY = ReadUInt16(TcpClient.GetStream());
            mstate.RegisterZ = ReadUInt16(TcpClient.GetStream());
            mstate.RegisterI = ReadUInt16(TcpClient.GetStream());
            mstate.RegisterJ = ReadUInt16(TcpClient.GetStream());
            mstate.RegisterPC = ReadUInt16(TcpClient.GetStream());
            mstate.RegisterSP = ReadUInt16(TcpClient.GetStream());
            mstate.RegisterEX = ReadUInt16(TcpClient.GetStream());
            mstate.RegisterIA = ReadUInt16(TcpClient.GetStream());
            mstate.ClockSpeed = ReadUInt32(TcpClient.GetStream());
            mstate.CyclesSinceReset = ReadUInt64(TcpClient.GetStream());
            mstate.QueuedInterrupts = ReadByte(TcpClient.GetStream());

            LastKnownMachineState = mstate;
            return mstate;
        }

        public void SetMachineState(MachineState State)
        {
            if (!TcpClient.Connected)
                throw new InvalidOperationException("The socket is not connected.");

            byte[] packet = MakeBoolean(State.IsRunning)
                .Concat(MakeUShort(State.RegisterA))
                .Concat(MakeUShort(State.RegisterB))
                .Concat(MakeUShort(State.RegisterC))
                .Concat(MakeUShort(State.RegisterX))
                .Concat(MakeUShort(State.RegisterY))
                .Concat(MakeUShort(State.RegisterZ))
                .Concat(MakeUShort(State.RegisterI))
                .Concat(MakeUShort(State.RegisterJ))
                .Concat(MakeUShort(State.RegisterPC))
                .Concat(MakeUShort(State.RegisterSP))
                .Concat(MakeUShort(State.RegisterEX))
                .Concat(MakeUShort(State.RegisterIA))
                .Concat(MakeUInt(State.ClockSpeed))
                .ToArray();
            SendPacket(packet, PacketID.SetMachineState);

            byte resposne = ReadByte(TcpClient.GetStream());
            ReadUInt32(TcpClient.GetStream());
            if (resposne == (byte)PacketID.ErrorProcessingRequest)
            {
                string error = ReadString(TcpClient.GetStream());
                throw new Exception(error);
            }
            LastKnownMachineState = State;
        }
    }
}
