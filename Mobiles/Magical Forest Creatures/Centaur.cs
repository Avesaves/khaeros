using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a centaur corpse" )]
	public class Centaur : BaseCreature, IMagicalForestCreature, IEnraged
	{
		[Constructable]
		public Centaur() : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "a centaur";
			Body = 101;
			BaseSoundID = 679;

			SetStr( 202, 210 );
			SetDex( 44, 60 );
			SetInt( 35 );

			SetHits( 130, 172 );

			SetDamage( 12, 14 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 45, 55 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 45, 55 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.Anatomy, 95.1, 115.0 );
			SetSkill( SkillName.Archery, 95.1, 100.0 );
			SetSkill( SkillName.MagicResist, 50.3, 80.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 95.1, 100.0 );

			Fame = 4500;
			Karma = 4500;

			VirtualArmor = 40;
			AddItem( new Bow() );
			PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) ); // OSI it is different: in a sub backpack, this is probably just a limitation of their engine
		}

		public override int Meat{ get{ return 8; } }
		public override int Bones{ get{ return 8; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public Centaur( Serial serial ) : base( serial )
		{
		}
		
		public override void OnKilledBy( Mobile mob )
		{
			if( mob is PlayerMobile )
				( (PlayerMobile)mob ).LastOffenseToNature = DateTime.Now;
			
			base.OnKilledBy( mob );
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

			if ( BaseSoundID == 678 )
				BaseSoundID = 679;
		}
	}
}
