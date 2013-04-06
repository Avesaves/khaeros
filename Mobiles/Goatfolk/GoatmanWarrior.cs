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

			SetStr( 66, 75 );
			SetDex( 126, 135 );
			SetInt( 35 );

			SetHits( 52, 59 );
			SetMana( 0 );

			SetDamage( 4, 6 );

			SetDamageType( ResistanceType.Piercing, 100 );

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.Poisoning, 40.1, 60.0 );
			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 30.1, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 40.1, 50.0 );

			Fame = 1600;
			Karma = -1600;

			VirtualArmor = 14;
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
			AddLoot( LootPack.Meager);
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
