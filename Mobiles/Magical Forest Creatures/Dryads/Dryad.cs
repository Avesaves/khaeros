using System;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a dryad corpse" )]
	public class Dryad : BaseCreature, IMagicalForestCreature
	{
		[Constructable]
		public Dryad() : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "a Dryad";
			Body = 266;
			BaseSoundID = 0x467;

			SetStr( 141, 150 );
			SetDex( 401, 500 );
			SetInt( 237, 278 );

			SetHits( 113, 218 );

			SetDamage( 11, 15 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 40, 60 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 50.5, 60.0 );
			SetSkill( SkillName.Tactics, 10.1, 20.0 );
			SetSkill( SkillName.UnarmedFighting, 100.1, 120.5 );
			SetSkill( SkillName.Peacemaking, 120.1, 140.5 );
			SetSkill( SkillName.Axemanship, 120.1, 140.5 );

			Fame = 3000;
			Karma = 3000;

			VirtualArmor = 40;
			Female = true;
		}

		public override HideType HideType{ get{ return HideType.Thick; } }
		public override int Hides{ get{ return 5; } }
		public override int Meat{ get{ return 1; } }

		public Dryad( Serial serial ) : base( serial )
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
		}
	}
}
