using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ShieldBash : BaseFeat
	{
		public override string Name{ get{ return "Shield Bash"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ShieldBash; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Parrying }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.ShieldMastery }; } }
		
		public override string FirstDescription{ get{ return "Using your shield, you " +
					"bash your enemy, dealing damage and slowing their attacks. This attack " +
					"cannot be parried. It will count as an attack with the speed of 1 second. " +
					"This attack will also change the opponent's target to you. Animation " +
					"(connect) speed is determined by Parry skill. The damage dealt does not " +
					"go through any kind of directional percentage (i.e. it deals 100% damage). " +
					"Damage type is blunt. This attack can only be used on foot. " + 
					"[1 base damage, -10% weapon speed reduction for 6 seconds]"; } }
		public override string SecondDescription{ get{ return "[2 base damage, -20% weapon speed reduction for 6 seconds]"; } }
		public override string ThirdDescription{ get{ return "[3 base damage, -30% weapon speed reduction for 6 seconds]"; } }

		public override string FirstCommand{ get{ return ".attack ShieldBash"; } }
		public override string SecondCommand{ get{ return ".attack ShieldBash"; } }
		public override string ThirdCommand{ get{ return ".attack ShieldBash"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ShieldBash()); }
		
		public ShieldBash() {}
	}
}
