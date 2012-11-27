using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Misc
{
    public class HueModTimer : Timer
	{
        public static void ApplyHueModToMobileAndGear( Mobile mob, int hue, TimeSpan delay )
        {
            ArrayList ourList = new ArrayList();
            ourList.Add( mob );

            foreach( Item item in mob.Items )
                ourList.Add( item );

            new HueModTimer( ourList, hue, delay ).Start();
        }

        private ArrayList list;

        public HueModTimer( ArrayList objects, int newhue, TimeSpan delay )
			: base( delay )
		{
            list = objects;

            if( list == null )
            {
                Stop();
                return;
            }

            for( int i = 0; i < list.Count; i++ )
            {
                if( list[i] is Item )
                    ((Item)list[i]).HueMod = newhue;

                else if( list[i] is Mobile )
                {
                    ( (Mobile)list[i] ).HueMod = newhue;
                    ( (Mobile)list[i] ).HairHueMod = newhue;
                    ( (Mobile)list[i] ).FacialHairHueMod = newhue;
                }
            }
		}

		protected override void OnTick()
		{
            if( list == null )
                return;

            for( int i = 0; i < list.Count; i++ )
            {
                if( list[i] is Item )
                    ( (Item)list[i] ).HueMod = -1;

                else if( list[i] is Mobile )
                {
                    ( (Mobile)list[i] ).HueMod = -1;
                    ( (Mobile)list[i] ).HairHueMod = -1;
                    ( (Mobile)list[i] ).FacialHairHueMod = -1;
                }
            }
		}
	}
}
