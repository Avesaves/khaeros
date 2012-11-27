using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class EnhancePotion : BaseFeat
	{
		public override string Name{ get{ return "Enhance Potion"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.EnhancePotion; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.LowerSideEffects }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to enhance the power of your potions. [+20% more effect]"; } }
		public override string SecondDescription{ get{ return "[+40% more effect]"; } }
		public override string ThirdDescription{ get{ return "[+60% more effect]"; } }
		
		public override string FirstCommand{ get{ return ".EnhancePotion"; } }
		public override string SecondCommand{ get{ return ".EnhancePotion"; } }
		public override string ThirdCommand{ get{ return ".EnhancePotion"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new EnhancePotion()); }
		
		public EnhancePotion() {}
	}
}
