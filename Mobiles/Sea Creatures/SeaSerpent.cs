using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a sea serpents corpse" )]
	[TypeAlias( "Server.Mobiles.Seaserpant" )]
	public class SeaSerpent : BaseCreature, ILargePredator, IEnraged, ISerpent
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public SeaSerpent() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a sea serpent";
			Body = 150;
			BaseSoundID = 447;

			SetStr( 168, 225 );
			SetDex( 28, 35 );
			SetInt( 35 );

			SetHits( 610, 627 );

			SetDamage( 27, 33 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 15, 20 );

			SetSkill( SkillName.MagicResist, 60.1, 75.0 );
			SetSkill( SkillName.Tactics, 60.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 70.0 );

			Fame = 14000;
			Karma = -14000;

			VirtualArmor = 40;
			CanSwim = true;
			CantWalk = true;
		}

        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new Gills());

        }
		public override int Meat{ get{ return 20; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Scaled; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich);
		}

		public SeaSerpent( Serial serial ) : base( serial )
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
