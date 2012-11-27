using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Cryptography : BaseFeat
	{
		public override string Name{ get{ return "Cryptography"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Cryptography; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Linguistics }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will allow you to cypher informantion on a book using simple " +
					"cryptography."; } }
		public override string SecondDescription{ get{ return "[This skill will allow you to cypher informantion on a book using medium cryptography.]"; } }
		public override string ThirdDescription{ get{ return "[This skill will allow you to cypher informantion on a book using complex cryptography.]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Cryptography()); }
		
		public Cryptography() {}
	}
}
