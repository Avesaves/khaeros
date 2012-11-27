using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Faith : BaseFeat
	{
		public override string Name{ get{ return "Faith"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Faith; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Faith }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ FeatList.ShieldOfSacrifice }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Invocation }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Meditation, FeatList.HealWounds, FeatList.InflictWounds, 
				FeatList.Curse, FeatList.Bless, FeatList.DivineConsecration, FeatList.AuraOfProtection, FeatList.ConsecrateItem, 
				FeatList.ShieldOfSacrifice, FeatList.Compassion, FeatList.Humility, FeatList.Justice }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Faith skill, which will " +
					"give you access to more faith-based powers, as well as the Shield of Sacrifice spell. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return ".ShieldOfSacrifice"; } }
		public override string SecondCommand{ get{ return ".ShieldOfSacrifice"; } }
		public override string ThirdCommand{ get{ return ".ShieldOfSacrifice"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Faith()); }
		
		public Faith() {}
	}
}
