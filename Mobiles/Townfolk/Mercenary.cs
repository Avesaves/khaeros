using System; 
using System.Collections; 
using Server.Items; 
using Server.ContextMenus; 
using Server.Misc; 
using Server.Network; 

namespace Server.Mobiles 
{ 
	public class Mercenary : BaseCreature
	{ 
		public override bool WaitForRess{ get{ return (Lives >= 0); } }
		private Mobile m_Owner;
		private int m_HoldCopper;
		private Timer m_PayTimer;
		private DateTime m_NextPayTime;
		private Item m_MyCorpse;
		private int m_TrainingReceived;
		private TimeSpan m_TimeUntilPay;
		
		[CommandProperty( AccessLevel.GameMaster )]
        public TimeSpan TimeUntilPay
        {
            get { return m_TimeUntilPay; }
            set { m_TimeUntilPay = value; }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool FixStats
		{
			get{ return false; }
			set
			{
				if( value == true )
					LevelSystem.FixStatsAndSkills( this );
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
        public Item MyCorpse
        {
            get { return m_MyCorpse; }
            set { m_MyCorpse = value; }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get
			{
				if( this.ControlMaster != null && this.ControlMaster is PlayerMobile )
					return this.ControlMaster;
				
				return m_Owner; 
			}
			set{ m_Owner = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int HoldCopper
		{
			get{ return m_HoldCopper; }
			set{ m_HoldCopper = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextPayTime
		{
			get{ return m_NextPayTime; }
		}
		
		public int ChargePerDay
		{
			get
			{ 
				if( Owner != null )
				{
					if( Owner is PlayerMobile )
					{
/*                         int basevalue = (Level * XPScale) + XPScale; */
                        int basevalue = 50;
						int skillbonus = 0;
                        int pay = 0;
				
						if( Owner.Skills[SkillName.Leadership].Base > 0 )
							skillbonus = Convert.ToInt32( Owner.Skills[SkillName.Leadership].Base * ( 0.02 * (Owner as PlayerMobile).Level) );

                        skillbonus += (Owner as PlayerMobile).Feats.GetFeatLevel(FeatList.LeadershipMastery) * (Level / XPScale);

                        if ((Owner as PlayerMobile).Nation == Nation.Mhordul)
                            skillbonus *= 2;

                        /* pay = basevalue - skillbonus; */
						pay = basevalue;

                        if (pay < XPScale * 2)
                            pay = XPScale * 2;
						
						return pay;
					}
				}
				return (Level * XPScale) + XPScale; 
			}
		}

		public int ChargePerRealWorldDay
		{
			get
			{ return ChargePerDay; }
		}
		
		public int TrainingReceived
		{
			get{ return m_TrainingReceived; }
			set{ m_TrainingReceived = value; }
		}

		[Constructable] 
		public Mercenary( Nation nation ) : base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 ) 
		{ 
			this.SpeechHue = Utility.RandomDyedHue(); 

			this.Female = Utility.RandomBool();
			
			if ( this.Female ) 
			{ 
				//AddItem( new Skirt( Utility.RandomRedHue() ) ); 
				this.Body = 0x191; 
				//this.Name = NameList.RandomName( "female" ); 
				this.Name = BaseKhaerosMobile.RandomName( nation, true );
			} 
			else 
			{ 
				//AddItem( new ShortPants( Utility.RandomRedHue() ) ); 
				this.Body = 0x190; 
				//this.Name = NameList.RandomName( "male" ); 
				this.Name = BaseKhaerosMobile.RandomName( nation, false );
			} 
			
			SetStr( Utility.RandomMinMax(5, 5) );
            SetDex( Utility.RandomMinMax(5, 5) );
			SetInt( 55 );

            SetHits( Utility.RandomMinMax(45, 55) );
            SetStam( Utility.RandomMinMax(45, 55) );
			SetMana( 55 );

			SetDamage( 0, 0 );

            switch (nation)
            {
                case Nation.Southern: { Int += 1; Dex += 1; break; }
                case Nation.Western: { Dex += 1; break; }
                case Nation.Haluaroc: { Int += 1; break; }
                case Nation.Mhordul: { Str += 1; break; }
                case Nation.Tirebladd: { Stam += 1; Str += 1; break; }
                case Nation.Northern: { Hits += 1; break; }
            }
			
			this.XPScale = 1;
		  	this.StatScale = 1;
		  	this.SkillScale = 1;
		  	
		  	m_HoldCopper = 0;
		  	
			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 0 );
			SetResistance( ResistanceType.Slashing, 0 );
			SetResistance( ResistanceType.Piercing, 0 );
			SetResistance( ResistanceType.Fire, 0 );
			SetResistance( ResistanceType.Cold, 0 );
			
			SetSkill( SkillName.Anatomy, 20.0 );
			SetSkill( SkillName.Fencing, 20.0 );
			SetSkill( SkillName.Macing, 20.0 );
			SetSkill( SkillName.Swords, 20.0 );
			SetSkill( SkillName.Tactics, 20.0 );
			SetSkill( SkillName.Polearms, 20.0 );
			SetSkill( SkillName.ExoticWeaponry, 20.0 );
			SetSkill( SkillName.Axemanship, 20.0 );
			SetSkill( SkillName.UnarmedFighting, 20.0 );

			this.Fame = 1000;
			this.Karma = -1000;
			this.Lives = 1;
			this.Level = 60;
			this.NextLevel = 1000;
			this.ControlSlots = 2;

			this.VirtualArmor = 0;
			this.Title = "the Servant";
			
			this.Hue = BaseKhaerosMobile.AssignRacialHue( nation );
			this.HairItemID = BaseKhaerosMobile.AssignRacialHair( nation, this.Female );
			this.HairHue = BaseKhaerosMobile.AssignRacialHairHue( nation );
			
			if( !this.Female )
			{
				this.FacialHairItemID = BaseKhaerosMobile.AssignRacialFacialHair( nation );
				this.FacialHairHue = this.HairHue;
			}
			
			if( this.Backpack == null )
				AddItem( new Backpack() );
			
/* 			AddItem( new Boots() );
			AddItem( new LeatherArms() );
			AddItem( new LeatherLegs() );
			AddItem( new LeatherChest() );
			AddItem( new LeatherGloves() );
			AddItem( new FancyShirt()) ; */
		}
		
		public void MercWeapon( int blunt, int slash, int pierc, int min, int max )
		{
			SetDamageType( ResistanceType.Blunt, blunt );
			SetDamageType( ResistanceType.Slashing, slash );
			SetDamageType( ResistanceType.Piercing, pierc );
			SetDamage( min, max );
		}
		
		public virtual bool IsOwner( Mobile m )
		{
			if ( m.AccessLevel >= AccessLevel.GameMaster )
				return true;
			
			else
			{
				return m == Owner;
			}
		}
		
		public override bool AllowEquipFrom( Mobile from )
		{
			if ( IsOwner( from ) )
				return true;

			return base.AllowEquipFrom( from );
		}
		
		public override bool CheckNonlocalLift( Mobile from, Item item )
		{
			if ( IsOwner( from ) )
			{
				return true;
			}

			return base.CheckNonlocalLift( from, item );
		}
		
		public override bool OnDragDrop( Mobile from, Item item )
		{
			if( from is PlayerMobile && this.Owner == null && ( item is Copper || item is Silver || item is Gold ) )
			{
				int basevalue = 35;
				int skillbonus = 0;
		
				if( from.Skills[SkillName.Leadership].Base > 0 )
					skillbonus = Convert.ToInt32( from.Skills[SkillName.Leadership].Base * 0.2 );
				
				int amountcharged = ( basevalue - skillbonus );
				
				if( item is Copper && from is PlayerMobile && item.Amount < ( amountcharged * 3 ) )
				{
					this.SayTo( from, "You need to give me an initial fee equal to at least 3 days of work, which for you is " + ( amountcharged * 3 ) + " copper pieces." );
					return false;
				}
				
				if( item is Silver && from is PlayerMobile && ( item.Amount * 10 ) < ( amountcharged * 3 ) )
				{
					this.SayTo( from, "You need to give me an initial fee equal to at least 3 days of work, which for you is " + ( amountcharged * 3 ) + " copper pieces." );
					return false;
				}
				
				if( item is Gold && from is PlayerMobile && ( item.Amount * 100 ) < ( amountcharged * 3 ) )
				{
					this.SayTo( from, "You need to give me an initial fee equal to at least 3 days of work, which for you is " + ( amountcharged * 3 ) + " copper pieces." );
					return false;
				}
				
				if( (from.Followers + this.ControlSlots) > from.FollowersMax )
				{
					this.SayTo( from, "You seem to already have as many followers as you can possibly handle." );
					return false;
				}
				
				TimeSpan delay = TimeSpan.FromHours( 8 );

				m_PayTimer = new PayTimer( this, delay );
				m_PayTimer.Start();
	
				this.m_NextPayTime = DateTime.Now + delay;
				this.ControlMaster = from;
				this.Controlled = true;
				this.IsBonded = true;
				this.SayTo( from, "I will work for you from now on as long as you always pay me properly." );
			}
			
			if ( !IsOwner( from ) )
			{
				this.SayTo( from, "I can only take items from my employer." );
				return false;
			}

			if ( item is Copper )
			{
				if ( this.HoldCopper < 1000000 )
				{
					SayTo( from, 503210 ); // I'll take that to fund my services.

					this.HoldCopper += item.Amount;
					this.ControlMaster = from;
					this.Controlled = true;
					this.IsBonded = true;
					item.Delete();

					return true;
				}
				else
				{
					this.SayTo( from, "I already have as many coins as I can carry." );

					return false;
				}
			}
			
			if ( item is Silver )
			{
				if ( this.HoldCopper < 1000000 )
				{
					SayTo( from, 503210 ); // I'll take that to fund my services.

					this.HoldCopper += item.Amount * 10;
					this.ControlMaster = from;
					this.Controlled = true;
					this.IsBonded = true;
					item.Delete();

					return true;
				}
				else
				{
					this.SayTo( from, "I already have as many coins as I can carry." );

					return false;
				}
			}
			
			if ( item is Gold )
			{
				if ( this.HoldCopper < 1000000 )
				{
					SayTo( from, 503210 ); // I'll take that to fund my services.

					this.HoldCopper += item.Amount * 100;
					this.ControlMaster = from;
					this.Controlled = true;
					this.IsBonded = true;
					item.Delete();

					return true;
				}
				else
				{
					this.SayTo( from, "I already have as many coins as I can carry." );

					return false;
				}
			}
			
			AddToBackpack( item );
			return true;
		}
		
		private void StartPayment()
		{
			HandlePayment();
		}
		
		private void HandlePayment()
		{
			this.m_NextPayTime = DateTime.Now + TimeSpan.FromHours( 24 );

			int pay = 0;
			int totalCopper = 0;
			bool problem = false;

			try
			{
				pay = this.ChargePerDay;
			}
			
			catch
			{
				problem = true;
			}
			
			totalCopper = this.HoldCopper;

			if ( problem || pay > totalCopper )
			{
				this.Controlled = false;
				this.ControlMaster = null;
				this.IsBonded = false;
				this.HoldCopper = 0;
			}
			
			else
				this.HoldCopper -= pay;
			
			if( Owner != null )
			{
				TimeSpan delay = m_NextPayTime - DateTime.Now;
				m_PayTimer = new PayTimer( this, delay );
				m_PayTimer.Start();
			}
		}
		
		private class PayTimer : Timer
		{
			private Mercenary m_merc;

			public PayTimer( Mercenary merc, TimeSpan delay ) : base( delay )
			{
				m_merc = merc;

				Priority = TimerPriority.OneMinute;
			}

			protected override void OnTick()
			{
				m_merc.StartPayment();
			}
		}

		public Mercenary( Serial serial ) : base( serial ) 
		{ 
		}
		
		public void StartPayTimer()
		{
			TimeSpan delay = TimeUntilPay;
				
			if( DateTime.Compare( m_NextPayTime, DateTime.Now ) > 0 )
				TimeUntilPay = m_NextPayTime - DateTime.Now;
			
			m_PayTimer = new PayTimer( this, delay );
			m_PayTimer.Start();
		}
		
		public void StopPayTimer()
		{
			m_PayTimer.Stop();
			TimeUntilPay = TimeSpan.FromHours( 24 );
			
			if( DateTime.Compare( m_NextPayTime, DateTime.Now ) > 0 )
				TimeUntilPay = m_NextPayTime - DateTime.Now;
		}

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 11 ); // version 
			
			writer.Write( (TimeSpan) m_TimeUntilPay );
			
			writer.Write( (int) m_TrainingReceived );
			
			writer.Write( (Item) m_MyCorpse );
			/*
			writer.WriteDeltaTime( m_TimeOfDeath );

            writer.Write( m_RessTimer != null );

            if( m_RessTimer != null )
                writer.WriteDeltaTime( m_RessTime );
			
			writer.Write( (int) m_Lives );*/
			
			writer.WriteDeltaTime( (DateTime) m_NextPayTime );

			writer.Write( (Mobile) m_Owner );

			writer.Write( (int) m_HoldCopper );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt();

            if (version < 11)
                this.ControlSlots = 2;

            if (version < 10)
                SetInt(25 + this.Level);

			if( version > 8 )
				m_TimeUntilPay = reader.ReadTimeSpan();
			
			if( version < 8 )
				this.ControlSlots = 3;
			
			if( version < 7 )
				Feats = new Feats();
			
			if( version > 6 )
				m_TrainingReceived = reader.ReadInt();
			
			if( version > 2 )
			{
				m_MyCorpse = reader.ReadItem();
				
				if( version < 6 )
				{
		            TimeOfDeath = reader.ReadDeltaTime();
		
		            if( reader.ReadBool() )
		            	BeginRess( reader.ReadDeltaTime() - DateTime.Now, ( (Container)m_MyCorpse ) );
				}
			}
			
			if( version < 6 )
				Lives = reader.ReadInt();
			
			m_NextPayTime = reader.ReadDeltaTime();
			
			m_Owner = reader.ReadMobile();
			
			m_HoldCopper = reader.ReadInt();
			
			if( Owner != null && !IsStabled )
			{
				TimeSpan delay = TimeSpan.FromHours( 24 );
				
				if( DateTime.Compare( m_NextPayTime, DateTime.Now ) > 0 )
					delay = m_NextPayTime - DateTime.Now;
				
				m_PayTimer = new PayTimer( this, delay );
				m_PayTimer.Start();
			}
			
			if( version < 4 )
				LevelSystem.FixStatsAndSkills( this );
			
			if( version < 5 )
				this.ControlSlots = 2;
		}
	} 
}
