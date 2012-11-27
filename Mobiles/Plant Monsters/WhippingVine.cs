using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a whipping vine corpse" )]
	public class WhippingVine : BaseCreature, IMediumPredator, IHasReach
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public WhippingVine() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a whipping vine";
			Body = 8;
			Hue = 0;
			BaseSoundID = 352;
			IsSneaky = true;
			
			RangePerception = 8;

			SetStr( 51, 60 );
			SetDex( 26, 30 );
			SetInt( 16, 20 );

			SetMana( 0 );

			SetHits( 58, 72 );
			SetDamage( 4, 5 );

			SetDamageType( ResistanceType.Blunt, 70 );
			SetDamageType( ResistanceType.Poison, 30 );

			SetResistance( ResistanceType.Blunt, 75, 85 );
			SetResistance( ResistanceType.Piercing, 60, 80 );
			SetResistance( ResistanceType.Slashing, 60, 80 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 70.0 );
			SetSkill( SkillName.Macing, 70.0 );

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 25;
			
			this.RangeFight = 2;
		}

		public override bool BardImmune{ get{ return !Core.AOS; } }

		public WhippingVine( Serial serial ) : base( serial )
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
