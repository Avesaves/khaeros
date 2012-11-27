using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Poisoning;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a scorpion corpse" )]
	public class DesertScorpion : BaseCreature, ILargePredator, IDesertCreature, IGiantBug
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public DesertScorpion() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a scorpion";
			Body = 48;
			BaseSoundID = 397;

			SetStr( 73, 115 );
			SetDex( 16, 25 );
			SetInt( 16, 30 );

			SetHits( 50, 63 );
			SetMana( 0 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Piercing, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Cold, 20, 25 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.Poisoning, 80.1, 100.0 );
			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 60.3, 75.0 );
			SetSkill( SkillName.UnarmedFighting, 50.3, 65.0 );

			Fame = 2000;
			Karma = -2000;

			VirtualArmor = 28;
		}
		
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }
		public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison{ get{ return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StaminaDecrease, 100),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 20)
		}; } }
		public override int PoisonDuration { get { return 360; } }
		public override int PoisonActingSpeed { get { return 2; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new ScorpionVenom() );
		}
		
		public DesertScorpion( Serial serial ) : base( serial )
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
