using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ArmouredDodge : BaseFeat
	{
		public override string Name{ get{ return "Armoured Dodge"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ArmouredDodge; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.PureDodge }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "With this ability, you will be able to enjoy one of the benefits of Enhanced Dodge " +
					"even while wearing light or medium armour, as long as the total penalty of the pieces you are wearing do not exceed 2. [Retains " +
					"the chance to avoid damage from Enhanced Dodge, but not the resists]"; } }
		public override string SecondDescription{ get{ return "Changes the limit to 5."; } }
		public override string ThirdDescription{ get{ return "Changes the limit to 8."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ArmouredDodge()); }
		
		public ArmouredDodge() {}
	}
}
