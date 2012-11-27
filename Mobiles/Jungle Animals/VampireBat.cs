using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a vampire bat corpse" )]
	public class VampireBat : BaseCreature, IMediumPredator, IJungleCreature
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public VampireBat() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a vampire bat";
			Body = 317;
			BaseSoundID = 0x270;
			Hue = 2989;

			SetStr( 41, 50 );
			SetDex( 21, 35 );
			SetInt( 26, 30 );

			SetHits( 25, 36 );

			SetDamage( 2, 5 );

			SetDamageType( ResistanceType.Piercing, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 55.1, 80.0 );
			SetSkill( SkillName.UnarmedFighting, 30.1, 55.0 );

			Fame = 700;
			Karma = -700;

			VirtualArmor = 14;
			PackItem( new Guano() );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new BatWing( 2 ) );
		}
		
		public override int Hides{ get{ return 1; } }
		public override int Meat{ get{ return 2; } }
		public override int Bones{ get{ return 2; } }

		public override int GetIdleSound()
		{
			return 0x29B;
		}

		public VampireBat( Serial serial ) : base( serial )
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
