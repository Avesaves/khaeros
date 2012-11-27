using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server.Gumps
{
	public class PaintingGump : Gump
	{
        private Painting m_painting;
        private PlayerMobile m_pm;

		public PaintingGump( PlayerMobile pm, Painting painting )
			: base( 0, 0 )
		{
            m_painting = painting;
            m_pm = pm;

			pm.CloseGump( typeof( PaintingGump ) );
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;

            this.AddPage( 0 );
            this.AddBackground( 54, 31, 400, 383, 9270 );
            this.AddImage( 4, 10, 10440 );
            this.AddImage( 423, 10, 10441 );
            this.AddImage( 180, 50, 29 );
            this.AddLabel( 220, 47, 2010, @"Painting" );
            this.AddButton( 404, 46, 1150, 1152, 0, GumpButtonType.Reply, 0 );
            this.AddBackground( 82, 162, 345, 226, 3500 );
            this.AddButton( 369, 46, 1153, 1155, 1, GumpButtonType.Reply, 0 );

            if( pm.Feats.GetFeatLevel(FeatList.Painter) > 1 )
            {
                this.AddLabel( 123, 176, 0, @"Name:" );
                this.AddTextEntry( 160, 176, 235, 20, 0, 2, @"" + painting.Name );
            }

            if( pm.Feats.GetFeatLevel(FeatList.Painter) > 2 )
            {
                this.AddLabel( 219, 205, 0, @"Description" );
                this.AddTextEntry( 123, 230, 261, 140, 0, 3, @"" + painting.Description );
            }

            this.AddButton( 369, 79, 5533, 5535, 4, GumpButtonType.Reply, 0 );
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile m = sender.Mobile as PlayerMobile;
			
			if ( m == null )
				return;

			switch ( info.ButtonID )
			{
					
				case 0:
				{
					break;
				}
					
				case 1: 
				{
                    if( m_pm.Feats.GetFeatLevel(FeatList.Painter) > 1 )
                    {
                        TextRelay target = info.GetTextEntry( 2 );
                        m_painting.Name = target.Text;
                    }

                    if( m_pm.Feats.GetFeatLevel(FeatList.Painter) > 2 )
                    {
                        TextRelay target2 = info.GetTextEntry( 3 );
                        m_painting.Description = target2.Text;
                    }
					break;
				}

                case 4:
                {
                    if( m_pm.Feats.GetFeatLevel(FeatList.Painter) > 1 )
                    {
                        TextRelay target = info.GetTextEntry( 2 );
                        m_painting.Name = target.Text;
                    }

                    if( m_pm.Feats.GetFeatLevel(FeatList.Painter) > 2 )
                    {
                        TextRelay target2 = info.GetTextEntry( 3 );
                        m_painting.Description = target2.Text;
                    }

                    Painting.ChangeID( m_painting, m_painting.Index + 1 );
                    m_pm.SendGump( new PaintingGump( m_pm, m_painting ) );
                    break;
                }
			}
		}
	}
}
