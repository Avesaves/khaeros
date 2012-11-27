using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;

namespace Server.FeatInfo
{
    public class Terror : BaseFeat
    {
        public override string Name { get { return "Terror"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Terror; } }
        public override FeatCost CostLevel { get { return FeatCost.Medium; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.VampireAbilities }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "You can use your monstrous gaze to paralyze creatures. [lasts 1 second]"; } }
        public override string SecondDescription { get { return "[lasts 2 seconds]"; } }
        public override string ThirdDescription { get { return "[lasts 3 seconds]"; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription( this ); } }

        public static void Initialize() { }

        public Terror() { }

        public override bool MeetsOurRequirements( PlayerMobile m )
        {
            if( !m.IsVampire )
                return false;

            return base.MeetsOurRequirements( m );
        }

        public override bool ShouldDisplayTo( PlayerMobile m )
        {
            if( !m.IsVampire )
                return false;

            return base.ShouldDisplayTo( m );
        }

        public static void HandleTerror( PlayerMobile m )
        {
            if( m.Feats.GetFeatLevel( FeatList.Terror ) < 1 )
            {
                m.SendMessage( "You need the first level of Terror to use this ability." );
                return;
            }

            if( m.Combatant != null && m.InRange( m.Combatant.Location, 6 ) )
                TerrorTarget.TryTerror( m, m.Combatant );

            else
                m.Target = new TerrorTarget();
        }

        private class TerrorTarget : Target
        {
            public TerrorTarget()
                : base( 6, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
                if( from == null || targeted == null || !( from is PlayerMobile ) )
                    return;

                if( !from.Alive || from.Paralyzed || !( targeted is Mobile ) )
                    return;

                TryTerror( (PlayerMobile)from, (Mobile)targeted );
            }

            public static void TryTerror( Mobile from, Mobile mob )
            {
                if( from == null || mob == null || !( from is PlayerMobile ) || !from.InRange( mob, 6 ) )
                    return;

                PlayerMobile vamp = from as PlayerMobile;

                if( vamp.NextFeatUse > DateTime.Now )
                    vamp.SendMessage( "It is too early to use another ability." );

                else if( vamp.BPs < 1 )
                    vamp.SendMessage( "You lack enough blood points to use this ability." );

                else
                {
                    if( Utility.RandomMinMax( 1, 100 ) > mob.EnergyResistance )
                    {
                        mob.Emote( "*is paralyzed in fear of " + vamp.Name + vamp.GetPossessive() + " gaze*" );
                        Engines.XmlSpawner2.XmlAttach.AttachTo( mob, new Engines.XmlSpawner2.XmlParalyze( ( (double)vamp.Feats.GetFeatLevel( FeatList.Terror ) ) ) );
                    }

                    else
                        mob.Emote( "*resisted " + vamp.Name + vamp.GetPossessive() + " monstrous gaze*" );

                    vamp.BPs--;
                    vamp.PlaySound( 581 );
                    vamp.NextFeatUse = DateTime.Now + TimeSpan.FromSeconds( 60 - vamp.Level );
                    vamp.RevealingAction();

                    Engines.XmlSpawner2.CombatSystemAttachment csa = Engines.XmlSpawner2.CombatSystemAttachment.GetCSA( vamp );
                    csa.Interrupted( true );

                    if( mob is BaseCreature )
                        ( (BaseCreature)mob ).AggressiveAction( vamp );
                }
            }
        }
    }
}
