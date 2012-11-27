using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Linen : BaseFeat
	{
		public override string Name{ get{ return "Linen"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Linen; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.EnhancedHarvesting }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to harvest flax."; } }
		public override string SecondDescription{ get{ return "With this skill level, you will be able to refine flax into Linen."; } }
		public override string ThirdDescription{ get{ return "You will now be able to craft with Linen."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override Nation[] AllowedNations{ get{ return new Nation[]{ Nation.Khemetar }; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Linen()); }
		
		public Linen() {}
	}
}
