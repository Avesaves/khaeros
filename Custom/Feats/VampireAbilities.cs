using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class VampireAbilities : BaseFeat
	{
		public override string Name{ get{ return "Vampire Abilities"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.VampireAbilities; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Obfuscate, FeatList.Protean, FeatList.NocturnalProwess, 
                                            FeatList.Celerity, FeatList.Awe, FeatList.Terror, FeatList.ControlUndead,
                                            FeatList.Feeding, FeatList.Daywalker, FeatList.Shapeshift }; } }
		
		public override string FirstDescription{ get{ return ""; } }
		public override string SecondDescription{ get{ return ""; } }
		public override string ThirdDescription{ get{ return ""; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }

        public static void Initialize() { }

        public VampireAbilities() { }

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
