using System;
using Server;
using Server.Network;
using Server.Regions;
using Server.Multis;
using Server.Gumps;
using Server.Targeting;

namespace Server.Items
{
	public enum DecorateCommand
	{
		None,
		Turn,
		Up,
		Down,
// added by Greywolf
		North,
		East,
		South,
		West
// end added
	}

	public class InteriorDecorator : Item
	{
		private DecorateCommand m_Command;

		[CommandProperty( AccessLevel.GameMaster )]
		public DecorateCommand Command{ get{ return m_Command; } set{ m_Command = value; InvalidateProperties(); } }

		[Constructable]
		public InteriorDecorator() : base( 0xFC1 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public override int LabelNumber{ get{ return 1041280; } } // an interior decorator

		public InteriorDecorator( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
            if ( m_Command == DecorateCommand.North )
                list.Add( "North" );
            else if ( m_Command == DecorateCommand.East )
                list.Add( "East" );
            else if ( m_Command == DecorateCommand.South )
                list.Add( "South" );
            else if ( m_Command == DecorateCommand.West )
                list.Add( "West" );
            else if ( m_Command != DecorateCommand.None )
                list.Add( 1018322 + (int)m_Command ); // Turn/Up/Down
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !CheckUse( this, from ) )
				return;

			if ( m_Command == DecorateCommand.None )
				from.SendGump( new InternalGump( this ) );
			else
				from.Target = new InternalTarget( this );
		}

		public static bool InHouse( Mobile from )
		{
			BaseHouse house = BaseHouse.FindHouseAt( from );

			return ( house != null && house.IsCoOwner( from ) );
		}

		public static bool CheckUse( InteriorDecorator tool, Mobile from )
		{
			/*if ( tool.Deleted || !tool.IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else*/
			if ( !InHouse( from ) )
				from.SendLocalizedMessage( 502092 ); // You must be in your house to do this.
			else
				return true;

			return false;
		}

		private class InternalGump : Gump
		{
			private InteriorDecorator m_Decorator;

			public InternalGump( InteriorDecorator decorator ) : base( 150, 50 )
			{
				m_Decorator = decorator;

				AddBackground( 0, 0, 170, 360, 2600 );

				AddButton( 40, 45, 2152, 2154, 1, GumpButtonType.Reply, 0 );
				AddLabel( 80, 50, 1149, "Turn" );
				//AddHtmlLocalized( 80, 50, 70, 40, 1018323, false, false ); // Turn

				AddButton( 40, 85, 2152, 2154, 2, GumpButtonType.Reply, 0 );
				AddLabel( 80, 90, 1149, "Up" );
				//AddHtmlLocalized( 80, 90, 70, 40, 1018324, false, false ); // Up

				AddButton( 40, 125, 2152, 2154, 3, GumpButtonType.Reply, 0 );
				AddLabel( 80, 130, 1149, "Down" );
				//AddHtmlLocalized( 80, 130, 70, 40, 1018325, false, false ); // Down
// added by Greywolf
				AddButton( 40, 165, 2152, 2154, 4, GumpButtonType.Reply, 0 );
				AddLabel( 80, 170, 1149, "North" );
				//AddHtmlLocalized( 80, 170, 70, 40, 1075389, false, false ); // north

				AddButton( 40, 205, 2152, 2154, 5, GumpButtonType.Reply, 0 );
				AddLabel( 80, 210, 1149, "East" );
				//AddHtmlLocalized( 80, 210, 70, 40, 1075387, false, false ); // east

				AddButton( 40, 245, 2152, 2154, 6, GumpButtonType.Reply, 0 );
				AddLabel( 80, 250, 1149, "South" );
				//AddHtmlLocalized( 80, 250, 70, 40, 1075386, false, false ); // south

				AddButton( 40, 285, 2152, 2154, 7, GumpButtonType.Reply, 0 );
				AddLabel( 80, 290, 1149, "West" );
				//AddHtmlLocalized( 80, 290, 70, 40, 1075390, false, false ); // west
// end added by Greywolf
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				DecorateCommand command = DecorateCommand.None;

				switch ( info.ButtonID )
				{
					case 1: command = DecorateCommand.Turn; break;
					case 2: command = DecorateCommand.Up; break;
					case 3: command = DecorateCommand.Down; break;
// added by Greywolf
					case 4: command = DecorateCommand.North; break;
					case 5: command = DecorateCommand.East; break;
					case 6: command = DecorateCommand.South; break;
					case 7: command = DecorateCommand.West; break;
//end added by Greywolf
				}

				if ( command != DecorateCommand.None )
				{
					m_Decorator.Command = command;
					sender.Mobile.Target = new InternalTarget( m_Decorator );
				}
			}
		}

		private class InternalTarget : Target
		{
			private InteriorDecorator m_Decorator;

			public InternalTarget( InteriorDecorator decorator ) : base( -1, false, TargetFlags.None )
			{
				CheckLOS = false;

				m_Decorator = decorator;
			}

			protected override void OnTargetNotAccessible( Mobile from, object targeted )
			{
				OnTarget( from, targeted );
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted == m_Decorator )
				{
					m_Decorator.Command = DecorateCommand.None;
					from.SendGump( new InternalGump( m_Decorator ) );
				}
				else if ( targeted is Item && InteriorDecorator.CheckUse( m_Decorator, from ) )
				{
					BaseHouse house = BaseHouse.FindHouseAt( from );
					Item item = (Item)targeted;

					if ( house == null || !house.IsCoOwner( from ) )
					{
						from.SendLocalizedMessage( 502092 ); // You must be in your house to do this.
					}
					else if ( item.Parent != null || !house.IsInside( item ) )
					{
						from.SendLocalizedMessage( 1042270 ); // That is not in your house.
					}
					else if ( !house.IsLockedDown( item ) && !house.IsSecure( item ) && (item.Movable))
					{
						from.SendLocalizedMessage( 1042271 ); // That is not locked down.
					}
					else if ( item is VendorRentalContract )
					{
						from.SendLocalizedMessage( 1062491 ); // You cannot use the house decorator on that object.
					}
					else
					{
						switch ( m_Decorator.Command )
						{
							case DecorateCommand.Up:	Up( item, from );	break;
							case DecorateCommand.Down:	Down( item, from );	break;
							case DecorateCommand.Turn:	Turn( item, from );	break;
// added by Greywolf
							case DecorateCommand.North:	North( item, from );	break;
							case DecorateCommand.East:	East( item, from );	break;
							case DecorateCommand.South:	South( item, from );	break;
							case DecorateCommand.West:	West( item, from );	break;
// end added by Greywolf
						}
					}
				}
			}

			private static void Turn( Item item, Mobile from )
			{
				FlipableAttribute[] attributes = (FlipableAttribute[])item.GetType().GetCustomAttributes( typeof( FlipableAttribute ), false );

				if( attributes.Length > 0 )
					attributes[0].Flip( item );
				else
					from.SendLocalizedMessage( 1042273 ); // You cannot turn that.
			}

			private static void Up( Item item, Mobile from )
			{
				int floorZ = GetFloorZ( item );

				if ( floorZ > int.MinValue && item.Z < (floorZ + 15) ) // Confirmed : no height checks here
					item.Location = new Point3D( item.Location, item.Z + 1 );
				else
					from.SendLocalizedMessage( 1042274 ); // You cannot raise it up any higher.
			}

			private static void Down( Item item, Mobile from )
			{
				int floorZ = GetFloorZ( item );

				if ( floorZ > int.MinValue && item.Z > GetFloorZ( item ) )
					item.Location = new Point3D( item.Location, item.Z - 1 );
				else
					from.SendLocalizedMessage( 1042275 ); // You cannot lower it down any further.
			}
// added by Greywolf
			private static void North( Item item, Mobile from )
			{
					item.Y = ( item.Y - 1 );
			}

			private static void East( Item item, Mobile from )
			{
					item.X = ( item.X + 1 );
			}

			private static void South( Item item, Mobile from )
			{
					item.Y = ( item.Y + 1 );
			}

			private static void West( Item item, Mobile from )
			{
					item.X = ( item.X - 1 );
			}
// end added by Greywolf

// modded for knives town houses
			private static int GetFloorZ( Item item )
			{
				Map map = item.Map;

				if ( map == null )
					return int.MinValue;

				Tile[] tiles = map.Tiles.GetStaticTiles( item.X, item.Y, true );

				int z = int.MinValue;

				for ( int i = 0; i < tiles.Length; ++i )
				{
					Tile tile = tiles[i];
					ItemData id = TileData.ItemTable[tile.ID & 0x3FFF];

					int top = tile.Z; // Confirmed : no height checks here

					if ( id.Surface && !id.Impassable && top > z && top <= item.Z )
						z = top;
				}

				if ( z == int.MinValue )
					z = map.Tiles.GetLandTile( item.X, item.Y ).Z;

				return z;
			}

			/*private static int GetFloorZ( Item item )
			{
				Map map = item.Map;

				if ( map == null )
					return int.MinValue;

				Tile[] tiles = map.Tiles.GetStaticTiles( item.X, item.Y, true );

				int z = int.MinValue;

				for ( int i = 0; i < tiles.Length; ++i )
				{
					Tile tile = tiles[i];
					ItemData id = TileData.ItemTable[tile.ID & 0x3FFF];

					int top = tile.Z; // Confirmed : no height checks here

					if ( id.Surface && !id.Impassable && top > z && top <= item.Z )
						z = top;
				}

				return z;
			}*/ //end mod-dupe for townhouses
		}
	}
}
