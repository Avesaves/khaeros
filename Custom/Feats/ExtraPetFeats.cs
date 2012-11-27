using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ExtraPetFeats : BaseFeat
	{
		public override string Name{ get{ return "Extra Pet Feats"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ExtraPetFeats; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.PetFeats }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Adds a random chance to add up to 3 skill points, in addition to those granted by Pet Feats."; } }
		public override string SecondDescription{ get{ return "The number of skill points increases to 6."; } }
		public override string ThirdDescription{ get{ return "The number of skill points increases to 9"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ExtraPetFeats()); }
		
		public ExtraPetFeats() {}
	}
}
