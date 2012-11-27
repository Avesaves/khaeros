using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class IronWilled : BaseBackground
	{
		public override int Cost{ get{ return 2000; } }
		public override string Name{ get{ return "Iron Willed"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.IronWilled; } }
		public override string Description{ get{ return "This merit will raise both your current Mana and also your Mana cap by 5."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new IronWilled()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.WeakWilled, message );
		}

        public override bool CanRemoveThisBackground( PlayerMobile m )
        {
            if( m.RawMana < 10 )
            {
                m.SendMessage( "That would lower your current Mana too much." );
                return false;
            }

            return base.CanRemoveThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.RawMana += 5;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.RawMana -= 5;
		}
		
		public IronWilled() {}
	}
}
