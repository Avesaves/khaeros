using System;
using Server.Mobiles;

namespace Server.Items
{
	[Flipable( 0x304B, 0x3049 )]
	public class DiabloDungeonCoffinSlave : Item
	{
		private Item m_CoffinMaster;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Item CoffinMaster
		{
			get{ return m_CoffinMaster; }
			set{ m_CoffinMaster = value; }
		}
		
		[Constructable]
		public DiabloDungeonCoffinSlave() : base( 0x304B )
		{
			Movable = false;
			Hue = 2594;
		}

		public DiabloDungeonCoffinSlave( Serial serial ) : base( serial )
		{
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( this.CoffinMaster != null )
			{
				try
				{
					this.CoffinMaster.OnDoubleClick( from );
				}
				
				catch
				{
					this.CoffinMaster = null;
				}
			}
			
			if( from.AccessLevel > AccessLevel.Player )
				from.SendMessage( "GM-only debug message: this coffin lacks a master counterpart. Please delete it." );
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
			
			writer.Write( (Item) m_CoffinMaster );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			m_CoffinMaster = reader.ReadItem();
		}
	}
	
	[Flipable( 0x304A, 0x3048 )]
	public class DiabloDungeonCoffin : Item
	{
		private DateTime m_LastTimeOpened;
		private Item m_CoffinSlave;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Item CoffinSlave
		{
			get{ return m_CoffinSlave; }
			set{ m_CoffinSlave = value; }
		}
		
		public bool CanBeOpened
		{
			get
			{
				if( DateTime.Compare( DateTime.Now, ( m_LastTimeOpened + TimeSpan.FromHours( 2 ) ) ) > 0 )
					return true;
				
				return false;
			}
		}
		
		[Constructable]
		public DiabloDungeonCoffin() : base( 0x304A )
		{
			Movable = false;
			Hue = 2594;
		}

		public DiabloDungeonCoffin( Serial serial ) : base( serial )
		{
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !from.Alive || from.Paralyzed || !from.InRange( this, 2 ) || !from.InLOS( this ) )
			{
				from.SendMessage( "That is too far away." );
				return;
			}
			
			if( this.CoffinSlave == null && from.AccessLevel > AccessLevel.Player )
			{
				from.SendMessage( "Creating a slave for the coffin." );
				DiabloDungeonCoffinSlave slave = new DiabloDungeonCoffinSlave();
				slave.Map = this.Map;
				slave.Location = new Point3D( this.X - 1, this.Y, this.Z );
				this.CoffinSlave = slave;
				slave.CoffinMaster = this;
				return;
			}
			
			if( this.CanBeOpened )
			{
				this.m_LastTimeOpened = DateTime.Now;
				
				int chance = Utility.RandomMinMax( 1, 100 );
				
				if( chance > 70 )
				{
					from.SendMessage( "You have found some copper coins inside the coffin!" );
					chance = Utility.RandomMinMax( 3, 6 );
					Copper copper = new Copper( chance );
					Container pack = from.Backpack;
					pack.DropItem( copper );
				}
				
				else
				{
					from.SendMessage( "You have disturbed the undead!" );
					
					BaseCreature skeleton = new Skeleton() as BaseCreature;
					
					if( chance < 20 )
						skeleton = new SkeletalSoldier() as BaseCreature;

					skeleton.FightMode = FightMode.Closest;
	
					bool validLocation = false;
					Point3D loc = this.Location;
	
					for ( int j = 0; !validLocation && j < 10; ++j )
					{
						int x = X + Utility.Random( 3 ) - 1;
						int y = Y + Utility.Random( 3 ) - 1;
						int z = this.Map.GetAverageZ( x, y );
	
						if ( validLocation = this.Map.CanFit( x, y, this.Z, 16, false, false ) )
							loc = new Point3D( x, y, Z );
						else if ( validLocation = this.Map.CanFit( x, y, z, 16, false, false ) )
							loc = new Point3D( x, y, z );
					}
	
					skeleton.MoveToWorld( loc, this.Map );
					skeleton.Combatant = from;
					skeleton.VanishTime = DateTime.Now + TimeSpan.FromHours( 1 );
					skeleton.VanishEmote = "*crumbles into dust*";
					skeleton.PlaySound( 1169 );
				}
			}
			
			else
				from.SendMessage( "The coffin appears to be empty." );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
			
			writer.Write( (DateTime) m_LastTimeOpened );
			writer.Write( (Item) m_CoffinSlave );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			m_LastTimeOpened = reader.ReadDateTime();
			m_CoffinSlave = reader.ReadItem();
		}
	}
}
