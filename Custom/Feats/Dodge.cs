using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Dodge : BaseFeat
	{
		public override string Name{ get{ return "Dodge"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Dodge; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Dodge }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.EnhancedDodge }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Dodge skill, which will " +
					"give you physical resistances when you are not wearing any armour. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Dodge()); }
		
		public Dodge() {}

        public override void OnLevelChanged( PlayerMobile owner )
        {
            base.OnLevelChanged( owner );

            owner.ComputeResistances();

            if( owner.HasGump( typeof( Gumps.CharInfoGump ) ) && owner.m_CharInfoTimer == null )
            {
                owner.m_CharInfoTimer = new Gumps.CharInfoGump.CharInfoTimer( owner );
                owner.m_CharInfoTimer.Start();
            }
        }
	}
}
