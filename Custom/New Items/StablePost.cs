using System;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;
using Server.Multis;
using Server.Gumps;
using Server.ContextMenus;
using System.Collections.Generic;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x14E8, 0x14E7 )]
	public class StablePost : Item
	{
		[CommandProperty( AccessLevel.GameMaster )]
		public int MinRange { get { return 1; } }
		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxRange { get { return 5; } }
		[CommandProperty( AccessLevel.GameMaster )]
		public static int DefaultWander { get { return 2; } }
		private BaseCreature m_Controlled;
		private Mobile m_Owner;
		private int m_HomeRange = DefaultWander;
		private Server.Mobiles.FightMode m_Mode;
		private Server.Mobiles.OrderType m_Order;
		private int m_Loyal;
		private bool m_IsBonded;
		private DateTime m_OwnerAbandonTime;
		private bool m_Command;
		private double m_MinTameSkill;
		private DateTime m_BondingBegin;
		private int m_InstaSell;
		private int m_LastBid;
		private int m_CurrentBid;
		private int m_StartingBid;
		private int m_BidInc;
		private bool m_Bidding;
		private int m_NumBids;
		private Mobile m_Last;
		private Mobile m_Current;
		private double m_Hours = 72.0;
		private DateTime m_AuctionStart;
		private DateTime m_StabledDate = DateTime.MinValue;

		public BaseCreature Controlled { get { return m_Controlled; } set { m_Controlled = value; } }
		public Mobile Owner { get { return m_Owner; } set { m_Owner = value; } }
		[CommandProperty( AccessLevel.Player )]
		public int HomeRange { get { if ( m_HomeRange > 5 ) m_HomeRange = 5; else if ( m_HomeRange <= 0 ) m_HomeRange = 0; return m_HomeRange; } set { if ( value > 5 ) value = 5; else if ( value <= 0 ) value = 0; m_HomeRange = value; } }
		public Server.Mobiles.FightMode Mode { get { return m_Mode; } set { m_Mode = value; } }
		public int Loyal { get { return m_Loyal; } set { m_Loyal = value; } }
		public Server.Mobiles.OrderType Order { get { return m_Order; } set { m_Order = value; } }
		public DateTime OwnerAbandonTime { get { return m_OwnerAbandonTime; } set { m_OwnerAbandonTime = value; } }
		public bool IsBonded { get { return m_IsBonded; } set { m_IsBonded = value; } }
		public double MinTameSkill { get { return m_MinTameSkill; } set { m_MinTameSkill = value; } }
		public bool Command { get { return m_Command; } set { m_Command = value; } }
		public DateTime BondingBegin { get { return m_BondingBegin; } set { m_BondingBegin = value; } }
		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime StabledDate { get { return m_StabledDate; } set { m_StabledDate = value; } }

		#region Auction Stuff
		public int InstaSell { get { return m_InstaSell; } set { m_InstaSell = value; } }
		public int LastBid { get { return m_LastBid; } set { m_LastBid = value; } }
		public int CurrentBid { get { return m_CurrentBid; } set { m_CurrentBid = value; } }
		public int StartingBid { get { return m_StartingBid; } set { m_StartingBid = value; } }
		public int BidInc { get { return m_BidInc; } set { m_BidInc = value; } }
		public bool Bidding { get { return m_Bidding; } set { m_Bidding = value; } }
		public int NumBids { get { return m_NumBids; } set { m_NumBids = value; } }
		public Mobile Last { get { return m_Last; } set { m_Last = value; } }
		public Mobile Current { get { return m_Current; } set { m_Current = value; } }
		public double Hours { get { return m_Hours; } set { m_Hours = value; } }
		private TimeSpan AuctionLength { get { return TimeSpan.FromHours( Hours ); } }
		public DateTime AuctionStart { get { return m_AuctionStart; } set { m_AuctionStart = value; } }
		#endregion
		[CommandProperty( AccessLevel.GameMaster )]
		public override bool HandlesOnMovement { get { return ( Controlled != null ? true : false ); } } // Tell the core that we implement OnMovement
		public bool East { get { return this.ItemID == 0x14E7; } }
		#region Constructors
		[Constructable]
		public StablePost() : base( 0x14E7 )
		{
			Name = "Hitching Post";
			Movable = false;
		}

		[Constructable]
		public StablePost( bool east ) : base( east ? 0x14E7 : 0x14E8 )
		{
			Name = "Hitching Post";
			Movable = false;
		}

		public StablePost( Serial serial ) : base( serial )
		{
		}
		#endregion

		public override void OnDoubleClick( Mobile from )
		{
			if ( Owner == null )
			{
				from.Target = new StablePostTarget( this );
				from.SendMessage( "What do you wish to use this on?" );
			}
			if ( Owner != null && from == Owner )
			{
				from.Target = new StablePostTarget( this );
				from.SendMessage( "Target the animal you wish to release from the stable." );
			}
			else if ( Owner != null && Owner != from )
			{
				from.SendMessage( "This is under the control of another owner." ); 
			}
		}

		public override void Delete()
		{
			if ( Controlled == null )
				base.Delete();
			else
			{
				BaseCreature bc = this.Controlled;
				
				try
				{
					bc.Delete();
					base.Delete();
				}
				catch
				{
					base.Delete();
				}
			}
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			base.OnMovement( m, oldLocation );
			if ( m is BaseCreature )
			{
				if ( (BaseCreature)m == Controlled )
				{
					if ( ( (BaseCreature)m ).Loyalty <= 100 )
						( (BaseCreature)m ).Loyalty = 100;
					if ( ( (BaseCreature)m ).RangeHome != HomeRange )
						( (BaseCreature)m ).RangeHome = HomeRange;
				}
			}
		}

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
		}

        public static void GetContextMenuEntries(Mobile from, Item item, List<ContextMenuEntry> list)
		{
			list.Add( new MenuEntry( from, item ) );
		}

		private class MenuEntry : ContextMenuEntry
		{
			private StablePost m_Item;
			private Mobile m_Mobile;

			public MenuEntry( Mobile from, Item item ) : base( 2132 ) // uses "Configure" entry
			{
				m_Item = item as StablePost;
				m_Mobile = from;
			}

			public override void OnClick()
			{
				// send gump
				m_Mobile.CloseGump( typeof( HitchGump ) );
				m_Mobile.SendGump( new HitchGump( m_Item ) );
			}
		}

		#region Serialization
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)3 ); // version

			writer.Write( (DateTime)m_StabledDate );
			writer.Write( (int)m_InstaSell );
			writer.Write( (int)m_LastBid );
			writer.Write( (int)m_CurrentBid );
			writer.Write( (int)m_StartingBid );
			writer.Write( (int)m_BidInc );
			writer.Write( (bool)m_Bidding );
			writer.Write( (int)m_NumBids );
			writer.Write( (Mobile)m_Last );
			writer.Write( (Mobile)m_Current );
			writer.Write( (double)m_Hours );
			writer.Write( (DateTime)m_AuctionStart );

			writer.Write( (bool)m_Command );
			writer.Write( (DateTime)m_BondingBegin );

			writer.Write( (int)m_HomeRange );
			writer.Write( (int)m_Mode );
			writer.Write( (int)m_Loyal );
			writer.Write( (int)m_Order );
			writer.Write( (Mobile)m_Owner );
			writer.Write( (Mobile)m_Controlled );
			writer.Write( (bool)m_IsBonded );
			writer.Write( (DateTime)m_OwnerAbandonTime );
			writer.Write( (int)m_MinTameSkill );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
				{
					m_StabledDate = reader.ReadDateTime();
					goto case 2;
				}
				case 2:
				{
					m_InstaSell = reader.ReadInt();
					m_LastBid = reader.ReadInt();
					m_CurrentBid = reader.ReadInt();
					m_StartingBid = reader.ReadInt();
					m_BidInc = reader.ReadInt();
					m_Bidding = reader.ReadBool();
					m_NumBids = reader.ReadInt();
					m_Last = reader.ReadMobile();
					m_Current = reader.ReadMobile();
					m_Hours = reader.ReadDouble();
					m_AuctionStart = reader.ReadDateTime();
					goto case 1;
				}
				case 1:
				{
					m_Command = reader.ReadBool();
					m_BondingBegin = reader.ReadDateTime();
					goto case 0;
				}
				case 0:
				{
					m_HomeRange = reader.ReadInt();
					m_Mode = (Server.Mobiles.FightMode)reader.ReadInt();
					m_Loyal = reader.ReadInt();
					m_Order = (Server.Mobiles.OrderType)reader.ReadInt();
					m_Owner = reader.ReadMobile();
					m_Controlled = (BaseCreature)reader.ReadMobile();
					m_IsBonded = reader.ReadBool();
					m_OwnerAbandonTime = reader.ReadDateTime();
					m_MinTameSkill = reader.ReadInt();
					break;
				}
			}
			
			if( this.Owner != null && DateTime.Compare( DateTime.MinValue, this.StabledDate ) == 0 )
				this.StabledDate = DateTime.Now;
		}
		#endregion

		public void Say( string args )
		{
			PublicOverheadMessage( MessageType.Regular, 0x3B2, false, args );
		}
		
		public void DoRelease( object target )
		{
			BaseCreature c = (BaseCreature)target;
			c.ControlMaster = this.Owner;
			c.Home = this.Owner.Location;
			c.RangeHome = 0;
			c.Loyalty = this.Loyal;
			this.Loyal = 100;
			c.IsBonded = this.IsBonded;
			this.IsBonded = false;
			c.FightMode = this.Mode;
			this.Mode = FightMode.None;
			c.ControlOrder = this.Order;
			this.Order = OrderType.None;
			c.Controlled = this.Command;
			this.Command = false;
			c.Blessed = false;
			c.BondingBegin = this.BondingBegin;
			this.BondingBegin = DateTime.MaxValue;
			c.OwnerAbandonTime = this.OwnerAbandonTime;
			this.OwnerAbandonTime = DateTime.MaxValue;
			c.MinTameSkill = this.MinTameSkill;
			this.MinTameSkill = 0.0;
			this.Controlled = null;
			this.Owner = null;
			c.Frozen = false;
			this.StabledDate = DateTime.MinValue;

            if (c is BaseBreedableCreature)
            {
                BaseBreedableCreature pet = c as BaseBreedableCreature;
                pet.UpdateSpeeds();
            }
		}

		private class StablePostTarget : Target
		{
			private StablePost m_Post;

			public StablePostTarget( StablePost p ) : base( 10, false, TargetFlags.None )
			{
				m_Post = p;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if ( target == from )
					m_Post.Say( "You can't place yourself in a stable!" );
				else if ( target is StablePost )
				{
					if ( (StablePost)target == m_Post )
					{
						if ( m_Post.Controlled != null )
							m_Post.Say( "Release the pet controlled by this first." );
						else
						{
							BaseHouse house = BaseHouse.FindHouseAt( m_Post );

							if ( house != null && ( house.IsOwner( from ) || from.AccessLevel >= AccessLevel.GameMaster ) )
							{
								Effects.PlaySound( m_Post.GetWorldLocation(), m_Post.Map, 0x3B3 );
								from.SendMessage( "You redeed your StablePost." );

								from.AddToBackpack( new StablePostDeed( m_Post.East ) );
								m_Post.Delete();
							}
						}
					}
				}
				else if ( target is PlayerMobile )
					m_Post.Say( "You can't place other players in a stable!" );
				else if ( target is Item )
					m_Post.Say( "Why would you want to stable an item?" );
				else if ( Server.Spells.SpellHelper.CheckCombat( from ) )
					m_Post.Say( "You cannot stable your pet while you're fighting." );
				else if ( ( target is BaseCreature ) && m_Post.Controlled == null )
				{
					BaseCreature c = (BaseCreature)target;

					if ( c.ControlMaster == null || !c.Controlled )
						m_Post.Say( "You can only stable a pet that has been tamed." );
					if ( c.ControlMaster != from && c.Controlled )
						m_Post.Say( "You can only stable a pet you control." );
					else if ( c.Summoned )
						m_Post.Say( "You cannot stable a summoned creature." );
					//else if ( !(c is BaseMount) )
					//	m_Post.Say( "You can only stable mounts." );
					else if ( c.Combatant != null && c.InRange( c.Combatant, 12 ) && c.Map == c.Combatant.Map )
						m_Post.Say( "Your pet is fighting, You cannot stable it yet." );
					else if ( c.Controlled && c.ControlMaster == from )
					{
						m_Post.Owner = c.ControlMaster;
						m_Post.Controlled = c;
						c.Home = m_Post.Location;
						c.RangeHome = m_Post.HomeRange;
						m_Post.Loyal = c.Loyalty;
						c.Loyalty = 100;
						m_Post.IsBonded = c.IsBonded;
						m_Post.Mode = c.FightMode;
						c.FightMode = FightMode.None;
						m_Post.Order = c.ControlOrder;
						c.ControlOrder = OrderType.None;
						m_Post.Command = c.Controlled;
						c.Blessed = true;
						m_Post.BondingBegin = c.BondingBegin;
						c.BondingBegin = DateTime.MaxValue;
						m_Post.OwnerAbandonTime = c.OwnerAbandonTime;
						c.OwnerAbandonTime = DateTime.MaxValue;
						m_Post.MinTameSkill = c.MinTameSkill;
						c.MinTameSkill = 240.0;
						c.ControlMaster = null;
						c.Frozen = true;
						m_Post.StabledDate = DateTime.Now;
					}
				}
				else if ( ( target is BaseCreature ) && m_Post.Controlled != null )
				{
					if ( m_Post.Controlled == (BaseCreature)target && m_Post.Owner == from )
					{
						if( from.Followers + ((BaseCreature)target).ControlSlots > from.FollowersMax )
							from.SendMessage( "You have too many followers already." );
						
						else
							m_Post.DoRelease( target );
					}
					else
						from.SendMessage( "This post already has a creature stabled. (Unstable the other first)" );
				}
			}
		}
	}

	[Flipable( 0x14F0, 0x14EF )]
	public class StablePostDeed : Item
	{
		private bool m_East;
		public bool East { get { return m_East; } set { m_East = value; } }

		[Constructable]
		public StablePostDeed() : base( 0x14F0 )
		{
			East = Utility.RandomBool();
			if ( East )
				Name = "hitching post east deed";
			else
				Name = "hitching post south deed";
			Weight = 1.0;
		}

		[Constructable]
		public StablePostDeed( bool east ) : base( 0x14F0 )
		{
			East = east;
			if ( East )
				Name = "hitching post east deed";
			else
				Name = "hitching post south deed";
			Weight = 1.0;
		}

		public StablePostDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
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
			if ( IsChildOf( from.Backpack ) )
				from.Target = new InternalTarget( this );
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		private class InternalTarget : Target
		{
			private StablePostDeed m_Deed;

			public InternalTarget( StablePostDeed deed ) : base( -1, true, TargetFlags.None )
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

				StablePost post = new StablePost( m_Deed.East );
				BaseHouse house = BaseHouse.FindHouseAt( from.Location, from.Map, 20 );

				if ( m_Deed.IsChildOf( from.Backpack ) )
				{
					Server.Spells.SpellHelper.GetSurfaceTop( ref p );

					if ( house != null && house.IsInside( from ) && house.IsOwner( from ) )
					{
						post.MoveToWorld( new Point3D( p ), map );
						m_Deed.Delete();
					}
					else if ( house != null && house.IsInside( from ) && !house.IsOwner( from ) )
					{
						from.SendLocalizedMessage( 500274 ); // You can only place this in a house that you own!
						post.Delete();
					}
					else if ( house == null )
					{
						from.SendLocalizedMessage( 500269 ); // You cannot build that there.
						post.Delete();
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

namespace Server.Gumps
{
	public class HitchGump : Gump
	{
		private StablePost m_Post;
		private int temp;
		public HitchGump( StablePost post ) : base( 0, 0 )
		{
			m_Post = post;
			temp = m_Post.HomeRange;
			Dragable = true;

			AddPage( 0 );
			AddBackground( 0, 0, 248, 122, 9270 );
			AddLabel( 105, 75, 1071, Convert.ToString( temp ) );
			AddLabel( 20, 20, 1071, @"Owner:" );
			AddLabel( 75, 20, 1071, m_Post.Owner.Name );
			AddLabel( 20, 45, 1071, @"Pet:" );
			AddLabel( 75, 45, 1071, m_Post.Controlled.Name );
			AddLabel( 20, 75, 1071, @"HomeRange:" );
			AddButton( 150, 80, 2223, 2223, 1, GumpButtonType.Reply, 0 );
			AddButton( 170, 80, 2224, 2224, 2, GumpButtonType.Reply, 0 );
			AddButton( 200, 75, 1154, 1155, 3, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int button = info.ButtonID;

			switch ( button )
			{
				case 1:
				{
					if ( temp > m_Post.MinRange )
						temp -= 1;
					else
						sender.Mobile.SendMessage( "You cant lower this value any further." );
					m_Post.HomeRange = temp;
					sender.Mobile.CloseGump( typeof( HitchGump ) );
					sender.Mobile.SendGump( new HitchGump( m_Post ) );
					break;
				}
				case 2:
				{
					if ( temp < m_Post.MaxRange )
						temp += 1;
					else
						sender.Mobile.SendMessage( "You cant raise this value any further." );
					m_Post.HomeRange = temp;
					sender.Mobile.CloseGump( typeof( HitchGump ) );
					sender.Mobile.SendGump( new HitchGump( m_Post ) );
					break;
				}
				case 3:
				{
					if ( temp > 5 )
						temp = 5;
					if ( temp < 0 )
						temp = 0;

					m_Post.HomeRange = temp;
					m_Post.Controlled.RangeHome = m_Post.HomeRange;
					sender.Mobile.CloseGump( typeof( HitchGump ) );
					break;
				}
			}
		}
	}
}
