using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Living Tree's corpse" )]
	public class LivingTree : BaseCreature, ILargePredator
	{
		[Constructable]
		public LivingTree() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Living Tree";
			Body = 47;
			BaseSoundID = 442;

			SetStr( 66, 115 );
			SetDex( 16, 25 );
			SetInt( 60 );

			SetHits( 120, 130 );
			SetStam( 0 );

			SetDamage( 7, 9 );

			SetDamageType( ResistanceType.Blunt, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Invocation, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 25.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 70.1, 80.0 );

			Fame = 10;
			Karma = -7000;

			this.RangeFight = 3;
		}
		
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override bool DisallowAllMoves{ get{ return true; } }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.15 >= Utility.RandomDouble())
                Stun(defender);
        }

        private void Stun(Mobile defender)
        {
            IKhaerosMobile defplayer = defender as IKhaerosMobile;

            if (defplayer.StunnedTimer != null)
                defplayer.StunnedTimer.Stop();

            defplayer.StunnedTimer = new LivingTreeStunTimer(defender, 1);
            defplayer.StunnedTimer.Start();
        }

		public LivingTree( Serial serial ) : base( serial )
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

        public class LivingTreeStunTimer : Timer
        {
            private Mobile target;

            public LivingTreeStunTimer(Mobile target, double duration) : base(TimeSpan.FromSeconds(duration))
            {
                this.target = target;
                this.target.SendMessage(60, "The tree whacks you on the head and you get stunned.");
            }

            protected override void OnTick()
            {
                if (this.target == null)
                    return;

                this.target.SendMessage(60, "Your head clears and you feel able to act again.");
                ((IKhaerosMobile)this.target).StunnedTimer = null;
            }
        }
	}
}
