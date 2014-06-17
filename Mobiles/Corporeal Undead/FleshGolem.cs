using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a flesh golem corpse" )]
	public class FleshGolem : BaseCreature, IUndead, IEnraged
	{
		[Constructable]
		public FleshGolem() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a flesh golem";
			Body = 304;
			BaseSoundID = 684;

			SetStr( 176, 200 );
			SetDex( 51, 75 );
			SetInt( 35 );

			SetHits( 146, 150 );

			SetDamage( 8, 12 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 50, 60 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 50.1, 75.0 );
			SetSkill( SkillName.Tactics, 55.1, 80.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 70.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 34;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			Ribs ribs = new Ribs( 5 );
			ribs.Hue = 2935;
			ribs.Name = "flesh";
			ribs.RotStage = RotStage.Rotten;
			
			bpc.DropItem( ribs );
            bpc.DropItem(new UndeadFetus());
		}

		public override bool BleedImmune{ get{ return true; } }
		public override int Bones{ get{ return 8; } }

		public FleshGolem( Serial serial ) : base( serial )
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
