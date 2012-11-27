using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class LowerSideEffects : BaseFeat
	{
		public override string Name{ get{ return "Lower Side Effects"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.LowerSideEffects; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Alchemy }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.EnhancePotion }; } }
		
		public override string FirstDescription{ get{ return "This skill will reduce the side effects of your potions. [-10% side effects]"; } }
		public override string SecondDescription{ get{ return "[-20% side effects]"; } }
		public override string ThirdDescription{ get{ return "[-30% side effects]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new LowerSideEffects()); }
		
		public LowerSideEffects() {}
	}
}
