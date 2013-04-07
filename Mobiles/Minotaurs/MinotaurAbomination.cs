using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a Minotaur Abomination corpse" )]
	public class MinotaurAbomination : BaseCreature, ILargePredator, IHasReach, IMinotaur, IEnraged
	{
		[Constructable]
		public MinotaurAbomination() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Minotaur Abomination";
			Body = 249;

			SetStr( 386, 430 );
			SetDex( 51, 65 );
			SetInt( 35 );

			SetHits( 1200 );

			SetDamage( 30, 35 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 65, 85 );
			SetResistance( ResistanceType.Piercing, 50, 70 );
			SetResistance( ResistanceType.Slashing, 50, 70 );
			SetResistance( ResistanceType.Fire, 70, 90 );
			SetResistance( ResistanceType.Cold, 50, 70 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.Macing, 95.1, 100.0 );
			SetSkill( SkillName.MagicResist, 45.1, 70.0 );
			SetSkill( SkillName.Tactics, 95.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 95.1, 100.0 );
			SetSkill( SkillName.Macing, 95.1, 100.0 );
			
			this.RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FrontalAOE;

			Fame = 30000;
			Karma = -30000;
			
			VirtualArmor = 30;
            PackItem( new RewardToken( 3 ) );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new MinotaurHorn( 2 ) );
			
			Ribs ribs = new Ribs( 16 );
			ribs.Hue = 2935;
			ribs.Name = "flesh";
			ribs.RotStage = RotStage.Rotten;
			bpc.DropItem( ribs );
		}
		
		public override bool HasFur{ get{ return true; } }
		
		public void SpawnSkeletons( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newSkeletons = Utility.RandomMinMax( 1, 2 );

			for ( int i = 0; i < newSkeletons; ++i )
			{
				UndeadMinotaur skeleton = new UndeadMinotaur();

				skeleton.Team = this.Team;
				skeleton.FightMode = FightMode.Closest;

				bool validLocation = false;
				Point3D loc = this.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				skeleton.MoveToWorld( loc, map );
				skeleton.Combatant = target;
				skeleton.VanishEmote = "*crumbles into dust*";
				this.PlaySound( 1109 );
			}
			
			this.Emote( "*raises fallen minotaurs to defend him in combat*" );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( 0.2 >= Utility.RandomDouble() )
				SpawnSkeletons( caster );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.2 >= Utility.RandomDouble() && this.CanUseSpecial )
			{
				this.CanUseSpecial = false;
				SpawnSkeletons( attacker );
			}
		}

		public override bool ReacquireOnMovement{ get{ return true; } }
		public override int Hides{ get{ return 5; } }
		public override int Bones{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Scaled; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
		}
		
		public override int GetAttackSound()
		{
			return 1260;
		}

		public override int GetAngerSound()
		{
			return 1262;
		}

		public override int GetDeathSound()
		{
			return 1259; //Other Death sound is 1258... One for Yamadon, one for Serado?
		}

		public override int GetHurtSound()
		{
			return 1263;
		}

		public override int GetIdleSound()
		{
			return 1261;
		}


		public MinotaurAbomination( Serial serial ) : base( serial )
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
