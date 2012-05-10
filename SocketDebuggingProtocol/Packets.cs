using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace SocketDebuggingProtocol
{
    public partial class SocketDebuggingClient
    {
        private void SendPacket(byte[] packet, PacketID ID)
        {
            packet = new byte[] { (byte)ID }
                .Concat(MakeUInt((uint)packet.Length))
                .Concat(packet).ToArray();
            TcpClient.GetStream().Write(packet, 0, packet.Length);
        }

        public void WriteBool(Stream stream, bool value)
        {
            stream.WriteByte((byte)(value ? 1 : 0));
        }

        public void WriteByte(Stream stream, byte value)
        {
            stream.WriteByte(value);
        }

        public void WriteUInt16(Stream stream, ushort integer)
        {
            byte[] b = BitConverter.GetBytes((ushort)IPAddress.HostToNetworkOrder((short)integer));
            stream.Write(b, 0, b.Length);
        }

        public void WriteUInt32(Stream stream, uint integer)
        {
            byte[] b = BitConverter.GetBytes((uint)IPAddress.HostToNetworkOrder(integer));
            stream.Write(b, 0, b.Length);
        }

        public void WriteUInt64(Stream stream, ulong integer)
        {
            byte[] b = BitConverter.GetBytes((ulong)IPAddress.HostToNetworkOrder((long)integer));
            stream.Write(b, 0, b.Length);
        }

        public void WriteString(Stream stream, string value)
        {
            WriteUInt16(stream, (ushort)value.Length);
            byte[] b = Encoding.ASCII.GetBytes(value);
            stream.Write(b, 0, b.Length);
        }

        public bool ReadBool(Stream stream)
        {
            return stream.ReadByte() == 1;
        }

        public byte ReadByte(Stream stream)
        {
            return (byte)stream.ReadByte();
        }

        public ushort ReadUInt16(Stream stream)
        {
            byte[] buffer = new byte[sizeof(ushort)];
            stream.Read(buffer, 0, sizeof(ushort));
            return (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 0));
        }

        public uint ReadUInt32(Stream stream)
        {
            byte[] buffer = new byte[sizeof(uint)];
            stream.Read(buffer, 0, sizeof(uint));
            return (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, 0));
        }

        public uint ReadUInt64(Stream stream)
        {
            byte[] buffer = new byte[sizeof(uint)];
            stream.Read(buffer, 0, sizeof(uint));
            return (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, 0));
        }

        public string ReadString(Stream stream)
        {
            ushort length = ReadUInt16(stream);
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            return Encoding.ASCII.GetString(buffer);
        }

        #region Mostly copied from LibMinecraft

        /// <summary>
        /// Makes the string.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] MakeString(String msg)
        {
            short len = IPAddress.HostToNetworkOrder((short)msg.Length);
            byte[] a = BitConverter.GetBytes(len);
            byte[] b = Encoding.ASCII.GetBytes(msg);
            return a.Concat(b).ToArray();
        }

        /// <summary>
        /// Makes the int.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] MakeInt(int i)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
        }

        public static byte[] MakeUInt(uint i)
        {
            return MakeInt((int)i);
        }

        /// <summary>
        /// Makes the short.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] MakeShort(short i)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
        }

        public static byte[] MakeUShort(ushort i)
        {
            return MakeShort((short)i);
        }

        /// <summary>
        /// 
        /// </summary>
        static byte[] BooleanArray = new byte[] { 0 };
        /// <summary>
        /// Makes the boolean.
        /// </summary>
        /// <param name="b">if set to <c>true</c> [b].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] MakeBoolean(Boolean b)
        {
            BooleanArray[0] = (byte)(b ? 1 : 0);
            return BooleanArray;
        }

        #endregion
    }
}
