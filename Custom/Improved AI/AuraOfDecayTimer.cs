using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Engines;
using System.Collections;
using System.Collections.Generic;

namespace Server.Misc
{
    public class AuraOfDecayTimer : Timer
    {
        public static bool CanUseAura( Mobile mob )
        {
            if( mob == null || mob.Deleted || !mob.Alive || mob.Blessed || !mob.Warmode )
                return false;

            return true;
        }

        public static bool CanBeHarmedByAura( Mobile owner, Mobile mob, AuraType type )
        {
            if( BaseAI.AreAllies(owner, mob) || mob == null || mob.Deleted || !mob.Alive || mob.Blessed )
                return false;

            return true;
        }

        private BaseCreature owner;

        public AuraOfDecayTimer( BaseCreature creature )
            : base( TimeSpan.FromSeconds( 2.0 ) )
        {
            if( !CanUseAura( creature ) )
            {
                Stop();
                return;
            }

            owner = creature;
        }

        protected override void OnTick()
        {
            if( !CanUseAura( owner ) )
                return;

            int damage = (int)( owner.Fame * 0.0005 );
            int anim;
            List<Mobile> toDamage = new List<Mobile>();

            foreach( Mobile mob in owner.GetMobilesInRange( 3 ) )
                if( CanBeHarmedByAura( owner, mob, owner.AuraType ) )
                    toDamage.Add( mob );

            for( int i = 0; i < toDamage.Count; i++ )
            {
                anim = 14628;

                if( Utility.RandomBool() )
                    anim = 14613;

                Mobile mob = toDamage[i];
                mob.Damage( (int)( Math.Max( 5, Math.Min( 25, damage ) ) ), owner );
                mob.FixedParticles( anim, 10, 15, 9950, 0, 0, EffectLayer.Waist );
            }

            if( CanUseAura( owner ) )
            {
                owner.AuraTimer = new AuraOfDecayTimer( owner );
                owner.AuraTimer.Start();
            }
        }
    }
}
