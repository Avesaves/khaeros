using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class FlurryOfBlows : BaseFeat
	{
		public override string Name{ get{ return "Flurry of Blows"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.FlurryOfBlows; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Tactics }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are able to move quicker in combat, sacrificing damage " +
					"for speed and accuracy. [-3% dmg +5% Hit Chance +5% Weapon Speed]"; } }
		public override string SecondDescription{ get{ return "[-6% dmg +10% Hit Chance +10% Weapon Speed]"; } }
		public override string ThirdDescription{ get{ return "[-9% dmg +15% Hit Chance +15% Weapon Speed]"; } }

		public override string FirstCommand{ get{ return ".FlurryOfBlows"; } }
		public override string SecondCommand{ get{ return ".FlurryOfBlows"; } }
		public override string ThirdCommand{ get{ return ".FlurryOfBlows"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new FlurryOfBlows()); }
		
		public FlurryOfBlows() {}
	}
}
