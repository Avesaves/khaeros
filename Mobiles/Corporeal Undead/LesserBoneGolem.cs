using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Lesser Bone Golem corpse" )]
	public class LesserBoneGolem : BaseCreature, IUndead, IEnraged
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.Dismount;
		}

		[Constructable]
		public LesserBoneGolem() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Lesser Bone Golem";
			Body = 309;
			BaseSoundID = 0x48D;

			SetStr( 96, 120 );
			SetDex( 41, 55 );
			SetInt( 35 );

			SetHits( 58, 72 );

			SetDamage( 4, 9 );

			SetDamageType( ResistanceType.Blunt, 85 );
			SetDamageType( ResistanceType.Cold, 15 );

			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Piercing, 50, 60 );
			SetResistance( ResistanceType.Slashing, 50, 60 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 70.1, 95.0 );
			SetSkill( SkillName.Tactics, 55.1, 80.0 );
			SetSkill( SkillName.UnarmedFighting, 50.1, 70.0 );

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 54;
			
			PackItem( new Bone( 10 ) );
			PackItem( new CopperWire( 5 ) );
		}

		public override bool BleedImmune{ get{ return true; } }

		public LesserBoneGolem( Serial serial ) : base( serial )
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
