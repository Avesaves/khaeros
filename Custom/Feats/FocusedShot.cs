using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class FocusedShot : BaseFeat
	{
		public override string Name{ get{ return "Focused Shot"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.FocusedShot; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.CrossbowMastery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.AimedShot }; } }
		
		public override string FirstDescription{ get{ return "This skill grants the player the ability to sacrifice firing " +
					"speed for increased accuracy and damage. [-3% speed + 5% damage +5% hit chance]"; } }
		public override string SecondDescription{ get{ return "[-6% speed + 10% damage +10% hit chance]"; } }
		public override string ThirdDescription{ get{ return "[-9% speed + 15% damage +15% hit chance]"; } }

		public override string FirstCommand{ get{ return ".FocusedShot"; } }
		public override string SecondCommand{ get{ return ".FocusedShot"; } }
		public override string ThirdCommand{ get{ return ".FocusedShot"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new FocusedShot()); }
		
		public FocusedShot() {}
	}
}
