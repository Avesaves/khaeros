using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BasePoleArm : BaseMeleeWeapon, IUsesRemaining
	{
		public override int DefHitSound{ get{ return 0x237; } }
		public override int DefMissSound{ get{ return 0x238; } }
		
		public override bool Unwieldy{ get{ return true; } }
		public override bool CanUseDefensiveFormation{ get{ return true; } }
		public override bool CannotUseOnMount{ get{ return true; } }

		public override SkillName DefSkill{ get{ return SkillName.Swords; } }
		public override WeaponType DefType{ get{ return WeaponType.Polearm; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash2H; } }

		//public virtual HarvestSystem HarvestSystem{ get{ return Lumberjacking.System; } }

		private int m_UsesRemaining;
        private int m_FeatLevel;
		private bool m_ShowUsesRemaining;
		private bool m_Fixed;

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set { m_UsesRemaining = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowUsesRemaining
		{
			get { return m_ShowUsesRemaining; }
			set { m_ShowUsesRemaining = value; InvalidateProperties(); }
		}

		public BasePoleArm( int itemID ) : base( itemID )
		{
			m_UsesRemaining = 150;
		}

		public BasePoleArm( Serial serial ) : base( serial )
		{
		}
		
		/*public override bool IsStill( Mobile attacker )
		{
			if( attacker.Combatant != null )
			{
				string position = BaseWeapon.GetPosition(attacker, attacker.Combatant);
				
				if( position == "back" || position == "back flank" )
					return true;
			}
			
			double span = Math.Max( 0.7, (1.0 - (0.1 * ((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.PolearmsMastery))) );
			
			if( ((IKhaerosMobile)attacker).CombatManeuver is Misc.MountedCharge && attacker.Mounted && attacker.Stam >= ((((IKhaerosMobile)attacker).CombatManeuver.FeatLevel * 3 ) + 1) && DateTime.Compare( DateTime.Now, ((IKhaerosMobile)attacker).NextFeatUse ) > 0 )
				return true;
			   
			if( DateTime.Now > (attacker.LastMoveTime + TimeSpan.FromSeconds(span)) )
                return true;
			
			return false;
		}*/

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 4 ); // version
			
			writer.Write( (bool) m_Fixed );

            writer.Write( (int) m_FeatLevel );

			writer.Write( (bool) m_ShowUsesRemaining );

			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 4:
				{
                    m_Fixed = reader.ReadBool();
					goto case 3;
				}
                case 3:
				{
                    m_FeatLevel = reader.ReadInt();
					goto case 2;
				}
				case 2:
				{
					m_ShowUsesRemaining = reader.ReadBool();
					goto case 1;
				}
				case 1:
				{
					m_UsesRemaining = reader.ReadInt();
					goto case 0;
				}
				case 0:
				{
					if ( m_UsesRemaining < 1 )
						m_UsesRemaining = 150;

					break;
				}
			}
		}

		public override void OnHit( Mobile attacker, Mobile defender )
		{
			base.OnHit( attacker, defender );

			if ( !Core.AOS && (attacker.Player || attacker.Body.IsHuman) && Layer == Layer.TwoHanded && (attacker.Skills[SkillName.Anatomy].Value / 400.0) >= Utility.RandomDouble() )
			{
				StatMod mod = defender.GetStatMod( "Concussion" );

				if ( mod == null )
				{
					defender.SendMessage( "You receive a concussion blow!" );
					defender.AddStatMod( new StatMod( StatType.Int, "Concussion", -(defender.RawInt / 2), TimeSpan.FromSeconds( 30.0 ) ) );

					attacker.SendMessage( "You deliver a concussion blow!" );
					attacker.PlaySound( 0x11C );
				}
			}
		}
	}
}
