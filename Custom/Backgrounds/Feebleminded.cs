using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Feebleminded : BaseBackground
	{
		public override int Cost{ get{ return -1000; } }
		public override string Name{ get{ return "Feebleminded"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Feebleminded; } }
		public override string Description{ get{ return "This flaw will lower both your current Intelligence and also your Intelligence cap by 5."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Feebleminded()); }

        public override bool CanAcquireThisBackground( PlayerMobile m )
        {
            if( m.RawInt < 10 )
            {
                m.SendMessage( "Your current Intelligence is already too low." );
                return false;
            }

            return base.CanAcquireThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.RawInt -= 5;
			m.CPCapOffset -= 1000;
			m.FeatSlots += 1000;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.RawInt += 5;
			m.CPCapOffset += 1000;
			m.FeatSlots -= 1000;
		}
		
		public Feebleminded() {}
	}
}
