using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections;
using Server.Network;

namespace Server.Mobiles
{
[CorpseName( "a Chimera corpse" )]
	public class Chimera : BaseMount, ILargePredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		
		[Constructable]
		public Chimera() : this( "a Chimera" )
		{
		}
		[Constructable]
		public Chimera( string name ) : base( name, 276, 0x3e90, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x4FB;
			Hue = 0;

			SetStr( 196, 225 );
			SetDex( 86, 105 );
			SetInt( 35 );

			SetHits( 320, 330 );

			SetDamage( 14, 18 );

			SetDamageType( ResistanceType.Slashing, 100 );			

			SetResistance( ResistanceType.Slashing, 25, 35 );
			SetResistance( ResistanceType.Blunt, 25, 35 );
			SetResistance( ResistanceType.Piercing, 25, 35 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 65, 75 );

			SetSkill( SkillName.Invocation, 150.4, 200.0 );
			SetSkill( SkillName.Magery, 130.4, 150.0 );
			SetSkill( SkillName.MagicResist, 50.0 );
			SetSkill( SkillName.Tactics, 137.6, 158.0 );
			SetSkill( SkillName.UnarmedFighting, 130.5, 152.5 );

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 40;

			Tamable = false;
			ControlSlots = 99;
			MinTameSkill = 100.0;
			
		}

		public override int GetAngerSound()
		{
			if ( !Controlled )
				return 0x16A;

			return base.GetAngerSound();
		}

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int Meat{ get{ return 16; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Scaled; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override bool BardImmune{ get{ return true; } }				

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public Chimera( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( BaseSoundID == 0x16A )
				BaseSoundID = 0xA8;
		}
	}
}
