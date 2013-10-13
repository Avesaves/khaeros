using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class DivineConsecration : BaseFeat
	{
		public override string Name{ get{ return "Divine Consecration"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.DivineConsecration; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Faith }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.SummonProtector }; } }
		
		public override string FirstDescription{ get{ return "This power blesses your weapon."; } }
		public override string SecondDescription{ get{ return "Improved effect."; } }
		public override string ThirdDescription{ get{ return "Improved effect."; } }

		public override string FirstCommand{ get{ return ".DivineConsecration | .DivineConsecration 1"; } }
		public override string SecondCommand{ get{ return ".DivineConsecration | .DivineConsecration 2"; } }
		public override string ThirdCommand{ get{ return ".DivineConsecration | .DivineConsecration 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new DivineConsecration()); }
		
		public DivineConsecration() {}
	}
}
