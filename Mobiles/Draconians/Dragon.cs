using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dragon corpse" )]
	public class Dragon : BaseCreature, ILargePredator, IHasReach, IEnraged, IDraconic, IHuge
	{
		public override int Height{ get{ return 25; } }
		public Dragon () : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dragon";
			BodyValue = 61;
			BaseSoundID = 362;

			SetStr( 350, 400 );
			SetDex( 150, 200 );
			SetInt( 100, 150 );

			SetHits( 900, 1100 );

			SetDamage( 40, 45 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 60, 65 );
			SetResistance( ResistanceType.Piercing, 60, 60 );
			SetResistance( ResistanceType.Slashing, 60, 60 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 65, 70 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.Invocation, 30.1, 40.0 );
			SetSkill( SkillName.Magery, 30.1, 40.0 );
			SetSkill( SkillName.MagicResist, 99.1, 100.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 90.1, 92.5 );
			SetSkill( SkillName.Macing, 90.1, 92.5 );
			
			this.RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FrontalAOE;

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 50;
			PackItem( new SulfurousAsh( 12 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
			AddLoot( LootPack.Gems, 5 );
		}
		
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override bool AutoDispel{ get{ return !Controlled; } }
		public override int Meat{ get{ return 40; } }
		public override int Bones{ get{ return 30; } }
		public override int Hides{ get{ return 25; } }
		public override HideType HideType{ get{ return HideType.Scaled; } }
		//public override int Scales{ get{ return 7; } }
		//public override ScaleType ScaleType{ get{ return ( Body == 12 ? ScaleType.Yellow : ScaleType.Red ); } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override bool CanAngerOnTame { get { return true; } }

		public Dragon( Serial serial ) : base( serial )
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
