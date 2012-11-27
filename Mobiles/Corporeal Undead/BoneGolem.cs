using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a bone golem corpse" )]
	public class BoneGolem : BaseCreature, IUndead, IEnraged
	{
		[Constructable]
		public BoneGolem() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a bone golem";
			Body = 308;
			BaseSoundID = 0x48D;

			SetStr( 500 );
			SetDex( 51, 75 );
			SetInt( 35 );

			SetHits( 140 );

			SetDamage( 8, 13 );

			SetDamageType( ResistanceType.Blunt, 50 );

			SetResistance( ResistanceType.Blunt, 75 );
			SetResistance( ResistanceType.Piercing, 80, 85 );
			SetResistance( ResistanceType.Slashing, 80, 85 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 90 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 60 );

			SetSkill( SkillName.DetectHidden, 80.0 );
			SetSkill( SkillName.Invocation, 77.6, 87.5 );
			SetSkill( SkillName.Magery, 77.6, 87.5 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.MagicResist, 50.1, 75.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 100.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 24;
			
			PackItem( new Bone( 20 ) );
			PackItem( new CopperWire( 10 ) );
		}

		public override bool Unprovokable{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }

		public BoneGolem( Serial serial ) : base( serial )
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
