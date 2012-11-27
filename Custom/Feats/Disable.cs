using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Disable : BaseFeat
	{
		public override string Name{ get{ return "Disable"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Disable; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are able to hit vital spots on your opponent's body, temporarily disabling " +
					"one of their limbs. [+2.5% Hit Chance, +3 base damage, chance to hit is the difference between the tactics skill " +
					"or 20%, whichever is higher. Leg hit=cannot run, right arm=cannot attack, left arm=cannot attack. Lasts 4 seconds]"; } }
		public override string SecondDescription{ get{ return "[+5% Hit Chance, +6 base damage, chance to hit is the difference between " +
					"the tactics skill or 40%, whichever is higher. Leg hit=cannot run, right arm=cannot attack, left arm=cannot attack. Lasts 8 seconds]"; } }
		public override string ThirdDescription{ get{ return "[+7.5% Hit Chance, +9 base damage, chance to hit is the difference between the tactics skill " +
					"or 60%, whichever is higher. Leg hit=cannot run, right arm=cannot attack, left arm=cannot attack. Lasts 12 seconds]"; } }

		public override string FirstCommand{ get{ return ".Disable"; } }
		public override string SecondCommand{ get{ return ".Disable"; } }
		public override string ThirdCommand{ get{ return ".Disable"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Disable()); }
		
		public Disable() {}
	}
}
