using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class VenomousWay : BaseFeat
	{
		public override string Name{ get{ return "Venomous Way"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.VenomousWay; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Buildup }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Like the spiteful serpent, you strike with deadly force " +
					"and no regard for your safety. [+15% Attack Chance, -15% Defense Chance]"; } }
		public override string SecondDescription{ get{ return "[+30% Attack Chance, -30% Defense Chance]"; } }
		public override string ThirdDescription{ get{ return "[+45% Attack Chance, -45% Defense Chance]"; } }

		public override string FirstCommand{ get{ return ".VenomousWay"; } }
		public override string SecondCommand{ get{ return ".VenomousWay"; } }
		public override string ThirdCommand{ get{ return ".VenomousWay"; } }

		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new VenomousWay()); }
		
		public VenomousWay() {}
	}
}
