using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class WeaponParrying : BaseFeat
	{
		public override string Name{ get{ return "Weapon Parrying"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.WeaponParrying; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.WeaponSpecialization }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Whenever an attack is parried successfully with the specialized weapon " +
					"(shields cannot be used for this to work), the weapon specialist's defense cooldown timer is lowered somewhat. " +
					"[-0.125 cooldown]"; } }
		public override string SecondDescription{ get{ return "[-0.25 cooldown]"; } }
		public override string ThirdDescription{ get{ return "[-0.5 cooldown]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new WeaponParrying()); }
		
		public WeaponParrying() {}
	}
}
