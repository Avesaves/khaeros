using System;
using Server;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server.Spells;
using Server.Mobiles;
using Server.Network;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a Lantern Archon corpse" )]
	public class LanternArchon : BaseCreature, ICelestial
	{
		public override bool DeleteCorpseOnDeath
		{
			get{ return true; }
		}
		
		[Constructable]
		public LanternArchon() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Lantern Archon";
			Body = 58;
			BaseSoundID = 466;

			SetStr( 196, 225 );
			SetDex( 196, 225 );
			SetInt( 196, 225 );

			SetHits( 118, 135 );

			SetDamage( 7, 8 );

			SetDamageType( ResistanceType.Energy, 100 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 20, 40 );
			SetResistance( ResistanceType.Cold, 10, 30 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.Invocation, 80.0 );
			SetSkill( SkillName.Magery, 40.0 );
			SetSkill( SkillName.MagicResist, 40.0 );
			SetSkill( SkillName.Tactics, 80.0 );
			SetSkill( SkillName.UnarmedFighting, 80.0 );

			Fame = 2000;
			Karma = 0;

			VirtualArmor = 40;
			
			AddItem( new LightSource() );
		}

		public override void GenerateLoot()
		{
		}
		
		public virtual void HealCelestial( Mobile m )
		{
			Direction = GetDirectionTo( m );

				m.PlaySound( 0x1F2 );
				m.FixedEffect( 0x376A, 9, 32 );
				this.Mana -= 30;

				m.Hits += 30;
		}

		public override void OnThink()
		{
			if( this.Mana >= 30 && Utility.RandomMinMax( 1, 10 ) > 8 )
			{
		        ArrayList list = new ArrayList();
		
		        foreach( Mobile m in this.GetMobilesInRange( 6 ) )
		            list.Add( m );
		
		        ArrayList targets = new ArrayList();
		
		        for( int i = 0; i < list.Count; ++i )
		        {
		            Mobile m = (Mobile)list[i];
		
		            if( SpellHelper.ValidIndirectTarget( this, m ) )
		            {
		            	if( m == null || m.Deleted || m.Map != this.Map || !m.Alive || !this.CanSee( m ) || m.Combatant == this )
		                    continue;
		                
		            	if( this.InLOS( m ) && ( m is ICelestial || ( m is PlayerMobile && ( (PlayerMobile)m ).Friendship.Celestial > 0 && ( (PlayerMobile)m ).Friendship.Abyssal == 0 ) ) )
		                    targets.Add( m );
		            }
		        }
		
		        if( targets.Count > 0 )
		        {
		        	Mobile m = (Mobile)targets[0];
		        	
					if ( m.Hits < m.HitsMax )
					{
						this.RevealingAction();
						HealCelestial( m );
					}
		        }
		    }
			
			base.OnThink();
		}

		public LanternArchon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
