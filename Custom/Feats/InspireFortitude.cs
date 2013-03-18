using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class InspireFortitude : BaseFeat
	{
		public override string Name{ get{ return "Inspire Fortitude"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.InspireFortitude; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Faith }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "With this skill, you will be able to inspire your allies and refresh their " +
					"condition entirely (improved hits, stam and mana regen)."; } }
		public override string SecondDescription{ get{ return "Reduced stamina cost, better bonuses."; } }
		public override string ThirdDescription{ get{ return "Reduced stamina cost, better bonuses."; } }

		public override string FirstCommand{ get{ return ".InspireFortitude | .CancelCommand"; } }
		public override string SecondCommand{ get{ return ".InspireFortitude | .CancelCommand"; } }
		public override string ThirdCommand{ get{ return ".InspireFortitude | .CancelCommand"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new InspireFortitude()); }
		
		public InspireFortitude() {}
	}
}
