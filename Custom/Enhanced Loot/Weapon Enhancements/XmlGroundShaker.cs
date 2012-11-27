using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Engines;

namespace Server.Engines.XmlSpawner2
{
    public class XmlGroundShaker : XmlAttachment
    {
        private int m_Damage = 0;
        private int m_Squares = 0;
        private int m_Chance = 0;

        [CommandProperty( AccessLevel.GameMaster )]
        public int Damage { get { return m_Damage; } set { m_Damage = Math.Max( 0, value ); } }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Squares { get { return m_Squares; } set { m_Squares = Math.Max( 0, value ); } }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Chance { get { return m_Chance; } set { m_Chance  = Math.Min( 100, Math.Max( 0, value ) ); } }

        public XmlGroundShaker( ASerial serial )
            : base( serial )
        {
        }

        [Attachable]
        public XmlGroundShaker()
            : this( 0, 0, 0 )
        {
        }

        [Attachable]
        public XmlGroundShaker( int damage, int squares, int chance )
        {
            Damage = damage;
            Squares = squares;
            m_Chance = chance;
        }

        public static void PushAdjacentTargets( Mobile attacker, int damage, int squares )
        {
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;

            if( attacker.Direction == Direction.Up )
            {
                x1 = -1;
                y2 = -1;
            }

            else if( attacker.Direction == Direction.North )
            {
                x1 = -1;
                y1 = -1;
                x2 = 1;
                y2 = -1;
            }

            else if( attacker.Direction == Direction.Right )
            {
                y1 = -1;
                x2 = 1;
            }

            else if( attacker.Direction == Direction.East )
            {
                x1 = 1;
                y1 = -1;
                x2 = 1;
                y2 = 1;
            }

            else if( attacker.Direction == Direction.Down )
            {
                x1 = 1;
                y2 = 1;
            }

            else if( attacker.Direction == Direction.South )
            {
                x1 = 1;
                y1 = 1;
                x2 = -1;
                y2 = 1;
            }

            else if( attacker.Direction == Direction.Left )
            {
                x1 = -1;
                y2 = 1;
            }

            else if( attacker.Direction == Direction.West )
            {
                x1 = -1;
                y1 = -1;
                x2 = -1;
                y2 = 1;
            }

            Point2D loc1 = new Point2D( attacker.X + x1, attacker.Y + y1 );
            Point2D loc2 = new Point2D( attacker.X + x2, attacker.Y + y2 );
            List<Mobile> list1 = new List<Mobile>();
            List<Mobile> list2 = new List<Mobile>();

            foreach( Mobile m in attacker.GetMobilesInRange( 1 ) )
            {
                if( m.Map == attacker.Map && m.Alive && !m.Blessed && attacker.CanSee( m ) && attacker.InLOS( m ) && !BaseAI.AreAllies( attacker, m ) )
                {
                    if( ( m.X == loc1.X && m.Y == loc1.Y ) )
                        list1.Add( m );

                    else if( ( m.X == loc2.X && m.Y == loc2.Y ) )
                        list2.Add( m );
                }
            }

            foreach( Mobile m in list1 )
                PushMobile( attacker, m, x1, y1, damage, squares );

            foreach( Mobile m in list2 )
                PushMobile( attacker, m, x2, y2, damage, squares );
        }

        public static void PushMobile( Mobile attacker, Mobile defender, int x, int y, int damage, int squares )
        {
            Point3D loc = new Point3D( defender.X + x, defender.Y + y, defender.Z );

            if( squares > 0 && defender.Map.CanFit( loc, 16 ) && defender.InLOS( loc ) )
            {
                defender.Location = loc;
                defender.FixedParticles( 8902, 10, 10, 9950, 0, 0, EffectLayer.Waist );
            }

            else if( squares > 1 )
            {
                PushMobile( attacker, defender, x, y, damage, squares - 1 );
                return;
            }

            if( damage > 0 )
                defender.Damage( damage, attacker );
        }

        public override void OnWeaponHit( Mobile attacker, Mobile defender, BaseWeapon weapon, int HitsGiven )
        {
            if( attacker == null || defender == null || Chance < 1 || ( Damage < 1 && Squares < 1 ) )
                return;

            if( Chance >= Utility.RandomMinMax( 0, 100 ) )
                PushAdjacentTargets( attacker, Damage, Squares );
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 );

            writer.Write( (int)m_Damage );
            writer.Write( (int)m_Squares );
            writer.Write( (int)m_Chance );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            m_Damage = reader.ReadInt();
            m_Squares = reader.ReadInt();
            m_Chance = reader.ReadInt();
        }
    }
}
