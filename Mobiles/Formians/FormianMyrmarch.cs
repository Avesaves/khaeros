using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a formian myrmarch corpse" )] // TODO: Corpse name?
	public class FormianMyrmarch : BaseCreature, IMediumPredator, IEnraged, IFormian
	{
		public override int Height{ get{ return 10; } }
		[Constructable]
		public FormianMyrmarch() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a formian myrmarch";
			Body = 807;
			BaseSoundID = 959;
			Hue = 0;

			SetStr( 326, 350 );
			SetDex( 81, 95 );
			SetInt( 35 );

			SetHits( 151, 162 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Piercing, 70 );
			SetDamageType( ResistanceType.Poison, 30 );

			SetResistance( ResistanceType.Blunt, 30, 40 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 30, 35 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 35, 40 );
			SetResistance( ResistanceType.Energy, 25, 30 );

			SetSkill( SkillName.MagicResist, 30.0 );
			SetSkill( SkillName.Tactics, 90.0 );
			SetSkill( SkillName.UnarmedFighting, 90.0 );

			Fame = 6500;
			Karma = -6500;

			VirtualArmor = 50;
		}

		public override int GetAngerSound()
		{
			return 0x259;
		}

		public override int GetIdleSound()
		{
			return 0x259;
		}

		public override int GetAttackSound()
		{
			return 0x195;
		}

		public override int GetHurtSound()
		{
			return 0x250;
		}

		public override int GetDeathSound()
		{
			return 0x25B;
		}
		
		public override int Meat{ get{ return 10; } }

		public FormianMyrmarch( Serial serial ) : base( serial )
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
