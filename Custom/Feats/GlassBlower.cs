using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class GlassBlower : BaseFeat
	{
		public override string Name{ get{ return "Glass Blower"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.GlassBlower; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Craftsmanship }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill gives the character the ability to harvest sand."; } }
		public override string SecondDescription{ get{ return "This skill gives the character the ability to create ornamental glass bottles."; } }
		public override string ThirdDescription{ get{ return "This skill gives the character the ability to make alchemical bottles."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new GlassBlower()); }
		
		public override void FixAddOns( PlayerMobile owner )
		{
			if( Level > 0 )
				owner.SandMining = true;
			
			else
				owner.SandMining = false;
			
			if( Level > 1 )
				owner.Glassblowing = true;
			
			else
				owner.Glassblowing = false;
		}
		
		public GlassBlower() {}
	}
}
