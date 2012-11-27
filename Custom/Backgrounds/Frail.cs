using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Frail : BaseBackground
	{
		public override int Cost{ get{ return -1000; } }
		public override string Name{ get{ return "Frail"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Frail; } }
		public override string Description{ get{ return "This flaw will lower both your current Hit Points and also your Hit Points cap by 5."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Frail()); }

        public override bool CanAcquireThisBackground( PlayerMobile m )
        {
            if( m.RawHits < 10 )
            {
                m.SendMessage( "Your current Hit Points are already too low." );
                return false;
            }

            return base.CanAcquireThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.RawHits -= 5;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.RawHits += 5;
		}
		
		public Frail() {}
	}
}
