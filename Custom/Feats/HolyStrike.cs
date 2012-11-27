using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class HolyStrike : BaseFeat
	{
		public override string Name{ get{ return "Holy Strike"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.HolyStrike; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.HealWounds }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This power will grant you a melee attack. If it connects, you or an adjacent will " +
					"be healed. [Heals for 10, works as a combat maneuver]"; } }
		public override string SecondDescription{ get{ return "[Heals for 20, works as a combat maneuver]"; } }
		public override string ThirdDescription{ get{ return "[Heals for 30, works as a combat maneuver]"; } }

		public override string FirstCommand{ get{ return ".HolyStrike"; } }
		public override string SecondCommand{ get{ return ".HolyStrike"; } }
		public override string ThirdCommand{ get{ return ".HolyStrike"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new HolyStrike()); }
		
		public HolyStrike() {}
	}
}
