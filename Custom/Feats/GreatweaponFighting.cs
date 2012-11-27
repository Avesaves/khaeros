using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class GreatweaponFighting : BaseFeat
	{
		public override string Name{ get{ return "Greatweapon Fighting"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.GreatweaponFighting; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Swordsmanship, FeatList.Macing, FeatList.ExoticWeaponry, 
				FeatList.Fencing, FeatList.Axemanship, FeatList.Polearms }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Two handed weapons are an extension of your body. Increases " +
					"attack cooldown for the opponent when you parry their attack with a 2H weapon. [0.125s cooldown increase]"; } }
		public override string SecondDescription{ get{ return "[0.25s cooldown increase]"; } }
		public override string ThirdDescription{ get{ return "[0.5s cooldown increase]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new GreatweaponFighting()); }
		
		public GreatweaponFighting() {}
	}
}
