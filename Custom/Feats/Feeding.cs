using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Feeding : BaseFeat
    {
        public override string Name { get { return "Feeding"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Feeding; } }
        public override FeatCost CostLevel { get { return FeatCost.Medium; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.VampireAbilities }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "You may use your bite attack to feed on a stunned opponent."; } }
        public override string SecondDescription { get { return "You may use your bite attack to feed on a stunned opponent, dealing some damage."; } }
        public override string ThirdDescription { get { return "You may use your bite attack to feed on a stunned opponent, dealing more damage."; } }

        public override string FirstCommand { get { return ".vp feed"; } }
        public override string SecondCommand { get { return ".vp feed"; } }
        public override string ThirdCommand { get { return ".vp feed"; } }

        public override string FullDescription { get { return GetFullDescription( this ); } }

        public static void Initialize() { }

        public Feeding() { }

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
