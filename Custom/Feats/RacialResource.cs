using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class RacialResource : BaseFeat
	{
		public override string Name{ get{ return "Racial Resource"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.RacialResource; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You know how to find and harvest your culture's primary resource (please note " +
					"that there might be skills involved with it, so check your race's primary resource before taking this skill)."; } }
		public override string SecondDescription{ get{ return "You know how to refine your culture's primary resource."; } }
		public override string ThirdDescription{ get{ return "You know how to work your culture's primary resource."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new RacialResource()); }
		
		public RacialResource() {}
	}
}
