using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Hideout : BaseFeat
	{
		public override string Name{ get{ return "Hideout"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Hideout; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Camping }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to claim a small hideout somewhere in the wilderness. It counts as a house, so " +
					"you will not be able to keep both a hideout and a house. Contact an Overseer when you have picked a location " +
					"and they will check if it is available and handle the creation of the hideout."; } }
		public override string SecondDescription{ get{ return "Increases the hideout's size to medium if possible."; } }
		public override string ThirdDescription{ get{ return "Increases the hideout's size to large if possible."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Hideout()); }
		
		public Hideout() {}
	}
}
