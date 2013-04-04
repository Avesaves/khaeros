using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an Ogre corpse" )]
	public class Ogre : BaseCreature, ILargePredator, IMhordulFavoredEnemy, IOgre
	{
		public override int Height{ get{ return 20; } }
		[Constructable]
		public Ogre () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an Ogre";
			Body = Utility.RandomList( 53, 54 );
			BaseSoundID = 461;

			SetStr( 586, 785 );
			SetDex( 177, 255 );
			SetInt( 351, 450 );

			SetHits( 352, 471 );

			SetDamage( 13, 19 );

			SetDamageType( ResistanceType.Slashing, 100 );

			SetResistance( ResistanceType.Blunt, 80, 90 );
			SetResistance( ResistanceType.Piercing, 80, 90 );
			SetResistance( ResistanceType.Slashing, 80, 90 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 50.1, 75.0 );
			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 99.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 97.6, 100.0 );

			Fame = 8000;
			Karma = 8000;

			VirtualArmor = 40;
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 10; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 5; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public Ogre( Serial serial ) : base( serial )
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
