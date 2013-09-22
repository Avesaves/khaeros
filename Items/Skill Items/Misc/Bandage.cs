/**************************************
*   Bloody+Enhanced Bandage System    *
*      Distro files: Bandage.cs       *
*                                     *
*     Made by Demortris AKA Joeku     *
*             10/11/2005              *
*                                     *
* Anyone can modify/redistribute this *
*  DO NOT REMOVE/CHANGE THIS HEADER!  *
**************************************/

using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Gumps;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class Bandage : Item, IDyable
	{
		public override double DefaultWeight
		{
			get { return 0.1; }
		}
		[Constructable]
		public Bandage() : this( 1 )
		{
		}

		[Constructable]
		public Bandage( int amount ) : base( 0xE21 )
		{
			Stackable = true;
			Amount = amount;
		}

		public Bandage( Serial serial ) : base( serial )
		{
		}

		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;

			return true;
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
            DamageEntry de = from.FindMostRecentDamageEntry(false);
            if (HasBeenInCombatWithAPlayer(de) && HasMovedInThePastSecond(@from))
            {
                from.SendMessage("Your hands are still shaking from the battle. Try standing still.");
                return;
            }

			if( from is PlayerMobile )
			{
				PlayerMobile pm = from as PlayerMobile;
				if(pm.Mounted)
				  {
                	pm.SendMessage( "You cannot use bandages while mounted." );
                	return;
                }
				
				if( pm.HealingTimer != null )
                {
                	pm.SendMessage( "You are trying to heal someone already." );
                	return;
                }
			}
			
			if ( from.InRange( GetWorldLocation(), Core.AOS ? 2 : 1 ) )
			{
				from.RevealingAction();

				CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( from );
				csa.Interrupted( true );
				if ( from is PlayerMobile )
					((PlayerMobile)from).ClearHands( true );
				else
					from.ClearHands();
				
				from.SendLocalizedMessage( 500948 ); // Who will you use the bandages on?

				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendLocalizedMessage( 500295 ); // You are too far away to do that.
			}
		}

	    static bool HasMovedInThePastSecond(Mobile @from)
	    {
	        return DateTime.Compare(DateTime.Now, @from.LastMoveTime + TimeSpan.FromSeconds(1)) < 0;
	    }

	    static bool HasBeenInCombatWithAPlayer(DamageEntry de)
	    {
	        return de != null && de.Damager is PlayerMobile && DateTime.Compare(DateTime.Now, de.LastDamage + TimeSpan.FromMinutes(2)) < 0;
	    }

	    private class InternalTarget : Target
		{
			private Bandage m_Bandage;

			public InternalTarget( Bandage bandage ) : base( 1, false, TargetFlags.None )
			{
				m_Bandage = bandage;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Bandage.Deleted )
					return;

				if( targeted == from && from is PlayerMobile && ((PlayerMobile)from).IsVampire )
				{
					from.SendMessage( "You cannot heal yourself with bandages." );
					return;
				}
				
				if ( targeted is Mobile )
				{
					if( !((Mobile)targeted).Alive || (targeted is BaseCreature && ((BaseCreature)targeted).IsDeadPet) )
						from.SendMessage( "You cannot do anything for them." );
					
					else if ( from.InRange( m_Bandage.GetWorldLocation(), Core.AOS ? 2 : 1 ) )
					{
						if ( BandageContext.BeginHeal( from, (Mobile) targeted ) != null )
						{
							CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( from );
							csa.Interrupted( true );
                            if (from is PlayerMobile)
                                ((PlayerMobile)from).ClearHands(true);
                            else
                                from.ClearHands();
							int amount = m_Bandage.Amount;
	
							m_Bandage.Consume();
						}
					}
					else
					{
						from.SendLocalizedMessage( 500295 ); // You are too far away to do that.
					}
				}
				else if ( targeted is MagicalFountain )
				{
					if ( this != null )
					{
						int amount = m_Bandage.Amount;
						if ( m_Bandage.Amount > 1 )
						{
							from.SendMessage("You enhance the bandages and drop them into your pack.");
						}
						else
						{
							from.SendMessage("You enhance the bandage and drop it into your pack.");
						}
						m_Bandage.Amount = 1;
						m_Bandage.Consume();
						from.AddToBackpack( new EnhancedBandage( amount ) );
					}
				}
				else
				{
					from.SendLocalizedMessage( 500970 ); // Bandages can not be used on that.
				}
			}
		}
	}

	public class BandageContext
	{
		private Mobile m_Healer;
		private Mobile m_Patient;
		private int m_Slips;
		private Timer m_Timer;

		public Mobile Healer{ get{ return m_Healer; } }
		public Mobile Patient{ get{ return m_Patient; } }
		public int Slips{ get{ return m_Slips; } set{ m_Slips = value; } }
		public Timer Timer{ get{ return m_Timer; } }

		public void Slip()
		{
			m_Healer.SendLocalizedMessage( 500961 ); // Your fingers slip!
			++m_Slips;
		}

		public BandageContext( Mobile healer, Mobile patient, TimeSpan delay )
		{
			m_Healer = healer;
			m_Patient = patient;
			
			if( healer is PlayerMobile )
			{
				PlayerMobile m = healer as PlayerMobile;
				m.HealingTimer = new InternalTimer( this, ( delay ) );
                CombatSystemAttachment csa = CombatSystemAttachment.GetCSA(healer);
                csa.Interrupted(true);
                ((PlayerMobile)m).ClearHands(true);                   
				m.HealingTimer.Start();
			}
			
			else
			{
                healer.ClearHands();
				m_Timer = new InternalTimer( this, delay );
				m_Timer.Start();
			}
		}

		public void StopHeal()
		{
			m_Table.Remove( m_Healer );

			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;
		}

		private static Hashtable m_Table = new Hashtable();

		public static BandageContext GetContext( Mobile healer )
		{
			return (BandageContext)m_Table[healer];
		}

		public static SkillName GetPrimarySkill( Mobile m )
		{
			if ( !m.Player && (m.Body.IsMonster || m.Body.IsAnimal) )
				return SkillName.Veterinary;
			else
				return SkillName.Healing;
		}

		public static SkillName GetSecondarySkill( Mobile m )
		{
			if ( !m.Player && (m.Body.IsMonster || m.Body.IsAnimal) )
				return SkillName.AnimalLore;
			else
				return SkillName.Anatomy;
		}

		public void EndHeal()
		{
			StopHeal();

			int healerNumber = -1, patientNumber = -1;
			bool playSound = true;
			bool checkSkills = false;

			switch ( Utility.Random( 4 ))
			{ 
				case 0: m_Healer.AddToBackpack( new BloodyBandage( 1 ) );
					m_Healer.SendMessage( "You were able to recover a bloody bandage from what you used." ); break;
			}

			SkillName primarySkill = GetPrimarySkill( m_Patient );
			SkillName secondarySkill = GetSecondarySkill( m_Patient );

			BaseCreature petPatient = m_Patient as BaseCreature;

			if ( !m_Healer.Alive )
			{
				healerNumber = 500962; // You were unable to finish your work before you died.
				patientNumber = -1;
				playSound = false;
			}
			else if ( !m_Healer.InRange( m_Patient, Core.AOS ? 2 : 1 ) )
			{
				healerNumber = 500963; // You did not stay close enough to heal your target.
				patientNumber = -1;
				playSound = false;
			}
			else if ( !m_Patient.Alive || (petPatient != null && petPatient.IsDeadPet) )
			{
				m_Healer.SendMessage( "Your patient died before you could do anything for them." );
			}
			else if ( m_Patient.Poisoned )
			{
				m_Healer.SendLocalizedMessage( 500969 ); // You finish applying the bandages.

				double healing = m_Healer.Skills[primarySkill].Value;
				double anatomy = m_Healer.Skills[secondarySkill].Value;

				if ( PoisonEffect.Cure( m_Patient, (int)(healing+anatomy) ) )
				{
					healerNumber = (m_Healer == m_Patient) ? -1 : 1010058; // You have cured the target of all poisons.
					patientNumber = -1;
					//patientNumber = 1010059; // You have been cured of all poisons.
				}
				else
				{
					healerNumber = 1010060; // You have failed to cure your target!
					patientNumber = -1;
				}
			}
			else if ( BleedAttack.IsBleeding( m_Patient ) )
			{
				healerNumber = -1;
				patientNumber = 1060167; // The bleeding wounds have healed, you are no longer bleeding!

				BleedAttack.EndBleed( m_Patient, true );
			}
			else if ( MortalStrike.IsWounded( m_Patient ) )
			{
				healerNumber = ( m_Healer == m_Patient ? 1005000 : 1010398 );
				patientNumber = -1;
				playSound = false;
			}
			else if ( m_Patient.Hits == m_Patient.HitsMax )
			{
				healerNumber = 500967; // You heal what little damage your patient had.
				patientNumber = -1;
			}
			else
			{
				checkSkills = true;
				patientNumber = -1;

				double healing = m_Healer.Skills[primarySkill].Value;
				double anatomy = m_Healer.Skills[secondarySkill].Value;
				double chance = ((healing + 10.0) / 100.0) - (m_Slips * 0.02);

				if ( chance > Utility.RandomDouble() && !(m_Patient is PlayerMobile && ((PlayerMobile)m_Patient).IsVampire) )
				{
					healerNumber = 500969; // You finish applying the bandages.

					double min, max;

					if ( Core.AOS )
					{
						min = (anatomy / 25.0) + (healing / 25.0) + 3.0;
						max = (anatomy / 12.5) + (healing / 12.5) + 3.0;
					}
					else
					{
						min = (anatomy / 20.0) + (healing / 10.0) + 4.0;
						max = (anatomy / 10.0) + (healing / 5.0) + 4.0;
					}

					double toHeal = min + (Utility.RandomDouble() * (max - min));

					if ( m_Patient.Body.IsMonster || m_Patient.Body.IsAnimal )
						toHeal += m_Patient.HitsMax / 100;

					if ( Core.AOS )
						toHeal -= toHeal * m_Slips * 0.35; // TODO: Verify algorithm
					else
						toHeal -= m_Slips * 4;

					if ( toHeal < 1 )
					{
						toHeal = 1;
						healerNumber = 500968; // You apply the bandages, but they barely help.
					}
					m_Patient.Heal( (int) toHeal, false );
					m_Patient.LocalOverheadMessage( MessageType.Regular, 170, false, "+" + Convert.ToInt32( toHeal ) );
				}
				
				else
				{
					healerNumber = 500968; // You apply the bandages, but they barely help.
					playSound = false;
				}
			}

			if ( healerNumber != -1 )
				m_Healer.SendLocalizedMessage( healerNumber );

			if ( patientNumber != -1 )
				m_Patient.SendLocalizedMessage( patientNumber );

			if ( playSound )
				m_Patient.PlaySound( 0x57 );

			if ( checkSkills )
			{
				m_Healer.CheckSkill( secondarySkill, 0.0, 120.0 );
				m_Healer.CheckSkill( primarySkill, 0.0, 120.0 );
			}
			
			if( m_Healer is PlayerMobile )
			{
				PlayerMobile pm = m_Healer as PlayerMobile;
				pm.HealingTimer = null;
			}
		}

		private class InternalTimer : Timer
		{
			private BandageContext m_Context;

			public InternalTimer( BandageContext context, TimeSpan delay ) : base( delay )
			{
				m_Context = context;
				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				m_Context.EndHeal();
			}
		}

		public static BandageContext BeginHeal( Mobile healer, Mobile patient )
		{
			bool isDeadPet = ( patient is BaseCreature && ((BaseCreature)patient).IsDeadPet );

			if ( patient is BaseCreature && ((BaseCreature)patient).IsAnimatedDead )
			{
				healer.SendLocalizedMessage( 500951 ); // You cannot heal that.
			}
			else if ( !patient.Poisoned && patient.Hits == patient.HitsMax && !BleedAttack.IsBleeding( patient ) && !isDeadPet )
			{
				healer.SendLocalizedMessage( 500955 ); // That being is not damaged!
			}
			else if ( !patient.Alive && (patient.Map == null || !patient.Map.CanFit( patient.Location, 16, false, false )) )
			{
				healer.SendLocalizedMessage( 501042 ); // Target cannot be resurrected at that location.
			}
			else if ( healer.CanBeBeneficial( patient, true, true ) )
			{
				healer.DoBeneficial( patient );

				bool onSelf = ( healer == patient );
				int dex = healer.Dex;

				double seconds;
				double resDelay = ( patient.Alive ? 0.0 : 5.0 );

				if ( onSelf )
				{
					if ( Core.AOS )
						seconds = 4.55 + (0.1 * ((double)(0 - dex) / 10)); // TODO: Verify algorithm
					else
						seconds = 9.4 + (0.6 * ((double)(120 - dex) / 10));
				}
				else
				{
					if ( Core.AOS && GetPrimarySkill( patient ) == SkillName.Veterinary )
					{
						//if ( dex >= 40 )
							seconds = 2.0;
						//else
						//	seconds = 3.0;
					}
					else
					{
						if ( dex >= 100 )
							seconds = 2.0 + resDelay;
						else if ( dex >= 40 )
							seconds = 3.0 + resDelay;
						else
							seconds = 4.0 + resDelay;
					}
				}
				
				if( seconds < 2.0 )
					seconds = 2.0;

				BandageContext context = GetContext( healer );

				if ( context != null )
					context.StopHeal();

                CombatSystemAttachment csa = CombatSystemAttachment.GetCSA(healer);
                csa.Interrupted(true);
                if (healer is PlayerMobile)
                    ((PlayerMobile)healer).ClearHands(true);
                else
                    healer.ClearHands();

				context = new BandageContext( healer, patient, TimeSpan.FromSeconds( seconds ) );

				m_Table[healer] = context;

				if ( !onSelf )
					patient.SendLocalizedMessage( 1008078, false, healer.Name ); //  : Attempting to heal you.

				healer.SendLocalizedMessage( 500956 ); // You begin applying the bandages.
				return context;
			}

			return null;
		}
	}
}
