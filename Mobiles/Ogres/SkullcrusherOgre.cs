using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a skullcrusher ogre  corpse" )]
	public class SkullcrusherOgre : BaseCreature, ILargePredator, IMhordulFavoredEnemy, IOgre
	{
		public override int Height{ get{ return 35; } }
		
		public override bool SubdueBeforeTame{ get{ return true; } }
		
		[Constructable]
		public SkullcrusherOgre() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skullcrusher ogre";
			Body = 38;
			BaseSoundID = 427;

			SetStr( 201, 350 );
			SetDex( 66, 75 );
			SetInt( 41, 45 );

			SetHits( 500, 550 );

			SetDamage( 32, 36 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 25.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 100.0 );

			Fame = 15500;
			Karma = -15500;
                        Tamable = false;
			VirtualArmor = 54;
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 14; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 7; } }
		public override HideType HideType{ get{ return HideType.Beast; } }
		
				public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 1 );
			AddLoot( LootPack.Meager, 1 );
		}

		public SkullcrusherOgre( Serial serial ) : base( serial )
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
