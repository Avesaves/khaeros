using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
    public class GrappleTimer : Timer
    {
        public static bool LegalGrappling( Mobile mob )
        {
            if( mob == null || mob.Deleted || !mob.Alive || mob.Blessed )
                return false;

            return true;
        }

        private BaseCreature attacker;
        private Mobile defender;
        private int reps;

        public GrappleTimer( BaseCreature grappler, Mobile grappled, int repsRemaining )
            : base( TimeSpan.FromSeconds(2.0) )
        {
            if( !LegalGrappling( grappler ) || !LegalGrappling( grappled ) )
            {
                Stop();
                return;
            }

            attacker = grappler;
            defender = grappled;
            reps = repsRemaining;
            XmlAttach.AttachTo( attacker, new XmlParalyze( 4.0 ) );
            XmlAttach.AttachTo( defender, new XmlParalyze( 2.0 ) );
        }

        protected override void OnTick()
        {
        	if( !LegalGrappling(attacker) || !LegalGrappling(defender) )
        		return;

            Blood blood = new Blood();
            blood.ItemID = Utility.Random( 0x122A, 5 );
            Map map = defender.Map;
            bool validLocation = false;
            Point3D loc = defender.Location;

            for( int i = 0; !validLocation && i < 10; i++ )
            {
                int x = defender.X + Utility.RandomMinMax( 0, 2 ) - 1;
                int y = defender.Y + Utility.RandomMinMax( 0, 2 ) - 1;
                int z = map.GetAverageZ( x, y );

                if( validLocation = map.CanFit( x, y, defender.Z, 16, false, false ) )
                    loc = new Point3D( x, y, defender.Z );
                else if( validLocation = map.CanFit( x, y, z, 16, false, false ) )
                    loc = new Point3D( x, y, z );
            }

            blood.MoveToWorld( loc, map );
            defender.PlaySound( 0x133 );
            defender.Damage( (int)( Math.Max( 10, Math.Min( 50, ( attacker.Str / 10 ) ) ) ), attacker );

            if( attacker.MeleeAttackType != MeleeAttackType.PermanentGrapple )
                reps--;

            if( reps < 1 || !LegalGrappling( attacker ) || !LegalGrappling( defender ) )
                return;

            new GrappleTimer( attacker, defender, reps ).Start();
        }
    }
}
