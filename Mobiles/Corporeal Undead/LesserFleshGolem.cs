using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a Lesser Flesh Golem corpse" )]
	public class LesserFleshGolem : BaseCreature, IUndead, IEnraged
	{
		[Constructable]
		public LesserFleshGolem() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Lesser Flesh Golem";
			Body = 305;
			BaseSoundID = 224;

			SetStr( 161, 185 );
			SetDex( 41, 65 );
			SetInt( 35 );

			SetHits( 97, 111 );

			SetDamage( 7, 11 );

			SetDamageType( ResistanceType.Piercing, 85 );
			SetDamageType( ResistanceType.Poison, 15 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 40.1, 55.0 );
			SetSkill( SkillName.Tactics, 45.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 50.1, 70.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 24;
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			Ribs ribs = new Ribs( 3 );
			ribs.Hue = 2935;
			ribs.Name = "flesh";
			ribs.RotStage = RotStage.Rotten;
			bpc.DropItem( ribs );
		}
		
		public override int GetDeathSound()
		{
			return 1218;
		}

		public override bool BleedImmune{ get{ return true; } }
		public override int Bones{ get{ return 6; } }

		public LesserFleshGolem( Serial serial ) : base( serial )
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
