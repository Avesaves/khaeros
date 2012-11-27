using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Obfuscate : BaseFeat
	{
		public override string Name{ get{ return "Obfuscate"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Obfuscate; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.VampireAbilities }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are harder to detect and track."; } }
		public override string SecondDescription{ get{ return "Alertness' revealing area is 1 tile smaller against you."; } }
		public override string ThirdDescription{ get{ return "You have a chance of hiding in plain sight, based on the distance your enemies " +
            " are standing from you and their Detect Hidden and Alertness scores."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }

        public static void Initialize() { }

        public Obfuscate() { }

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
