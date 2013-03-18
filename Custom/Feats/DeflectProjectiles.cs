using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class DeflectProjectiles : BaseFeat
	{
		public override string Name{ get{ return "Deflect Projectiles"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.DeflectProjectiles; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.ShieldMastery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You know how to hide behind your shield to prevent projectiles from hitting " +
					"you. [10% chance to deflect projectiles]"; } }
		public override string SecondDescription{ get{ return "[20% chance to deflect projectiles]"; } }
		public override string ThirdDescription{ get{ return "[30% chance to deflect projectiles]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new DeflectProjectiles()); }
		
		public DeflectProjectiles() {}
	}
}
