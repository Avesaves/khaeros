using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class DamageIgnore : BaseFeat
	{
		public override string Name{ get{ return "Damage Ignore"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.DamageIgnore; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Rage }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.FastHealing }; } }
		
		public override string FirstDescription{ get{ return "Having conditioned yourself to extreme pain you are able to " +
					"shrug off some damage the enemies inflict upon you. [-1 damage you take from weapon sources]"; } }
		public override string SecondDescription{ get{ return "[-2 damage]"; } }
		public override string ThirdDescription{ get{ return "[-3 damage]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new DamageIgnore()); }
		
		public DamageIgnore() {}
	}
}
