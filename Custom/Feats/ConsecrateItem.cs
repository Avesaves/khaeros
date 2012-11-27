using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ConsecrateItem : BaseFeat
	{
		public override string Name{ get{ return "Consecrate Item"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ConsecrateItem; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Faith }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Mending }; } }
		
		public override string FirstDescription{ get{ return "This spell allows you to bless a gold necklace and turn it into a holy symbol. " +
					"[Consecrates a gold necklace, giving it 5% energy resist, costs 50 mana]"; } }
		public override string SecondDescription{ get{ return "[Consecrates a staff, giving it +10% Defend Chance and enables the cleric to " +
					"meditate with it, costs 50 mana]"; } }
		public override string ThirdDescription{ get{ return "[Consecrates a robe, giving it +2 mana and +1 hits regen, costs 50 mana]"; } }

		public override string FirstCommand{ get{ return ".ConsecrateItem | .ConsecrateItem 1"; } }
		public override string SecondCommand{ get{ return ".ConsecrateItem | ConsecrateItem 2"; } }
		public override string ThirdCommand{ get{ return ".ConsecrateItem | ConsecrateItem 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ConsecrateItem()); }
		
		public ConsecrateItem() {}
	}
}
