using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ArmourFocus : BaseFeat
	{
		public override string Name{ get{ return "Armour Focus"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ArmourFocus; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.LightArmour, FeatList.MediumArmour, FeatList.HeavyArmour }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill removes some of the penalties for fighting in light armour. " +
					"[Lowers dex and stam penalty for 1 point.]"; } }
		public override string SecondDescription{ get{ return "This skill removes some of the penalties for fighting in medium armour. " +
					"[Lowers dex and stam penalty for 1 point.]"; } }
		public override string ThirdDescription{ get{ return "This skill removes some of the penalties for fighting in heavy armour. " +
					"[Lowers dex and stam penalty for 1 point.]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ArmourFocus()); }
		
		public ArmourFocus() {}
	}
}
