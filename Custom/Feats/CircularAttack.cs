using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class CircularAttack : BaseFeat
	{
		public override string Name{ get{ return "Circular Attack"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.CircularAttack; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.WeaponSpecialization }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "While on foot and wielding a two handed weapon in which you are specialized " +
					"you are able to spin in a circle and attack all the opponents around you. [-50% total damage]"; } }
		public override string SecondDescription{ get{ return "[Normal damage]"; } }
		public override string ThirdDescription{ get{ return "[+50% total damage]"; } }

		public override string FirstCommand{ get{ return ".attack circulardisabledfornow"; } }
		public override string SecondCommand{ get{ return ".attack circulardisabledfornow"; } }
		public override string ThirdCommand{ get{ return ".attack circulardisabledfornow"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new CircularAttack()); }
		
		public CircularAttack() {}
	}
}
