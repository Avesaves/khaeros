using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Wandering Spirit corpse" )]
	public class WanderingSpirit : BaseCreature, IUndead, IEnraged
	{
		[Constructable]
		public WanderingSpirit() : base( AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.3, 0.6 )
		{
			Name = "a Wandering Spirit";
			Body = 165;
			BaseSoundID = 466;
			Hue = 12345678;

			SetStr( 16, 40 );
			SetDex( 16, 45 );
			SetInt( 11, 25 );

			SetHits( 10, 24 );

			SetDamage( 4, 6 );

			SetDamageType( ResistanceType.Energy, 100 );

			SetResistance( ResistanceType.Blunt, 15, 20 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 15, 20 );

			SetSkill( SkillName.Invocation, 40.0 );
			SetSkill( SkillName.Magery, 50.0 );
			SetSkill( SkillName.Meditation, 40.0 );
			SetSkill( SkillName.MagicResist, 10.0 );
			SetSkill( SkillName.Tactics, 0.1, 15.0 );
			SetSkill( SkillName.UnarmedFighting, 25.1, 40.0 );

			Fame = 500;
			Karma = 0;

			VirtualArmor = 18;

			PackItem( new Necroplasm( 1 ) );
		}
		

		public WanderingSpirit( Serial serial ) : base( serial )
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
