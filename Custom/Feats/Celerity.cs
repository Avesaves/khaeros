using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Celerity : BaseFeat
    {
        public override string Name { get { return "Celerity"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Celerity; } }
        public override FeatCost CostLevel { get { return FeatCost.Medium; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.VampireAbilities }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "Allows you to move at horseback speed on foot when not in combat. [costs 3 bps]"; } }
        public override string SecondDescription { get { return "[costs 2 bps]"; } }
        public override string ThirdDescription { get { return "[costs 1 bp]"; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription( this ); } }

        public static void Initialize() { }

        public Celerity() { }

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

        public static void HandleCelerity( PlayerMobile m )
        {
            if( m.Feats.GetFeatLevel( FeatList.Celerity ) < 1 )
                m.SendMessage( "You need the first level of Celerity to use this ability." );

            else if( m.SpeedHack )
                m.SendMessage( "You are already moving as fast as possible." );

            else if( m.BPs < Math.Max( 1, ( 4 - m.Feats.GetFeatLevel( FeatList.Celerity ) ) ) )
                m.SendMessage( "You lack enough blood points to use this ability." );

            else if( m.Warmode )
                m.SendMessage( "This command cannot be used in war mode." );

            else
            {
                m.BPs -= Math.Max( 1, ( 4 - m.Feats.GetFeatLevel( FeatList.Celerity ) ) );
                m.SendMessage( "You will be moving fast for two hours or until you enter war mode." );
                Engines.XmlSpawner2.XmlAttach.AttachTo( m, new Engines.XmlSpawner2.XmlSpeedHack( 20, 7200 ) );
            }
        }
    }
}
