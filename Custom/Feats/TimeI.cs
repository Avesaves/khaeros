using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class TimeI : BaseFeat
	{
		public override string Name{ get{ return "Time I"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.TimeI; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Meditation }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to cast time-based spells from ancient scrolls you may " +
					"find. It will also increase your mana regeneration rate."; } }
		public override string SecondDescription{ get{ return "Improved effect."; } }
		public override string ThirdDescription{ get{ return "Improved effect."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new TimeI()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m )
		{
			if( !m.CanBeMage )
				return false;
			
			return base.MeetsOurRequirements( m );
		}
		
		public TimeI() {}
	}
}
