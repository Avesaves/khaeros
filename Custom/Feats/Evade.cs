using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Evade : BaseFeat
	{
		public override string Name{ get{ return "Evade"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Evade; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.PureDodge }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are able to avoid special attacks and attacks of magical nature. [10% " +
					"chance to avoid spells and special attacks]"; } }
		public override string SecondDescription{ get{ return "[20% chance to avoid spells and special attacks]"; } }
		public override string ThirdDescription{ get{ return "[30% chance to avoid spells and special attacks]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Evade()); }
		
		public Evade() {}
	}
}
