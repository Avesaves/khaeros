using System;
using Server.Mobiles;
using Server.Factions;
using Server.Engines.Poisoning;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a giant copperhead corpse")]
	public class GiantCopperhead : BaseCreature, IMediumPredator, ICaveCreature, ISerpent
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public GiantCopperhead() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 89;
			Name = "a giant copperhead";
			BaseSoundID = 219;
			Hue = 2827;

			SetStr( 141, 160 );
			SetDex( 31, 40 );
			SetInt( 35 );

			SetHits( 137, 146 );

			SetDamage( 12, 15 );

			SetDamageType( ResistanceType.Piercing, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 5, 10 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.Poisoning, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 80.1, 95.0 );
			SetSkill( SkillName.UnarmedFighting, 85.1, 100.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 40;
		}

		public override bool DeathAdderCharmable{ get{ return true; } }

		public override int Meat{ get{ return 4; } }
		public override int Bones{ get{ return 4; } }
		public override int Hides{ get{ return 3; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.ManaDecrease, 150),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 30)
		}; } }
		public override int PoisonDuration { get { return 360; } }
		public override int PoisonActingSpeed { get { return 3; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new CopperheadVenom() );
		}

		public GiantCopperhead(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 1);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

            if( version < 1 )
                BodyValue = 89;

			if ( BaseSoundID == -1 )
				BaseSoundID = 219;
		}
	}
}
