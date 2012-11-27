using System;
using Server;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server.Spells;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Drider cleric corpse" )]
	public class DriderCleric : BaseCreature, IMediumPredator, IDrider
	{
		[Constructable]
		public DriderCleric() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Drider cleric";
			Body = 72;
			BaseSoundID = 599;
			Hue = 2886;

			SetStr( 316, 405 );
			SetDex( 66, 75 );
			SetInt( 50 );

			SetHits( 260, 283 );

			SetDamage( 12, 16 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 80.0 );

			Fame = 12000;
			Karma = -12000;
			
			VirtualArmor = 40;

			PackItem( new SpidersSilk( 10 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 1 );
		}
		
		public override int Meat{ get{ return 8; } }
		
		public virtual void HealDrider( Mobile m )
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
		            	if( m == null || m.Deleted || m.Map != this.Map || !m.Alive || !this.CanSee( m ) || !this.CanBeHarmful( m ) || m.Combatant == this )
		                    continue;
		            	
		            	if( m is BaseCreature && ((BaseCreature)m).ControlMaster != null )
	            			continue;
		                
		            	if( ( m is IDrider || m is ISpider ) && this.InLOS( m ) )
		                    targets.Add( m );
		            }
		        }
		
		        if( targets.Count > 0 )
		        {
		        	Mobile m = (Mobile)targets[0];
		        	
					if ( m.Hits < m.HitsMax )
					{
						this.RevealingAction();
						HealDrider( m );
					}
		        }
		    }
			
			base.OnThink();
		}

		public DriderCleric( Serial serial ) : base( serial )
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
