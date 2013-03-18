using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class BleedingStrike : BaseFeat
	{
		public override string Name{ get{ return "Bleeding Strike"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.BleedingStrike; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.ExoticWeaponry }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Being well-trained in anatomy, the assassin is able to strike his opponent in a " +
					"vital area, causing them to bleed profusely. [Makes the victim bleed for a short duration, damage increases per level. " +
					"This attack cannot be used with blunt or ranged weapons.]"; } }
		public override string SecondDescription{ get{ return "Improved damage."; } }
		public override string ThirdDescription{ get{ return "Improved damage."; } }

		public override string FirstCommand{ get{ return ".BleedingStrike"; } }
		public override string SecondCommand{ get{ return ".BleedingStrike"; } }
		public override string ThirdCommand{ get{ return ".BleedingStrike"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new BleedingStrike()); }
		
		public BleedingStrike() {}
	}
}
