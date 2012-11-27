using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class ControlUndead : BaseFeat
    {
        public override string Name { get { return "Control Undead"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.ControlUndead; } }
        public override FeatCost CostLevel { get { return FeatCost.Medium; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.VampireAbilities }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "Allows you to control lesser undead."; } }
        public override string SecondDescription { get { return "Allows you to control stronger undead."; } }
        public override string ThirdDescription { get { return "Allows you to control any undead."; } }

        public override string FirstCommand { get { return "<creature's name> obey"; } }
        public override string SecondCommand { get { return "<creature's name> obey"; } }
        public override string ThirdCommand { get { return "<creature's name> obey"; } }

        public override string FullDescription { get { return GetFullDescription( this ); } }

        public static void Initialize() { }

        public ControlUndead() { }

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
