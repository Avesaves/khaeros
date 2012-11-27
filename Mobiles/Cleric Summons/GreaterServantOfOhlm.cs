using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Greater Servant Of Ohlm corpse" )]
	public class GreaterServantOfOhlm : BaseCreature, ISerpent, IClericSummon
	{
		[Constructable]
		public GreaterServantOfOhlm() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "a Greater Servant Of Ohlm";
			BodyValue = 265;
			BaseSoundID = 904;

            SetStr(146, 170);
            SetDex(60, 70);
            SetInt(70);

            SetHits(328, 332);

            SetDamage(18, 20);

            SetDamageType(ResistanceType.Slashing, 100);

            SetResistance(ResistanceType.Blunt, 35, 45);
            SetResistance(ResistanceType.Piercing, 40, 60);
            SetResistance(ResistanceType.Slashing, 40, 60);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 15.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.UnarmedFighting, 99.0, 100.0);

            Fame = 10;
            Karma = -4000;

            VirtualArmor = 60;

            CanSwim = true;
            CantWalk = false;
		}
		
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public override void GenerateLoot()
		{
		}

		public GreaterServantOfOhlm( Serial serial ) : base( serial )
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
