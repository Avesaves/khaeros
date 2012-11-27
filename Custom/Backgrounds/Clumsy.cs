using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Clumsy : BaseBackground
	{
		public override int Cost{ get{ return -1000; } }
		public override string Name{ get{ return "Clumsy"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Clumsy; } }
		public override string Description{ get{ return "This flaw will lower both your current Dexterity and also your Dexterity cap by 5."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Clumsy()); }

        public override bool CanAcquireThisBackground( PlayerMobile m )
        {
            if( m.RawDex < 10 )
            {
                m.SendMessage( "Your current Dexterity is already too low." );
                return false;
            }

            return base.CanAcquireThisBackground( m );
        }
		
		public override void OnAddedTo( PlayerMobile m )
		{
			m.RawDex -= 5;
		}
		
		public override void OnRemovedFrom( PlayerMobile m )
		{
			m.RawDex += 5;
		}
		
		public Clumsy() {}
	}
}
