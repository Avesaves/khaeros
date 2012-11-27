using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class RangedEffect : BaseFeat
	{
		public override string Name{ get{ return "Ranged Effect"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.RangedEffect; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Magery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to create custom spells with range up to 5."; } }
		public override string SecondDescription{ get{ return "This skill will allow you to create custom spells with range up to 10."; } }
		public override string ThirdDescription{ get{ return "This skill will allow you to create custom spells with range up to 15."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new RangedEffect()); }
		
		public RangedEffect() {}
	}
}
