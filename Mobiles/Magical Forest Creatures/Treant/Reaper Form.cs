using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Treant corpse" )]
	public class Treant : BaseCreature, IMagicalForestCreature, IEnraged
	{
		[Constructable]
		public Treant() : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "a Treant";
			Body = 285;
			BaseSoundID = 442;

			SetStr( 316, 405 );
			SetDex( 246, 265 );
			SetInt( 66, 75 );

			SetHits( 350, 403 );

			SetDamage( 21, 33 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 40, 50 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Invocation, 110.1, 120.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 50.5, 60.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 100.1, 120.0 );

			Fame = 18000;
			Karma = 18000;

			VirtualArmor = 50;

			PackItem( new GnarledStaff() );
		}

		public override bool CanRummageCorpses{ get{ return true; } }

		public Treant( Serial serial ) : base( serial )
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
