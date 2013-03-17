using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class CatchProjectiles : BaseFeat
	{
		public override string Name{ get{ return "Catch Projectiles"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.CatchProjectiles; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.UnarmedFighting }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "With your lighting quick reflexes you are able to catch projectiles thrown at " +
					"you. Only works when unarmed and without a shield. [Catch chance is 10%]"; } }
		public override string SecondDescription{ get{ return "[Catch chance is 20%]"; } }
		public override string ThirdDescription{ get{ return "[Catch chance is 30%]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new CatchProjectiles()); }
		
		public CatchProjectiles() {}
	}
}
