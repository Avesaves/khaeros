using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Lesser Homunculus corpse" )]
	public class LesserHomunculus : BaseCreature, ISmallPredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public LesserHomunculus() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Lesser Homunculus";
			Body = 74;
			BaseSoundID = 422;

			SetStr( 11, 25 );
			SetDex( 61, 80 );
			SetInt( 86, 105 );

			SetHits( 25, 30 );

			SetDamage( 2, 4 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Invocation, 20.1, 30.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 30.1, 50.0 );
			SetSkill( SkillName.Tactics, 42.1, 50.0 );
			SetSkill( SkillName.UnarmedFighting, 40.1, 44.0 );

			Fame = 500;
			Karma = -500;

			VirtualArmor = 10;
		}

		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Daemon; } }

		public LesserHomunculus( Serial serial ) : base( serial )
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
