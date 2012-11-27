using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a bat corpse" )]
	public class Bat : BaseCreature, ISmallPrey
	{
		public override bool ParryDisabled{ get{ return true; } }
		
		[Constructable]
		public Bat() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a bat";
			Body = 317;
			BaseSoundID = 0x270;
			Hue = 1809;

			SetStr( 21, 30 );
			SetDex( 21, 35 );
			SetInt( 26, 50 );

			SetHits( 15, 26 );

			SetDamage( 2, 3 );

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

			Fame = 300;
			Karma = -300;

			VirtualArmor = 14;
			PackItem( new Guano() );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new BatWing( 2 ) );
		}

		public override int Meat{ get{ return 2; } }
		public override int Bones{ get{ return 2; } }
		public override int Hides{ get{ return 1; } }

		public override int GetIdleSound()
		{
			return 0x29B;
		}

		public Bat( Serial serial ) : base( serial )
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
