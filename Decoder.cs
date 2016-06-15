using System.Text;

namespace Util
{
    public class Decoder
    {
        public enum KeyType
        {
            Wii,
            WiiU
        }

        private static uint CRC32(byte[] data, uint offset, uint size)
        {
            uint _value = 0xFFFFFFFF;
            uint[] table = new uint[256];
            const uint kPoly = 0xEDB88320;

            for (uint i = 0; i < 256; i++)
            {
                uint r = i;
                for (int j = 0; j < 8; j++)
                    if ((r & 1) != 0) r = (r >> 1) ^ kPoly;
                    else r >>= 1;
                table[i] = r;
            }

            for (uint i = 0; i < size; i++) _value = table[(byte)_value ^ data[offset + i]] ^ (_value >> 8);
            return _value ^ 0xFFFFFFFF;
        }

        public static uint MasterKey(uint serviceCode, int month, int day, KeyType kt)
        {
            if (kt == KeyType.Wii)
            {
                string num = $"{month}{day}{serviceCode.ToString().Substring(4)}";
                var crc = CRC32(Encoding.ASCII.GetBytes(num), 0, 8);
                return ((crc ^ 0xaaaa) + 0x14c1) % 100000;
            }
            return 0;
        }
    }
}
