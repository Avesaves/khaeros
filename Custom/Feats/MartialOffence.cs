using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class MartialOffence : BaseFeat
	{
		public override string Name{ get{ return "Martial Offence"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.MartialOffence; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Technique }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.ThroatStrike, FeatList.Buildup, FeatList.Disarm, 
				FeatList.Dismount, FeatList.EyeRaking, FeatList.StunningBlow }; } }
		
		public override string FirstDescription{ get{ return "This skill allows you to use your dexterity to gain bonus damage and speed " +
					"when unarmed. [20% dexterity is used]"; } }
		public override string SecondDescription{ get{ return "[40% dexterity is used]"; } }
		public override string ThirdDescription{ get{ return "[60% dexterity is used]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new MartialOffence()); }
		
		public MartialOffence() {}
	}
}
