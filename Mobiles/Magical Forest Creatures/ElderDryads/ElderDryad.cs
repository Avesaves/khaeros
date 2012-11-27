using System;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "an Elder Dryad corpse" )]
	public class ElderDryad : BaseCreature, IMagicalForestCreature
	{
		[Constructable]
		public ElderDryad() : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "an Elder Dryad";
			Body = 258;
			BaseSoundID = 0x467;

			SetStr( 241, 250 );
			SetDex( 501, 600 );
			SetInt( 337, 378 );

			SetHits( 213, 318 );

			SetDamage( 15, 20 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 40, 60 );
			SetResistance( ResistanceType.Slashing, 40, 50 );
			SetResistance( ResistanceType.Piercing, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 70.5, 80.0 );
			SetSkill( SkillName.Tactics, 10.1, 20.0 );
			SetSkill( SkillName.UnarmedFighting, 100.1, 120.5 );
			SetSkill( SkillName.Peacemaking, 120.1, 140.5 );
			SetSkill( SkillName.Axemanship, 120.1, 140.5 );

			Fame = 8000;
			Karma = 8000;

			VirtualArmor = 40;
			Female = true;
		}

		public override HideType HideType{ get{ return HideType.Thick; } }
		public override int Hides{ get{ return 5; } }
		public override int Meat{ get{ return 1; } }

		public ElderDryad( Serial serial ) : base( serial )
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
