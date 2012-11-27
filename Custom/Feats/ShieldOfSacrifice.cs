using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ShieldOfSacrifice : BaseFeat
	{
		public override string Name{ get{ return "Shield of Sacrifice"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ShieldOfSacrifice; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Faith }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "With this spell, you will be able to shield a creature and channel some of the " +
					"damage it receives to you. [As long as you stay within 12 tiles of the target, 25% of damage will be redirected. Costs 10 " +
					"mana and lasts for 3 minutes]"; } }
		public override string SecondDescription{ get{ return "[As long as you stay within 12 tiles of the target, 50% of damage will be " +
					"redirected.Costs 20 mana and lasts for 6 minutes]"; } }
		public override string ThirdDescription{ get{ return "[As long as you stay within 12 tiles of the target, 75% of damage will be redirected. " +
					"Costs 30 mana and lasts for 9 minutes]"; } }

		public override string FirstCommand{ get{ return ".ShieldOfSacrifice | .ShieldOfSacrifice 1"; } }
		public override string SecondCommand{ get{ return ".ShieldOfSacrifice | .ShieldOfSacrifice 2"; } }
		public override string ThirdCommand{ get{ return ".ShieldOfSacrifice | .ShieldOfSacrifice 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ShieldOfSacrifice()); }
		
		public ShieldOfSacrifice() {}
	}
}
