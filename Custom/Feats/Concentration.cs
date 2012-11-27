using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Concentration : BaseFeat
	{
		public override string Name{ get{ return "Concentration"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Concentration; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Concentration }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Meditation }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.LifeII, FeatList.DeathII, FeatList.MatterII, FeatList.SpiritII, 
				FeatList.TimeII, FeatList.PrimeII, FeatList.MindII, FeatList.SpaceII, FeatList.FateII, FeatList.ForcesII }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Concentration skill, which will " +
					"lower your chance to fizzle spells when surrounded by enemies or getting struck while casting. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }
	
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Concentration()); }
		
		public Concentration() {}
	}
}
