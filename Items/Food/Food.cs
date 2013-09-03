// Advance Cooking System Script by Ricrick
// modifications and merging to beta 35 by Alari
// - see "old cooking readme.txt" and "new cooking readme.txt"


using System;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using Server.Misc;

namespace Server.Items
{
	public enum RotStage
	{
		None = 0,
		Moldy,
		Rotten
	}
	public abstract class Food : Item
	{
		private Mobile m_Poisoner;
		private Poison m_Poison;
        private Disease m_Disease;
		private int m_FillFactor;

		private int M_HitsBonus;
		private int M_ManaBonus;
		private DateTime m_Creation;
		private TimeSpan m_RotTimeElapsed = TimeSpan.Zero;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan RotTimeElapsed{ get{ return m_RotTimeElapsed; } set{ m_RotTimeElapsed = value; } }
		
		public virtual int HitsBonus{	get { return 0; } set { M_HitsBonus = value; } }
		public virtual int ManaBonus{	get { return 0; } set { M_ManaBonus = value; } }
		
		public virtual TimeSpan MoldTime{ get { return TimeSpan.FromHours( 7*8 ); } } // 7 IG days
		public virtual TimeSpan RotTime{ get { return TimeSpan.FromHours( 11*8 ); } } // 11 IG days
		
		private RotStage m_RotStage;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public RotStage RotStage { 
			get { return m_RotStage; }
			set 
			{ 
					m_RotStage = value;
					if ( value == RotStage.None )
						Hue = 0;
					else if ( value == RotStage.Moldy )
						Hue = 2669;
					else // rotten
						Hue = 2964;

					InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Poisoner
		{
			get { return m_Poisoner; }
			set { m_Poisoner = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison
		{
			get { return m_Poison; }
			set { m_Poison = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public Disease Disease
        {
            get { return m_Disease; }
            set { m_Disease = value; }
        }

		[CommandProperty( AccessLevel.GameMaster )]
		public int FillFactor
		{
			get { return m_FillFactor; }
			set { m_FillFactor = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster)]
		public DateTime Creation
		{
			get{ return m_Creation; }
			set{ m_Creation = value; }
		}

		public Food( int itemID ) : this( 1, itemID )
		{
		}

		public Food( int amount, int itemID ) : base( itemID )
		{
			Stackable = true;
			Amount = amount;
			m_FillFactor = 1;
			Creation = DateTime.Now;
			m_RotStage = RotStage.None;
			CheckRot();
		}

		public Food( Serial serial ) : base( serial )
		{
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from.Alive )
				list.Add( new ContextMenus.EatEntry( from, this ) );
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_RotStage != RotStage.None )
				list.Add( 1060847, "{0}\t{1}", "  " + m_RotStage.ToString(), " " ); // ~1_val~ ~2_val~

            if (RootParentEntity is PlayerMobile)
            {
                PlayerMobile player = RootParentEntity as PlayerMobile;

                if (player.Feats.GetFeatLevel(FeatList.Cooking) > 2)
                {
                    if (m_RotStage == RotStage.None)
                        list.Add(1060847, "{0}\t{1}", "Time until moldy: ", GetTimeToMold());
                    else if (m_RotStage == RotStage.Moldy)
                        list.Add(1060847, "{0}\t{1}", "Time until rotten:  ", GetTimeToRot());
                }
            }
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;

			if ( from.InRange( this.GetWorldLocation(), 1 ) )
			{
				Eat( from );
			}
		}

		private static void RotCallback( object state )
		{
			Food food = state as Food;
			if ( food == null )
				return;
			food.CheckRot();
		}
		
		public override void OnPlacedForSale( Mobile vendor )
		{
			RotTimeElapsed = DateTime.Now - Creation;
		}
		
		public override void OnRemovedFromSale( Mobile vendor )
		{
			Creation = DateTime.Now - RotTimeElapsed;
			RotTimeElapsed = TimeSpan.Zero;
		}
		
		public virtual void CheckRot()
		{
			/*if( RootParentEntity != null && RootParentEntity is PlayerVendor )
			{
				Timer.DelayCall(TimeSpan.FromHours( 1 ), new TimerStateCallback( RotCallback ), this );
				return;
			}
				
			TimeSpan nextCheck = TimeSpan.Zero;
			switch( (int)m_RotStage )
			{
				case (int)RotStage.None:
				{
					if ( DateTime.Compare( DateTime.Now, Creation + RotTime ) > 0 )
						RotStage = RotStage.Rotten;
					else if ( DateTime.Compare( DateTime.Now, Creation + MoldTime ) > 0 )
					{
						RotStage = RotStage.Moldy;
						nextCheck = RotTime;
					}
					else
						nextCheck = MoldTime - (DateTime.Now-Creation);
					break;
				}
				
				case (int)RotStage.Moldy:
				{
					if ( DateTime.Compare( DateTime.Now, Creation + RotTime ) > 0 )
						RotStage = RotStage.Rotten;
					else
						nextCheck = RotTime - (DateTime.Now-Creation);
					break;
				}
			}
			
			if ( nextCheck != TimeSpan.Zero )
				Timer.DelayCall( nextCheck + TimeSpan.FromSeconds( 5 ), new TimerStateCallback( RotCallback ), this );*/
		}
		
        string GetTimeToRot()
        {
            DateTime rot = Creation.Add(RotTime);
            TimeSpan rotSpan = (rot - DateTime.Now);

            return FormatTimeSpan(rotSpan);
        }

	    string FormatTimeSpan(TimeSpan rotSpan)
	    {
	        return string.Format("{0:D2}d:{1:D2}h:{2:D2}m",
	                             rotSpan.Days,        
	                             rotSpan.Hours,
	                             rotSpan.Minutes);
	    }

	    string GetTimeToMold()
        {
            DateTime mold = (Creation.Add(MoldTime));
	        TimeSpan moldSpan = (mold - DateTime.Now);

	        return FormatTimeSpan(moldSpan);
        }

		public void ApplyRotPoison( Mobile to )
		{
			if ( m_RotStage == RotStage.None || to == null )
				return;
			
			Dictionary<PoisonEffectEnum, int> effects = new Dictionary<PoisonEffectEnum, int>();
			int speed, duration;
			speed = 3;
			duration = 1;
			if ( m_RotStage == RotStage.Rotten )
			{
				speed*=2;
				duration*=2;
			}
			effects.Add( PoisonEffectEnum.DamageStamina, 30 );
			effects.Add( PoisonEffectEnum.DamageMana, 30 );
			PoisonEffect.Poison( to, null, effects, duration*60, speed, false );

            if (Utility.RandomBool())
                HealthAttachment.TrySpreadDiease(Disease.Dysentery, this);
            else
            {
                if (Utility.RandomMinMax(1, 100) == 1)
                    HealthAttachment.TrySpreadDiease(Disease.Bile, this);
            }
		}
		
		public override void OnAfterDuped( Item newItem )
		{
			Food food = newItem as Food;
			if ( food != null )
			{
				food.RotStage = m_RotStage;
				food.Creation = m_Creation;
			}
		}
			
		public override bool StackWith( Mobile from, Item dropped )
		{
			bool retVal = false;
			Food food = dropped as Food;
			if ( food != null )
			{
				PoisonedFoodAttachment newAttachment = null;
				PoisonedFoodAttachment attachment = XmlAttach.FindAttachment( food, typeof( PoisonedFoodAttachment ) ) as PoisonedFoodAttachment;
				if ( attachment != null )
					newAttachment = new PoisonedFoodAttachment( attachment.Effects, attachment.PoisonDuration, attachment.PoisonActingSpeed, attachment.Poisoner );
				if ( m_RotStage == food.RotStage )
					retVal = base.StackWith( from, dropped );
				if ( retVal ) // stacks
				{
					int currentStrength = 0;
					int candidateStrength = 0;
					// check if there's poison already, if so, use the strongest of the two
					attachment = XmlAttach.FindAttachment( this, typeof( PoisonedFoodAttachment ) ) as PoisonedFoodAttachment;
					if ( attachment != null && newAttachment != null )
					{
						Dictionary<PoisonEffectEnum, int> effects = attachment.Effects;
						foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in effects )
							currentStrength += kvp.Value;
						effects = newAttachment.Effects;
						foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in effects )
							candidateStrength += kvp.Value;
						if ( candidateStrength > currentStrength )
						{
							attachment.Delete();
							XmlAttach.AttachTo( this, newAttachment );
						}
					}
					else if ( newAttachment != null )
						XmlAttach.AttachTo( this, newAttachment ); // had no poison previously
				}
			}
			
			return retVal;
		}
		
		public virtual bool Eat( Mobile from )
		{
			DamageEntry de = from.FindMostRecentDamageEntry( false );
			if ( de != null && DateTime.Compare( DateTime.Now, de.LastDamage + TimeSpan.FromMinutes( 5 ) ) < 0 )
			{
				from.SendMessage( "Your heart is still pounding from the battle. You can't bring yourself to eat right now." );
				return false;
			}
			
			// Fill the Mobile with FillFactor
			if ( FillHunger( from, m_FillFactor, HitsBonus, ManaBonus ) )  // added HitsBonus, ManaBonus - alari
			{
				// Play a random "eat" sound
				from.PlaySound( Utility.Random( 0x3A, 3 ) );

				if ( from.Body.IsHuman && !from.Mounted )
					from.Animate( 34, 5, 1, true, false, 0 );
				
				PoisonedFoodAttachment attachment = XmlAttach.FindAttachment( this, typeof( PoisonedFoodAttachment ) ) as PoisonedFoodAttachment;
				
				if ( attachment != null )
				{
					attachment.OnConsumed( from );
					attachment.Delete(); // only one piece of food can be poisoned in the stack
				}

                if (m_RotStage != RotStage.None && from is PlayerMobile && ((PlayerMobile)from).Nation != Nation.Mhordul)
                {
                    ApplyRotPoison(from);
                }

                if(Disease != Disease.None)
                    HealthAttachment.GetHA(from).TryCatchDisease(Disease);

				Consume();

				return true;
			}

			return false;
		}

		static public bool FillHunger( Mobile from, int fillFactor, int hitsbonus, int manabonus )  // added: int hitsbonus, int manabonus - alari
		{
			if( from is PlayerMobile && ((PlayerMobile)from).IsVampire)
				return true;
			
			if ( from.Hunger >= 20 )
			{
                from.SendLocalizedMessage( 500872 ); // You manage to eat the food, but you are stuffed!
				return true;
			}
			
			int iHunger = from.Hunger + fillFactor;
			if ( from.Stam < from.StamMax )
				from.Stam += Utility.Random( 6, 3 ) + fillFactor/5;//restore some stamina
			if ( from.Hits < from.HitsMax && hitsbonus != 0 )
				from.Hits += Utility.RandomMinMax( hitsbonus, fillFactor + hitsbonus );//restore some health
			if ( from.Mana < from.ManaMax && manabonus != 0 )
				from.Mana += Utility.RandomMinMax( manabonus, fillFactor + manabonus );//restore some mana

			if ( iHunger >= 20 )
			{
				from.Hunger = 20;
				from.SendLocalizedMessage( 500872 ); // You manage to eat the food, but you are stuffed!
			}
			else
			{
				from.Hunger = iHunger;

				if ( iHunger < 5 )
					from.SendLocalizedMessage( 500868 ); // You eat the food, but are still extremely hungry.
				else if ( iHunger < 10 )
					from.SendLocalizedMessage( 500869 ); // You eat the food, and begin to feel more satiated.
				else if ( iHunger < 15 )
					from.SendLocalizedMessage( 500870 ); // After eating the food, you feel much less hungry.
				else
					from.SendLocalizedMessage( 500871 ); // You feel quite full after consuming the food.
			}
			
			FoodDecayTimer.CalculatePenalty( from );

			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 8 ); // version

            writer.Write( (int) m_Disease );
			
			writer.Write( (TimeSpan) m_RotTimeElapsed );
			
			writer.Write( (int) m_RotStage );
				
			writer.Write( (DateTime) m_Creation );

			writer.Write( m_Poisoner );

			Poison.Serialize( m_Poison, writer );
			writer.Write( m_FillFactor );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					switch ( reader.ReadInt() )
					{
						case 0: m_Poison = null; break;
						case 1: m_Poison = Poison.Lesser; break;
						case 2: m_Poison = Poison.Regular; break;
						case 3: m_Poison = Poison.Greater; break;
						case 4: m_Poison = Poison.Deadly; break;
					}

					break;
				}
				case 2:
				{
					m_Poison = Poison.Deserialize( reader );
					break;
				}
				case 3:
				{
					m_Poison = Poison.Deserialize( reader );
					m_FillFactor = reader.ReadInt();
					break;
				}
				case 4:
				{
					m_Poisoner = reader.ReadMobile();
					goto case 3;
				}
				case 5:
				{
					m_Creation = reader.ReadDateTime();
					CheckRot();
					goto case 4;
				}
				case 6:
				{
					m_RotStage = (RotStage)reader.ReadInt();
					goto case 5;
				}
				case 7:
				{
					m_RotTimeElapsed = reader.ReadTimeSpan(); 
					goto case 6;
				}
                case 8:
                {
                    m_Disease = (Disease)reader.ReadInt();
                    goto case 7;
                }
			}
		}
	}
}
