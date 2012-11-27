/***************************************************************************
 *                               Containers.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id: Containers.cs 4 2006-06-15 04:28:39Z mark $
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class PlayerBankBox : BankBox
	{
		private Mobile m_Owner;
		private bool m_Open;

		public PlayerBankBox( Serial serial ) : base( serial )
		{
		}
/*
		public void Open()
		{
			m_Open = true;

			if ( m_Owner != null )
			{
				m_Owner.PrivateOverheadMessage( MessageType.Regular, 0x3B2, true, String.Format( "Bank container has {0} items, {1} stones", TotalItems, TotalWeight ), m_Owner.NetState );
				m_Owner.Send( new EquipUpdate( this ) );
				m_Owner.SendMessage( "You have opened a player bank box, not a regular bank box." );
				DisplayTo( m_Owner );
			}
		}
*/
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

		//private static bool m_SendRemovePacket;

		//public static bool SendDeleteOnClose{ get{ return m_SendRemovePacket; } set{ m_SendRemovePacket = value; } }
/*
		public void Close()
		{
			m_Open = false;

			if ( m_Owner != null && m_SendRemovePacket )
				m_Owner.Send( this.RemovePacket );
		}
*/
		public override void OnSingleClick( Mobile from )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
		}

	/*	public override DeathMoveResult OnParentDeath( Mobile parent )
		{
			return DeathMoveResult.RemainEquiped;
		}
*/
		public PlayerBankBox( Mobile owner ) : base( owner )
		{
		}

		public override bool IsAccessibleTo(Mobile check)
		{
		 	//if ( ( check == m_Owner && m_Open ) || check.AccessLevel >= AccessLevel.GameMaster )
		 		return base.IsAccessibleTo (check);
		 	//else
		 		//return false;
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if( !( dropped is BaseJewel || dropped is Gold || dropped is Copper || dropped is Silver || 
                dropped is IGem || dropped is BankCheck || dropped is RewardToken ) )
			{
				from.SendMessage( "Bank boxes can only store coins, gems, jewelry, bank checks and reward tokens." );
				return false;
			}
			
		 	//if ( ( from == m_Owner && m_Open ) || from.AccessLevel >= AccessLevel.GameMaster )
		 		return base.OnDragDrop( from, dropped );
			//else
		 		//return false;
		}

		public override bool OnDragDropInto(Mobile from, Item dropped, Point3D p)
		{
            if( !( dropped is BaseJewel || dropped is Gold || dropped is Copper || dropped is Silver || 
                dropped is IGem || dropped is BankCheck || dropped is RewardToken ) )
            {
                from.SendMessage( "Bank boxes can only store coins, gems, jewelry, bank checks and reward tokens." );
                return false;
            }
			
		 	//if ( ( from == m_Owner && m_Open ) || from.AccessLevel >= AccessLevel.GameMaster )
		 		return base.OnDragDropInto (from, dropped, p);
		 	//else
		 		//return false;
		}
	}
}
