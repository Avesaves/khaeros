using System;
using Server;
using Server.Items;
using Server.Factions;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a famine spirit corpse" )]
	public class FamineSpirit: BaseCreature, ILargePredator, IAlwaysHungry, IHasReach, IUndead, IEnraged
	{
		[Constructable]
		public FamineSpirit() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a famine spirit" ;
			Body = 256;
			BaseSoundID = 357;

			SetStr( 306, 325 );
			SetDex( 86, 105 );
			SetInt( 35 );

			SetHits( 1278, 1325 );

			SetDamage( 25, 35 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.Invocation, 30.1, 40.0 );
			SetSkill( SkillName.Magery, 30.1, 40.0 );
			SetSkill( SkillName.MagicResist, 99.1, 100.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 92.5 );
			SetSkill( SkillName.Macing, 90.1, 92.5 );

			Fame = 45000;
			Karma = -45000;
			
			this.RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FrontalAOE;

			VirtualArmor = 60;
            PackItem( new RewardToken( 5 ) );
		}
		
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 0; } }
		public override int Bones{ get{ return 30; } }
		public override int Hides{ get{ return 25; } }
		public override HideType HideType{ get{ return HideType.Beast; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			Ribs ribs = new Ribs( 17 );
			ribs.Hue = 2935;
			ribs.Name = "flesh";
			ribs.RotStage = RotStage.Rotten;
			
			Ham ham = new Ham( 14 );
			ham.Hue = 2935;
			ham.Name = "flesh";
			ham.RotStage = RotStage.Rotten;
			
			BaconSlab bacon = new BaconSlab( 5 );
			bacon.Hue = 2935;
			bacon.Name = "flesh";
			bacon.RotStage = RotStage.Rotten;
			
			CookedBird bird = new CookedBird( 9 );
			bird.Hue = 2935;
			bird.Name = "flesh";
			bird.RotStage = RotStage.Rotten;
			
			Spam deer = new Spam( 1 );
			deer.Hue = 2964;
			deer.Name = "a partially digested hind";
			deer.RotStage = RotStage.Rotten;
			deer.ItemID = 15721;
			deer.Stackable = false;
			
			bpc.DropItem( ribs );
			bpc.DropItem( ham );
			bpc.DropItem( bacon );
			bpc.DropItem( bird );
			bpc.DropItem( deer );
            bpc.DropItem(new UndeadFetus());
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
		}

		
		public void SpawnSkeletons( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newSkeletons = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < newSkeletons; ++i )
			{
				Skeleton skeleton = new Skeleton();

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
				skeleton.VanishTime = DateTime.Now + TimeSpan.FromHours( 1 );
				skeleton.VanishEmote = "*crumbles into dust*";
				this.PlaySound( 1109 );
			}
			
			int newZombies = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < newZombies; ++i )
			{
				Zombie zombie = new Zombie();

				zombie.Team = this.Team;
				zombie.FightMode = FightMode.Closest;

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

				zombie.MoveToWorld( loc, map );
				zombie.Combatant = target;
				this.PlaySound( 1109 );
			}
			
			this.Emote( "*freshly created undead burst forth from the sickening chasm of his mouth*" );
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
		
		public FamineSpirit ( Serial serial ) : base( serial )
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
