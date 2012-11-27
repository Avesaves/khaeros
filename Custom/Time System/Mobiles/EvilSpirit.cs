//#define UseNonRedMageAI

using System;
using Server;
using Server.Items;
using Server.TimeSystem;

namespace Server.Mobiles
{
	[CorpseName( "a ghostly corpse" )]
	public class EvilSpirit : BaseCreature
	{
#if(UseNonRedMageAI)
		public EvilSpirit() : base( AIType.AI_NonRedMage, FightMode.NonRed, 10, 1, 0.1, 0.2 )
#else
        public EvilSpirit() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2 )
#endif
		{
			Name = "an evil spirit";
			Body = 26;
			Hue = 0x4001;
			BaseSoundID = 0x482;

			SetStr( 126, 180 );
			SetDex( 116, 125 );
			SetInt( 76, 103 );

			SetHits( 140, 185 );

			SetDamage( 18, 37 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Cold, 70 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 15, 35 );

			SetSkill( SkillName.Invocation, 75.1, 90.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 75.1, 90.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 45.1, 55.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 36;

			PackReg( 10 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}
		
		public override bool BleedImmune{ get{ return true; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

        public EvilSpirit(Serial serial)
            : base(serial)
		{
		}

        public override void Damage(int amount, Mobile from)
        {
            MobileObject mo = Support.GetMobileObject(from);

            if (mo != null && !mo.CanBeAttackedByEvilSpirit) // If can't be attacked, dish damage back to attacker.
            {
                from.Damage(amount);

                amount = 0;
            }

            base.Damage(amount, from);
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
