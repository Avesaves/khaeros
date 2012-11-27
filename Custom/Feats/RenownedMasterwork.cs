using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class RenownedMasterwork : BaseFeat
	{
		public override string Name{ get{ return "Renowned Masterwork"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.RenownedMasterwork; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Masterwork }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.CraftingSpecialization }; } }
		
		public override string FirstDescription{ get{ return "This skill gives the character the ability, unlike Masterwork, to choose the " +
					"bonuses they wish to add to their works. [You can choose one bonus]"; } }
		public override string SecondDescription{ get{ return "[You can choose two bonuses]"; } }
		public override string ThirdDescription{ get{ return "[You can choose three bonuses and also name your masterwork creations (and also " +
					"extraordinary if they are clothing)]"; } }
		
		public override string FirstCommand{ get{ return ".MasterworkEquip"; } }
		public override string SecondCommand{ get{ return ".MasterworkEquip"; } }
		public override string ThirdCommand{ get{ return ".MasterworkEquip | .NameEquip"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new RenownedMasterwork()); }
		
		public override void FixAddOns( PlayerMobile owner )
		{
			owner.Masterwork.BluntResist = 0;
			owner.Masterwork.SlashingResist = 0;
			owner.Masterwork.PiercingResist = 0;
			owner.Masterwork.WeaponAccuracy = 0;
			owner.Masterwork.WeaponDamage = 0;
			owner.Masterwork.WeaponSpeed = 0;
			owner.Masterwork.ArmourPointsLeft = Level;
			owner.Masterwork.WeaponPointsLeft = Level;
		}
		
		public RenownedMasterwork() {}
	}
}
