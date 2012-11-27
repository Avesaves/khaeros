using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Goatman Warrior corpse" )]
	public class GoatmanWarrior : BaseCreature, IMediumPredator, IEnraged, IGoatman
	{
		[Constructable]
		public GoatmanWarrior() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Goatman Warrior";
			Body = 44;
			BaseSoundID = 461;

			SetStr( 67, 85 );
			SetDex( 66, 75 );
			SetInt( 26, 30 );

			SetHits( 270, 286 );

			SetDamage( 13, 16 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Piercing, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 30 );
			SetResistance( ResistanceType.Fire, 35, 40 );

			SetSkill( SkillName.MagicResist, 40.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );
			SetSkill( SkillName.Macing, 80.1, 100.0 );

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 40;
			PackItem( new Mace() );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new GoatmanFur() );
		}
		
		public override int Meat{ get{ return 4; } }
		public override int Bones{ get{ return 4; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public GoatmanWarrior( Serial serial ) : base( serial )
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
