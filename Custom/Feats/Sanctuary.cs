using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Sanctuary : BaseFeat
	{
		public override string Name{ get{ return "Sanctuary"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Sanctuary; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.AuraOfProtection }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This spell creates a protected area around the caster. Those that are outside " +
					"cannot attack those that are inside it and vice-versa. [Costs 20 mana and lasts for 20 seconds]"; } }
		public override string SecondDescription{ get{ return "[Costs 40 mana and lasts for 40 second]"; } }
		public override string ThirdDescription{ get{ return "[Costs 60 mana and lasts for 60 second]"; } }

		public override string FirstCommand{ get{ return ".Sanctuary | .Sanctuary 1"; } }
		public override string SecondCommand{ get{ return ".Sanctuary | .Sanctuary 2"; } }
		public override string ThirdCommand{ get{ return ".Sanctuary | .Sanctuary 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Sanctuary()); }
		
		public Sanctuary() {}
	}
}
