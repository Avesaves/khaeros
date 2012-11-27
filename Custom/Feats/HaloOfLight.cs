using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class HaloOfLight : BaseFeat
	{
		public override string Name{ get{ return "Halo of Light"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.HaloOfLight; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Bless }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This spell allows you to place a halo of light upon a creature for some time. " +
					"[The spell gives you a halo, illuminating the area around you for 2 minutes, costs 5 mana]"; } }
		public override string SecondDescription{ get{ return "[The spell gives you night vision and a halo, illuminating the area around you for " +
					"4 minutes, costs 10 mana]"; } }
		public override string ThirdDescription{ get{ return "[The spell gives you and your allies within five tiles night vision and a halo, " +
					"illuminating the area around you for 6 minutes, costs 15 mana]"; } }

		public override string FirstCommand{ get{ return ".HaloOfLight | .HaloOfLight 1"; } }
		public override string SecondCommand{ get{ return ".HaloOfLight | .HaloOfLight 2"; } }
		public override string ThirdCommand{ get{ return ".HaloOfLight | .HaloOfLight 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new HaloOfLight()); }
		
		public HaloOfLight() {}
	}
}
