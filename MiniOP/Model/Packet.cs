using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniOP.Model
{
    public class Packet
    {
        public const int STXSIZE = 1;
        public const int IDSIZE = 3;
        public const int DATASIZE = 5;
        public const int CHECKSUMSIZE = 1;
        public const int ETXSIZE = 1;
        public const int PACKETSIZE = STXSIZE + IDSIZE + DATASIZE + CHECKSUMSIZE + ETXSIZE;

        public const int STXINDEX = 0;
        public const int IDINDEX = STXINDEX + STXSIZE;
        public const int DATAINDEX = IDINDEX + IDSIZE;
        public const int CHECKSUMINDEX = DATAINDEX + DATASIZE;
        public const int ETXINDEX = CHECKSUMINDEX + CHECKSUMSIZE;

        private readonly byte[] _packet = new byte[PACKETSIZE];

        public void SetPacket(byte[] receive)
        {
            receive.CopyTo(_packet, 0);
        }

        public byte[] GetPacket()
        {
            return _packet;
        }

        public static byte MakeChecksum(byte[] packet)
        {
            byte checkSum = 0;

            for (int i = IDINDEX; i < CHECKSUMINDEX; i++)
            {
                checkSum += packet[i];
            }

            return checkSum;
        }

        public static byte[] MakePacket(string id, string data)
        {
            var packet = new byte[PACKETSIZE];
            packet[STXINDEX] = 0x02;

            Array.Copy(Encoding.Default.GetBytes(id), 0, packet, IDINDEX, IDSIZE);

            Array.Copy(Encoding.Default.GetBytes(data), 0, packet, DATAINDEX, DATASIZE);

            packet[CHECKSUMINDEX] = MakeChecksum(packet);
            packet[ETXINDEX] = 0x03;

            return packet;
        }
    }
}
