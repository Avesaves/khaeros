using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class FlashyAttack : BaseFeat
	{
		public override string Name{ get{ return "Flashy Attack"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.FlashyAttack; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Fencing }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This maneuver adds an additional attack. Once your original attack lands, " +
					"or is parried, the second attack commences right away automatically, in the best direction (but still different from " +
					"the first strike) according to the weapon in your hands. <BR>If there are no other directions than the current one, the " +
					"skill will not work (for example, using swing-only weapons on a horse). [The second attack will deal 20% of total damage]"; } }
		public override string SecondDescription{ get{ return "[The second attack will deal 40% of total damage]"; } }
		public override string ThirdDescription{ get{ return "[The second attack will deal 60% of total damage]"; } }

		public override string FirstCommand{ get{ return ".FlashyAttack"; } }
		public override string SecondCommand{ get{ return ".FlashyAttack"; } }
		public override string ThirdCommand{ get{ return ".FlashyAttack"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new FlashyAttack()); }
		
		public FlashyAttack() {}
	}
}
