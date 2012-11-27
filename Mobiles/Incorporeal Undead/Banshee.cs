using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a banshee corpse" )]
	public class Banshee : BaseCreature, IUndead, IEnraged
	{

		[Constructable]
		public Banshee() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a banshee";
			Body = 310;
			BaseSoundID = 0x482;
			Hue = 12345678;

			SetStr( 126, 150 );
			SetDex( 46, 50 );
			SetInt( 35 );

			SetHits( 176, 190 );

			SetDamage( 10, 14 );

			SetDamageType( ResistanceType.Energy, 100 );

			SetResistance( ResistanceType.Blunt, 50, 60 );
			SetResistance( ResistanceType.Piercing, 50, 60 );
			SetResistance( ResistanceType.Slashing, 50, 60 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 70.1, 95.0 );
			SetSkill( SkillName.Tactics, 45.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 50.1, 70.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 40;
			
			PackItem( new Necroplasm( 7 ) );
		}

		public override bool BleedImmune{ get{ return true; } }

		public Banshee( Serial serial ) : base( serial )
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
