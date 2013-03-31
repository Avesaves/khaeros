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
	[CorpseName( "a Kobold Cleric corpse" )]
	public class KoboldCleric : BaseCreature, IMediumPredator, IKobold
	{
		[Constructable]
		public KoboldCleric() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Kobold Cleric";
			Body = 255;
			BaseSoundID = 0x452;

			SetStr( 26, 30 );
			SetDex( 31, 45 );
			SetInt( 35 );

			SetHits( 41, 50 );

			SetDamage( 3, 6 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 45, 65 );
			SetResistance( ResistanceType.Poison, 35, 55 );
			SetResistance( ResistanceType.Energy, 25, 50 );

			SetSkill( SkillName.Anatomy, 115.1, 130.0 );
			SetSkill( SkillName.MagicResist, 100.1, 120.0 );
			SetSkill( SkillName.Tactics, 115.1, 130.0 );
			SetSkill( SkillName.UnarmedFighting, 110.1, 130.0 );
			SetSkill( SkillName.Invocation, 92.6, 107.5 );
			SetSkill( SkillName.Magery, 105.1, 115.0 );
			SetSkill( SkillName.Meditation, 100.1, 110.0 );
			SetSkill( SkillName.MagicResist, 15.0 );

			Fame = 1600;
			Karma = -1600;

		}
		
		public virtual void HealKobold( Mobile m )
		{
			Direction = GetDirectionTo( m );

				m.PlaySound( 0x1F2 );
				m.FixedEffect( 0x376A, 9, 32 );
				this.Mana -= 10;

				m.Hits += 10;
		}

		public override void OnThink()
		{
			if( this.Mana >= 10 && Utility.RandomMinMax( 1, 10 ) > 8 )
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
		                
		            	if( ( m is KoboldWorker || m is KoboldWarrior || m is KoboldLord || m is KoboldCleric ) && this.InLOS( m ) )
		                    targets.Add( m );
		            }
		        }
		
		        if( targets.Count > 0 )
		        {
		        	Mobile m = (Mobile)targets[0];
		        	
					if ( m.Hits < m.HitsMax )
					{
						this.RevealingAction();
						HealKobold( m );
					}
		        }
		    }

            base.OnThink();
		}
		
		public override FoodType FavoriteFood{ get{ return FoodType.Fish; } }
		public override int Meat{ get{ return 6; } }
		public override int Bones{ get{ return 6; } }
		public override int Hides{ get{ return 3; } }
		public override HideType HideType{ get{ return HideType.Thick; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		
		public KoboldCleric( Serial serial ) : base( serial )
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
		
		public override int GetIdleSound()
		{
			return 0x42A;
		}

		public override int GetAttackSound()
		{
			return 0x435;
		}

		public override int GetHurtSound()
		{
			return 0x436;
		}

		public override int GetDeathSound()
		{
			return 0x43A;
		}
	}
}
