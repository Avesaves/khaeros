using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
    public class XmlLightningStrike : XmlAttachment
    {
        private int m_Damage = 0;
        private int m_Chance = 0;

        [CommandProperty( AccessLevel.GameMaster )]
        public int Damage { get { return m_Damage; } set { m_Damage = Math.Max( 0, value ); } }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Chance { get { return m_Chance; } set { m_Chance  = Math.Min( 100, Math.Max( 0, value ) ); } }

        public XmlLightningStrike( ASerial serial )
            : base( serial )
        {
        }

        [Attachable]
        public XmlLightningStrike()
            : this( 0, 0 )
        {
        }

        [Attachable]
        public XmlLightningStrike( int damage, int chance )
        {
            m_Damage = damage;
            m_Chance = chance;
        }

        public override void OnWeaponHit( Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven )
        {
            if( Damage < 1 || Chance < 1 )
                return;

            if( Chance >= Utility.RandomMinMax( 0, 100 ) )
            {
                if( Damage > 0 )
                {
                    SpellHelper.Damage( TimeSpan.Zero, defender, attacker, Damage, 0, 0, 0, 0, 100 );
                    defender.BoltEffect( 0 );
                }
            }
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 );

            writer.Write( (int)m_Damage );
            writer.Write( (int)m_Chance );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            m_Damage = reader.ReadInt();
            m_Chance = reader.ReadInt();
        }
    }
}
