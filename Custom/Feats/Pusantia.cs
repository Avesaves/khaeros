using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Pusantia : BaseFeat
	{
		public override string Name{ get{ return "Pusantia"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Pusantia; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.HerbalGathering }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to harvest Pusantia Root."; } }
		public override string SecondDescription{ get{ return "With this skill level, you will be able to refine Pusantia Root."; } }
		public override string ThirdDescription{ get{ return "You will now be able to enhance potions with Pusantia."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }

		public override Nation[] AllowedNations{ get{ return new Nation[]{ Nation.Northern, Nation.Southern, Nation.Western }; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Pusantia()); }
		
		public Pusantia() {}
	}
}
