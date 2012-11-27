using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class WeakWilled : BaseBackground
	{
		public override int Cost{ get{ return -1000; } }
		public override string Name{ get{ return "Weak Willed"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.WeakWilled; } }
		public override string Description{ get{ return "This flaw will lower both your current Mana and also your Mana cap by 5."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new WeakWilled()); }

        public override bool MeetsOurRequirements( PlayerMobile m, bool message )
        {
            return TestBackgroundForPurchase( m, BackgroundList.IronWilled, message );
        }

        public override bool CanAcquireThisBackground( PlayerMobile m )
        {
            if( m.RawMana < 10 )
            {
                m.SendMessage( "Your current Mana is already too low." );
                return false;
            }

            return base.CanAcquireThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.RawMana -= 5;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.RawMana += 5;
		}
		
		public WeakWilled() {}
	}
}
