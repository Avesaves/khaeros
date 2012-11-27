using System;
using Server;
using Server.Regions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.ContextMenus;
using Server.Mobiles;
using Knives.TownHouses;

namespace Server.Items
{
	public class WorldController : Item
	{
		private int m_Sequence = 1;
		private TimeSpan m_Frequency;
		public Timer Cleanse;
		private bool m_FixedMercs;
		private List<Item> m_Guilds = new List<Item>();
		
		public List<Item> Guilds{ get{ return m_Guilds; } set{ m_Guilds = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool FixedMercs{ get{ return m_FixedMercs; } set{ m_FixedMercs = value; } }
		
		public int Sequence
		{
			get{ return m_Sequence; }
			set{ m_Sequence = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Frequency
        {
            get { return m_Frequency; }
            set { m_Frequency = value; }
        }
		
		[Constructable]
		public WorldController() : base( 0x1817 )
		{
			Weight = 1.0;
			Stackable = false;
            Name = "World Controller";
            Frequency = TimeSpan.FromSeconds( 10 );
            
            foreach( Item item in World.Items.Values )
			{
            	if( item is WorldController && item != this )
            		this.Sequence++;
            }
            
            if( this.Sequence == 1 )
			{
				this.Cleanse = new CleanseTimer( Frequency, this );
				this.Cleanse.Start();
			}
            
            else
            	this.Delete();
		}

		public WorldController( Serial serial ) : base( serial )
		{
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( from.AccessLevel > AccessLevel.Player )
			{
				if( this.Sequence == 1 )
				{
					if( this.Cleanse != null )
					{
						this.Cleanse.Stop();
						this.Cleanse = null;
					}
					
					this.CleanseTheWorld();
				}
			}
		}
		
		public void CleanseTheWorld()
		{
			int summoneditem = 0;
        	int summonedmob = 0;
        	int count = 0;
        	
        	ArrayList itemlist = new ArrayList();
        	ArrayList moblist = new ArrayList();
        	ArrayList terminatelist = new ArrayList();
        	List<PlayerMobile> players = new List<PlayerMobile>();
        	List<AnimalTrainer> animalTrainers = new List<AnimalTrainer>();
        	List<BaseStorageContainer> storage = new List<BaseStorageContainer>();
        	
        	foreach( Item item in World.Items.Values )
			{
        		if( item is SpiderWeb && DateTime.Compare( DateTime.Now, ( (SpiderWeb)item ).CreationDate + TimeSpan.FromMinutes( 1 ) ) > 0 )
            		itemlist.Add( item );
        		
        		if( item is FootTrap && !item.Visible && DateTime.Compare( DateTime.Now, ( (FootTrap)item ).CreationDate + TimeSpan.FromHours( 1 ) ) > 0 )
					itemlist.Add( item );
        		
        		if( item is StablePost && ( (StablePost)item ).Owner != null )
        		{
        			StablePost post = item as StablePost;
        			
        			if( post.StabledDate != DateTime.MinValue && DateTime.Compare( DateTime.Now, ( post.StabledDate + TimeSpan.FromDays( 30 ) ) ) > 0 )
        			{
        				itemlist.Add( item );
        			}
        		}
        		
        		if( item is Stash )
        		{
        			Stash stash = item as Stash;
        			
        			if( DateTime.Compare( DateTime.Now, stash.LastsUntil ) > 0 )
        				itemlist.Add( item );
        		}
        		
        		if( item is BaseStorageContainer )
        			storage.Add( (BaseStorageContainer)item );
            }
        	
        	foreach( Mobile mob in World.Mobiles.Values )
        	{
        		if( mob is BaseCreature )
        		{
        			BaseCreature bc = mob as BaseCreature;
        			
        			if( bc.SummonedByMob && DateTime.Compare( DateTime.Now, bc.VanishTime ) > 0 )
        			{
        				moblist.Add( bc );
        			}
        			
        			if( bc.MarkedForTermination )
        				terminatelist.Add( bc );
        		}
        		
        		if( mob is PlayerMobile )
        			players.Add( (PlayerMobile)mob );
        		
        		if( mob is AnimalTrainer )
        			animalTrainers.Add( (AnimalTrainer)mob );
        	}
        	
        	count = itemlist.Count;
        	
        	for( int i = 0; i < count; i++ )
        	{
        		Item todelete = (Item)itemlist[i];
        		
    			summoneditem++;
    			
    			if( todelete is StablePost )
    			{
    				StablePost post = todelete as StablePost;
    				StablePost newpost = new StablePost();
    				
    				try
    				{
    					newpost.MoveToWorld( post.Location, post.Map );
    				}
    				
    				catch
    				{
    					Console.WriteLine( "There has been a problem with the World Controller replacing a Stable Post." );
    				}
    			}
        		
        		try
        		{
        			todelete.Delete();
        		}
        		
        		catch
        		{
        			summoneditem--;
					this.ThrowError();
					continue;
        		}
        	}
        	
        	count = moblist.Count;
        	
        	for( int i = 0; i < count; i++ )
        	{
        		BaseCreature todelete = (BaseCreature)moblist[i];
        		

    			summonedmob++;
        		
        		try
        		{
        			todelete.Emote( todelete.VanishEmote );
        			
        			if( todelete is IUndead )
        			{
        				GraveDust dust = new GraveDust();
        				dust.MoveToWorld( todelete.Location, todelete.Map );
        			}
        			
        			else if( todelete is BlackPuddingSpawn )
        			{
        				BlackSlime dust = new BlackSlime( 1 );
        				dust.MoveToWorld( todelete.Location, todelete.Map );
        			}
        			
        			else if( todelete is GelatinousBlobSpawn )
        			{
        				GreenSlime dust = new GreenSlime( 1 );
        				dust.MoveToWorld( todelete.Location, todelete.Map );
        			}
        			
        			else if( todelete is JellyOozeSpawn )
        			{
        				BlueSlime dust = new BlueSlime( 1 );
        				dust.MoveToWorld( todelete.Location, todelete.Map );
        			}
        			
        			else if( todelete is Wortling )
        			{
        				Wortfruit fruit = new Wortfruit( 1 );
        				fruit.MoveToWorld( todelete.Location, todelete.Map );
        			}
        			
        			todelete.Delete();
        		}
        		
        		catch
        		{
        			summonedmob--;
					this.ThrowError();
					continue;
        		}
        	}
        	
        	count = terminatelist.Count;
        	int killed = 0;
        	
        	for( int i = 0; i < count; i++ )
        	{
        		try
        		{
	        		Mobile m = terminatelist[i] as Mobile;
	        		m.Kill();
	        		killed++;
        		}
        		
        		catch(Exception e)
        		{
        			Console.WriteLine(e.Message);
        		}
        	}
        	
        	for( int i = 0; i < storage.Count; i++ )
        	{
        		BaseStorageContainer cont = storage[i] as BaseStorageContainer;
        		cont.HandleRent();
        	}
        	
        	HandleAnimalTrainers( animalTrainers );
        	
        	foreach ( NetState state in NetState.Instances )
			{
				Mobile m = state.Mobile;

				if ( m != null && m.AccessLevel > AccessLevel.Player )
				{
					m.SendMessage( "World controller finished cleansing:" );
					m.SendMessage( "" + summoneditem + " Items deleted" );
					m.SendMessage( "" + summonedmob + " Mobiles deleted" );
					m.SendMessage( "" + killed + " Mobiles killed" );
				}
			}
    		
    		this.Cleanse = null;
    		this.Cleanse = new CleanseTimer( this.Frequency, this );
    		this.Cleanse.Start();
        }
		
		public static void HandleAnimalTrainers( List<AnimalTrainer> trainers )
		{
			for( int i = 0; i < trainers.Count; i++ )
        	{
        		try
        		{
	        		AnimalTrainer m = trainers[i] as AnimalTrainer;
	        		
	        		if( m.ChargedLastTime )
	        			m.ChargedLastTime = false;
	        		
	        		else if( m.Stabled != null )
	        		{
	        			m.ChargedLastTime = true;
	        			
	        			for( int a = 0; a < m.Stabled.Count; a++ )
	        			{
	        				Mobile mob = m.Stabled[a] as Mobile;
	        				
	        				if( !(mob is BaseCreature) )
	        				{
	        					m.Stabled.Remove( mob );
	        					mob.Delete();
	        				}
	        				
	        				else if( mob is BaseCreature )
	        				{
	        					BaseCreature bc = mob as BaseCreature;
	        					
	        					if( !(bc.StabledOwner != null && !bc.StabledOwner.Deleted && bc.StabledOwner.BankBox != null && bc.StabledOwner.BankBox.ConsumeTotal(typeof(Copper), 1)) )
	        					{
		        					m.Stabled.Remove( mob );
		        					mob.Delete();
	        					}
	        				}
	        			}
	        		}
        		}
        		
        		catch(Exception e)
        		{
        			Console.WriteLine(e.Message);
        		}
        	}
		}
	
		private void ThrowError()
	    {
	    	foreach ( NetState state in NetState.Instances )
			{
				Mobile m = state.Mobile;
	
				if ( m != null && m.AccessLevel > AccessLevel.Player )
				{
					m.SendMessage( "World controller encountered an error while cleansing the map." );
				}
			}
	    }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 );
			writer.Write( (List<Item>)m_Guilds );
			writer.Write( (bool) m_FixedMercs );
			writer.Write( (int) m_Sequence );
			writer.Write( (TimeSpan) m_Frequency );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version > 1 )
				m_Guilds = (List<Item>)reader.ReadStrongItemList();
			
			if( version > 0 )
				m_FixedMercs = reader.ReadBool();
			
			m_Sequence = reader.ReadInt();
			m_Frequency = reader.ReadTimeSpan();
			
			if( this.Sequence == 1 )
			{
				if( this.Cleanse != null )
				{
					this.Cleanse.Stop();
					this.Cleanse = null;
				}
				
				this.Cleanse = new CleanseTimer( Frequency, this );
				this.Cleanse.Start();
			}
		}
		
		public class CleanseTimer : Timer
        {
			private WorldController m_wc;
			
            public CleanseTimer( TimeSpan freq, WorldController wc )
                : base( freq )
            {
            	m_wc = wc;
            }

            protected override void OnTick()
            {
            	m_wc.CleanseTheWorld();
            }           	
        }
	}
}
