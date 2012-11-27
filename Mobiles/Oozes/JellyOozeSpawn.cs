using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a Jelly Ooze Spawn corpse" )]
	public class JellyOozeSpawn : BaseCreature, ISmallPredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public JellyOozeSpawn() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Jelly Ooze Spawn";
			BodyValue = 94;
			BaseSoundID = 456;
			Hue = 0;

			SetStr( 22, 34 );
			SetDex( 16, 21 );
			SetInt( 16, 20 );

			SetHits( 15, 19 );

			SetDamage( 1, 5 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 5, 10 );
			SetResistance( ResistanceType.Piercing, 10, 20 );
			SetResistance( ResistanceType.Slashing, 10, 20 );
			SetResistance( ResistanceType.Poison, 20 );

			SetSkill( SkillName.Poisoning, 30.1, 50.0 );
			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 19.3, 34.0 );
			SetSkill( SkillName.UnarmedFighting, 19.3, 34.0 );

			Fame = 300;
			Karma = -300;

			VirtualArmor = 8;

			ControlSlots = 1;
			MinTameSkill = 23.1;
		}

		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish | FoodType.FruitsAndVegies | FoodType.GrainsAndHay | FoodType.Eggs; } }

		public JellyOozeSpawn( Serial serial ) : base( serial )
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
