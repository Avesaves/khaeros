using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Painter : BaseFeat
	{
		public override string Name{ get{ return "Painter"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Painter; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Craftsmanship }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.TattooArtist }; } }
		
		public override string FirstDescription{ get{ return "This skill gives the character the ability to paint paintings."; } }
		public override string SecondDescription{ get{ return "This skill gives the character the ability to paint and name paintings."; } }
		public override string ThirdDescription{ get{ return "This skill gives the character the ability to paint, rename and add descriptions " +
					"to paintings."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Painter()); }
		
		public Painter() {}
	}
}
