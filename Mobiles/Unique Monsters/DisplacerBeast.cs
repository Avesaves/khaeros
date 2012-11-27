using System;
using System.Collections;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Displacer Beast corpse" )]
	public class DisplacerBeast : BaseCreature, IMediumPredator, IHasReach
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public DisplacerBeast() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Displacer Beast";
			Body = 246;
			Hue = 2800;

			SetStr( 71, 120 );
			SetDex( 126, 145 );
			SetInt( 35 );

			SetHits( 101, 150 );

			SetDamage( 12, 15 );

			SetDamageType( ResistanceType.Piercing, 70 );
			SetDamageType( ResistanceType.Energy, 30 );

			SetResistance( ResistanceType.Blunt, 40, 60 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Fire, 70, 90 );
			SetResistance( ResistanceType.Cold, 40, 60 );
			SetResistance( ResistanceType.Poison, 40, 60 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.Invocation, 200.0 );
			SetSkill( SkillName.Magery, 112.6, 117.5 );
			SetSkill( SkillName.Meditation, 200.0 );
			SetSkill( SkillName.MagicResist, 40.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 84.1, 88.0 );
			SetSkill( SkillName.Macing, 84.1, 88.0 );
			
			this.RangeFight = 3;

			Fame = 6000;
			Karma = -6000;
		}

		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new DisplacerBeastFur() );
		}
		
		public override int Meat{ get{ return 10; } }
		public override int Bones{ get{ return 10; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		
		public override int GetAngerSound()
		{
			return 0x4DE;
		}

		public override int GetIdleSound()
		{
			return 0x4DD;
		}

		public override int GetAttackSound()
		{
			return 0x4DC;
		}

		public override int GetHurtSound()
		{
			return 0x4DF;
		}

		public override int GetDeathSound()
		{
			return 0x4DB;
		}
		
		public void DisplaceItself( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

				bool validLocation = false;
				Point3D loc = this.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 6 ) - 1;
					int y = Y + Utility.Random( 6 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}
				
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				Effects.SendLocationParticles( EffectItem.Create(   loc, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

				this.PlaySound( 0x1FE );
				
				this.Location = loc;
				this.ProcessDelta();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.5 >= Utility.RandomDouble() )
				DisplaceItself( attacker );
		}

		public DisplacerBeast( Serial serial ) : base( serial )
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
