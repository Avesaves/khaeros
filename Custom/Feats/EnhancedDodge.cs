using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class EnhancedDodge : BaseFeat
	{
		public override string Name{ get{ return "Enhanced Dodge"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.EnhancedDodge; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Dodge }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.PureDodge }; } }
		
    public override string FirstDescription{ get{ return "You are extremely good at dodging blows. [+2% all physical resists 5% chance " +
          "to completely dodge an attack]"; } }
    public override string SecondDescription{ get{ return "[+5% all physical resists 10% chance to completely dodge an attack]"; } }
    public override string ThirdDescription{ get{ return "[+10% all physical resists 15% chance to completely dodge an attack]"; } }
//>>>>>>> master

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new EnhancedDodge()); }
		
		public EnhancedDodge() {}

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
