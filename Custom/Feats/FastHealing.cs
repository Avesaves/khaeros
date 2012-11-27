using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class FastHealing : BaseFeat
	{
		public override string Name{ get{ return "Fast Healing"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.FastHealing; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.DamageIgnore }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Your body can recover from blows at a faster rate. [With the skill " +
					"you'll regain 1 hp in 2,5 seconds. If you have the Quick healer trait you will regain 1 hp in 2,0 seconds]"; } }
		public override string SecondDescription{ get{ return "[With the skill you'll regain 1 hp in 2,0 seconds. If you have the " +
					"Quick healer trait you will regain 1 hp in 1,6 seconds]"; } }
		public override string ThirdDescription{ get{ return "[With the skill you'll regain 1 hp in 1,6 seconds. If you have the " +
					"Quick healer trait you will regain 1 hp in 1,42 seconds]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new FastHealing()); }
		
		public FastHealing() {}
	}
}
