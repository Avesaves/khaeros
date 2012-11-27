using System;
using System.Collections;
using Server;
using Server.Targeting;

namespace Server.Items
{
	[Flipable( 0x14F0, 0x14EF )]
	public abstract class BaseAddonDeed : Item
	{
		public abstract BaseAddon Addon{ get; }

		public BaseAddonDeed() : base( 0x14F0 )
		{
			Weight = 1.0;

			if ( !Core.AOS )
				LootType = LootType.Newbied;
		}

		public BaseAddonDeed( Serial serial ) : base( serial )
		{
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

			if ( Weight == 0.0 )
				Weight = 1.0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) ) {
				from.SendMessage( "Target a spot on the ground where you wish to place this, or target this deed to change the orientation." );
				from.Target = new InternalTarget( this );
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		private class InternalTarget : Target
		{
			private BaseAddonDeed m_Deed;

			public InternalTarget( BaseAddonDeed deed ) : base( -1, true, TargetFlags.None )
			{
				m_Deed = deed;

				CheckLOS = false;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				IPoint3D p = targeted as IPoint3D;
				Map map = from.Map;

				if ( p == null || map == null || m_Deed.Deleted )
					return;

				if ( m_Deed.IsChildOf( from.Backpack ) )
				{
					if ( targeted == m_Deed ) // change direction
					{
						string currentType = m_Deed.GetType().FullName;
						int south = currentType.LastIndexOf( "SouthDeed" );
						int east = currentType.LastIndexOf( "EastDeed" );
						string newType;
						if ( south == -1 && east == -1 )
						{
							from.SendMessage( "This does not have any other orientation." );
							return;
						}
						else if ( east != -1 )
						{
							newType = currentType.Replace( "EastDeed", "SouthDeed" );
						}
						else
							newType = currentType.Replace( "SouthDeed", "EastDeed" );
						
						Type type = Type.GetType( newType );
						
						if ( type == null )
						{
							from.SendMessage( "This does not have any other orientation." );
							return;
						}
						
						Item item = Activator.CreateInstance(type) as Item;
						
						if ( item == null )
						{
							from.SendMessage( "This does not have any other orientation." );
							return;
						}
						
						m_Deed.Delete();
						from.Backpack.DropItem( item );
						return;
					}
					else
					{
						BaseAddon addon = m_Deed.Addon;

						Server.Spells.SpellHelper.GetSurfaceTop( ref p );

						ArrayList houses = null;

						AddonFitResult res = addon.CouldFit( p, map, from, ref houses );

						if ( res == AddonFitResult.Valid )
							addon.MoveToWorld( new Point3D( p ), map );
						else if ( res == AddonFitResult.Blocked )
							from.SendLocalizedMessage( 500269 ); // You cannot build that there.
						else if ( res == AddonFitResult.NotInHouse )
							from.SendLocalizedMessage( 500274 ); // You can only place this in a house that you own!
						else if ( res == AddonFitResult.DoorsNotClosed )
							from.SendMessage( "You must close all house doors before placing this." );
						else if ( res == AddonFitResult.DoorTooClose )
							from.SendLocalizedMessage( 500271 ); // You cannot build near the door.
						else if ( res == AddonFitResult.NoWall )
							res = AddonFitResult.Valid;
						
						if ( res == AddonFitResult.Valid )
						{
							m_Deed.Delete();

							if ( houses != null )
							{
								foreach ( Server.Multis.BaseHouse h in houses )
									h.Addons.Add( addon );
							}
						}
						else
						{
							addon.Delete();
						}
					}
				}
				else
				{
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				}
			}
		}
	}
}
