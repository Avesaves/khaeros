using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Curse : BaseFeat
	{
		public override string Name{ get{ return "Curse"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Curse; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Faith }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.HoldPerson }; } }
		
		public override string FirstDescription{ get{ return "The curse cast by this spell will reduce a creature's hit points, stamina and mana " +
					"for some time. [Debuffs hits, stamina and mana by 5% vs creatures and 2.5% vs player characters]"; } }
		public override string SecondDescription{ get{ return "[Debuffs hits, stamina and mana by 10% vs creatures and 5% vs player characters]"; } }
		public override string ThirdDescription{ get{ return "[Debuffs hits, stamina and mana by 15% vs creatures and 7.5% vs player characters]"; } }
		
		public override string FirstCommand{ get{ return ".Curse | .Curse 1"; } }
		public override string SecondCommand{ get{ return ".Curse | .Curse 2"; } }
		public override string ThirdCommand{ get{ return ".Curse | Curse 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Curse()); }
		
		public Curse() {}
	}
}
