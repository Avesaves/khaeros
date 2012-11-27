using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class SunderWeapon : BaseFeat
	{
		public override string Name{ get{ return "Sunder Weapon"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.SunderWeapon; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Your blow connects with your opponents weapon as well, greatly damaging it in " +
					"the impact. [+5% Hit Chance, 25% of your damage is dealt to the target's weapon as well]"; } }
		public override string SecondDescription{ get{ return "[+10% Hit Chance, 50% of your damage is dealt to the target's weapon as well]"; } }
		public override string ThirdDescription{ get{ return "[+15% Hit Chance, 75% of your damage is dealt to the target's weapon as well]"; } }

		public override string FirstCommand{ get{ return ".SunderWeapon"; } }
		public override string SecondCommand{ get{ return ".SunderWeapon"; } }
		public override string ThirdCommand{ get{ return ".SunderWeapon"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new SunderWeapon()); }
		
		public SunderWeapon() {}
	}
}
