using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Poisoning;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a king cobra corpse" )]
	[TypeAlias( "Server.Mobiles.Lavaserpant" )]
	public class KingCobra : BaseCreature, IMediumPredator, IDesertCreature, ISerpent
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public KingCobra() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a king cobra";
			Body = 89;
			BaseSoundID = 219;
			Hue = 1806;

			SetStr( 106, 115 );
			SetDex( 26, 30 );
			SetInt( 16, 25 );

			SetHits( 132, 149 );
			SetMana( 0 );

			SetDamage( 10, 12 );

			SetDamageType( ResistanceType.Piercing, 20 );
			SetDamageType( ResistanceType.Fire, 80 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 70, 80 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 65.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 80.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 20;
		}

		public override bool DeathAdderCharmable{ get{ return true; } }

		public override int Meat{ get{ return 4; } }
		public override int Hides{ get{ return 3; } }
		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 20)
		}; } }
		public override int PoisonDuration { get { return 90; } }
		public override int PoisonActingSpeed { get { return 7; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new KingCobraVenom() );
		}

		public KingCobra(Serial serial) : base(serial)
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
