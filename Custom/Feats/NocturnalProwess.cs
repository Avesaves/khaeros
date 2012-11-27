using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class NocturnalProwess : BaseFeat
    {
        public override string Name { get { return "Nocturnal Prowess"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.NocturnalProwess; } }
        public override FeatCost CostLevel { get { return FeatCost.Medium; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.VampireAbilities }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "Your stat bonuses during the night are raised by 10."; } }
        public override string SecondDescription { get { return "Your stat bonuses during the night are raised by 20."; } }
        public override string ThirdDescription { get { return "Your stat bonuses during the night are raised by 30."; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription( this ); } }

        public static void Initialize() { }

        public NocturnalProwess() { }

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
    }
}
