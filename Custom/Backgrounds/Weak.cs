using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Weak : BaseBackground
	{
		public override int Cost{ get{ return -1000; } }
		public override string Name{ get{ return "Weak"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Weak; } }
		public override string Description{ get{ return "This flaw will lower both your current Strength and also your Strength cap by 5."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Weak()); }

        public override bool CanAcquireThisBackground( PlayerMobile m )
        {
            if( m.RawStr < 10 )
            {
                m.SendMessage( "Your current Strength is already too low." );
                return false;
            }

            return base.CanAcquireThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.RawStr -= 5;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.RawStr += 5;
		}
		
		public Weak() {}
	}
}
