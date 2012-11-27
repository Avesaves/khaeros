using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class Stash : LockableContainer
	{
		public override int MaxWeight{ get{ return 2000; } }
		
		private Mobile m_Owner;
		private DateTime m_LastsUntil;
		private Item m_StashContainer;
		private int m_StashID;
		private bool m_CloseNext;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner{ get{ return m_Owner; } set{ m_Owner = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime LastsUntil{ get{ return m_LastsUntil; } set{ m_LastsUntil = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int StashID{ get{ return m_StashID; } set{ m_StashID = value; } }
		
		public bool CloseNext{ get{ return m_CloseNext; } set{ m_CloseNext = value; } }

		[Constructable]
		public Stash( PlayerMobile owner, Point3D location )
			: base( 0x9A8 )
		{
			if( owner == null || owner.Deleted )
				this.Delete();
			
			else
			{
				this.Owner = owner;
	
				int choice = Utility.RandomMinMax( 1, 6 );
				
				switch( choice )
				{
					case 1: ItemID = 6011; break;
					case 2: ItemID = 6004; break;
					case 3: ItemID = 4971; break;
					case 4: ItemID = 3258; break;
					case 5: ItemID = 3239; break;
					case 6: ItemID = 3241; break;
				}
				
				this.LastsUntil = DateTime.Now + TimeSpan.FromDays( (owner.Feats.GetFeatLevel(FeatList.Stash) * 5) );
				owner.HasStash = true;
				this.StashID = this.ItemID;
				
				Weight = 1.0;
			}
		}
		
		public override void OnDoubleClick( Mobile m )
		{
			if( this == null || m == null || this.Deleted || m.Deleted )
				return;
			
			if( this.ItemID != 0x9A8 )
			{
				this.ItemID = 0x9A8;
				NetState ns = m.NetState;
				
				if( ns != null )
				{
					ns.Send( this.RemovePacket );
					this.SendInfoTo( ns );
				}
			}
			
			else if( !this.CloseNext )
			{
				base.OnDoubleClick( m );
				this.CloseNext = true;
			}
			
			else
			{
				this.CloseNext = false;
				this.ItemID = this.StashID;
			}
		}
		
		public override void OnDelete()
		{
			if( this.Owner != null && !this.Owner.Deleted && this.Owner is PlayerMobile )
				( (PlayerMobile)this.Owner ).HasStash = false;
			
			base.OnDelete();
		}

		public Stash( Serial serial )
			: base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
			writer.Write( (Mobile) m_Owner );
			writer.Write( (DateTime) m_LastsUntil );
			writer.Write( (int) m_StashID );
			writer.Write( (bool) m_CloseNext );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Owner = reader.ReadMobile();
			m_LastsUntil = reader.ReadDateTime();
			m_StashID = reader.ReadInt();
			m_CloseNext = reader.ReadBool();
		}
	}
}
