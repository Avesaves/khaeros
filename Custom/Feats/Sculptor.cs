using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Sculptor : BaseFeat
	{
		public override string Name{ get{ return "Sculptor"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Sculptor; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Craftsmanship }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are able to mine granite and coal."; } }
		public override string SecondDescription{ get{ return "You have learned the art of sculpting."; } }
		public override string ThirdDescription{ get{ return "Your skill with the mallet and chisel has reached such levels that you are " +
					"able to sculpt truly unique sculptures."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return ".EngraveMasonry"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Sculptor()); }
		
		public override void FixAddOns( PlayerMobile owner )
		{
			if( Level > 0 )
				owner.StoneMining = true;
			
			else
				owner.StoneMining = false;
			
			if( Level > 1 )
				owner.Masonry = true;
			
			else
				owner.Masonry = false;
		}
		
		public Sculptor() {}
	}
}
