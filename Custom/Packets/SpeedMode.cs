using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Network
{
    public sealed class SpeedMode : Packet
    {
        public static readonly Packet Walk = Packet.SetStatic( new SpeedMode( 2 ) );
        public static readonly Packet Run = Packet.SetStatic( new SpeedMode( 1 ) );
        public static readonly Packet Disabled = Packet.SetStatic( new SpeedMode( 0 ) );

        public static Packet Instantiate(int mode)
        {
            switch (mode)
            {
                default:
                case 0: return Disabled;
                case 1: return Run;
                case 2: return Walk;
            }
        }

        public SpeedMode(int mode)
            : base(0xBF)
        {
            EnsureCapacity(3);
            m_Stream.Write((short)0x26);
            m_Stream.Write((byte)mode);
        }
    }
}
