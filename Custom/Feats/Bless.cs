using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Bless : BaseFeat
	{
		public override string Name{ get{ return "Bless"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Bless; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Faith }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.HaloOfLight }; } }
		
		public override string FirstDescription{ get{ return "The blessing granted by this spell will enhance a creature's hit points, stamina " +
					"and mana for some time. [Buffs hits, stamina and mana by 5% or 2.5% on self]"; } }
		public override string SecondDescription{ get{ return "[Buffs hits, stamina and mana by 10% or 5% on self]"; } }
		public override string ThirdDescription{ get{ return "[Buffs hits, stamina and mana by 15% or 7.5% on self]"; } }

		public override string FirstCommand{ get{ return ".Bless | .Bless 1"; } }
		public override string SecondCommand{ get{ return ".Bless | .Bless 2"; } }
		public override string ThirdCommand{ get{ return ".Bless | Bless 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Bless()); }
		
		public Bless() {}
	}
}
