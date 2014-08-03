using System;
using Server.Items;
using Server.Engines.Poisoning;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an astral plant corpse" )]
	public class Quest2 : BaseCreature, IMediumPredator
	{
		//public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public Quest2() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "an astral plant";
			Body = 789;
			BaseSoundID = 352;
            Hue = 2832; 
			SetStr( 101, 130 );
			SetDex( 26, 35 );
			SetInt( 35 );

			SetHits( 200, 400 );

			SetDamage( 15, 25 );

			SetDamageType( ResistanceType.Piercing, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Blunt, 50, 60 );
			SetResistance( ResistanceType.Piercing, 30, 50 );
			SetResistance( ResistanceType.Slashing, 30, 50 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 50.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 80.0 );

			Fame = 1700;
			Karma = -1700;
            PackItem(new Quest2Item());
			VirtualArmor = 39;
            int rand = Utility.Random(500);
            if (rand > 499)
                PackItem(new StarmetalOre(1));  
		}

		public override int GetAngerSound()
		{
			return 353;
		}

		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageStamina, 20)
		}; } }
		public override int PoisonDuration { get { return 90; } }
		public override int PoisonActingSpeed { get { return 7; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			//bpc.DropItem( new Quest2Venom() );
		}
		
		public override double HitPoisonChance{ get{ return 0.1; } }

		public Quest2( Serial serial ) : base( serial )
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

			if ( BaseSoundID == -1 )
				BaseSoundID = 352;
		}
	}
}
