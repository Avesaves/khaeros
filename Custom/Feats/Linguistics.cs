using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Linguistics : BaseFeat
	{
		public override string Name{ get{ return "Linguistics"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Linguistics; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Linguistics }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.SouthernLanguage, FeatList.WesternLanguage, FeatList.HaluarocLanguage, 
				FeatList.NorthernLanguage, FeatList.Shorthand, FeatList.Ventriloquism, FeatList.Cryptography }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Linguistics skill, which will " +
					"give you a small chance, according to your skill, to understand things spoken in languages you don't know. [5% chance " +
					"to understand languages you don't know]"; } }
		public override string SecondDescription{ get{ return "[10% chance to understand languages you don't know]"; } }
		public override string ThirdDescription{ get{ return "[15% chance to understand languages you don't know]"; } }
		
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Linguistics()); }
		
		public Linguistics() {}
	}
}
