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
	[CorpseName( "a female minotaur corpse" )]
	
	public class MinotaurCleric : BaseCreature, ILargePredator, IMinotaur, IEnraged
	{
		[Constructable]
		public MinotaurCleric() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a minotaur cleric";
			Body = 262;
			BaseSoundID = 427;

			SetStr( 167, 245 );
			SetDex( 116, 125 );
			SetInt( 35 );

			SetHits( 600 );
			SetMana( 300 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Blunt, 100 );			

			SetResistance( ResistanceType.Blunt, 15, 25 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 30 );
			SetResistance( ResistanceType.Energy, 40, 50 );
			SetResistance( ResistanceType.Poison, 40 );

			SetSkill( SkillName.Macing, 95.1, 100.0 );
			SetSkill( SkillName.MagicResist, 45.1, 70.0 );
			SetSkill( SkillName.Tactics, 95.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 65.1, 70.0 );

			Fame = 14000;
			Karma = -14000;
			
			Female = true;

			VirtualArmor = 30;

			PackItem( new Club() );
		}
		
		public override bool HasFur{ get{ return true; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new MinotaurHorn( 2 ) );
		}
		
		public virtual void HealMinotaur( Mobile m )
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
		                
		            	if( ( m is Minotaur || m is MinotaurBrute || m is MinotaurWarrior || m is MinotaurAbomination || m is MinotaurCleric ) && this.InLOS( m ) )
		                    targets.Add( m );
		            }
		        }
		
		        if( targets.Count > 0 )
		        {
		        	Mobile m = (Mobile)targets[0];
		        	
					if ( m.Hits < m.HitsMax )
					{
						this.RevealingAction();
						HealMinotaur( m );
					}
		        }
		    }

            base.OnThink();
		}

		public override int Meat{ get{ return 14; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 6; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public MinotaurCleric( Serial serial ) : base( serial )
		{
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
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
