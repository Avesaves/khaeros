using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;
using Server;
using Server.Items;
using Server.Misc;
using Server.Engines.XmlSpawner2;

namespace Khaeros.Scripts.Mobiles.Ghosts
{
    [CorpseName("The Corpse Of The Vengeful Spirit")]
    public class VengefulSpirit : BaseCreature, IUndead, IEnraged
    {
        [Constructable]
		public VengefulSpirit() : base( AIType.AI_Melee, FightMode.Closest, 18, 1, 0.2, 0.4 )
		{
			Name = "The Vengeful Spirit";
			Body = 40;
            Hue = 2932;

			SetStr( 300 );
			SetDex( 100 );
			SetInt( 100 );

			SetHits( 10000 );

			SetDamage( 12, 14 );

			SetDamageType(ResistanceType.Piercing, 100 );

			SetResistance(ResistanceType.Blunt, 100 );
			SetResistance(ResistanceType.Slashing, 100 );
			SetResistance(ResistanceType.Piercing, 100 );
            SetResistance(ResistanceType.Poison, 100 );
            SetResistance(ResistanceType.Physical, 100);

			SetSkill( SkillName.UnarmedFighting, 120);
			SetSkill( SkillName.Tactics, 120);
			SetSkill( SkillName.MagicResist, 0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Invocation, 120.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			

			PackItem( new Necroplasm( 3 ) );

			Fame = 8000;
			Karma = -8000;

            XP = 2000;
         
			VirtualArmor = 30;
		}		

		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (Utility.RandomDouble() < 0.25)
 	        this.Emote("The weapon seems to pass right through the ghostly figure..");
            base.OnGotMeleeAttack(attacker);
        }

        public override void OnDamagedBySpell(Mobile from)
        {
            if (Utility.RandomDouble() < 0.25)
 	         this.Emote("The spell seems to burn the ghastly figure, causing it's face to distort in pain.");
            base.OnDamagedBySpell(from);
        }

        public override void  OnGaveMeleeAttack(Mobile defender)
        {
            if (Utility.RandomDouble() < 0.25)
            {
                Trip(defender);
            }
 	        base.OnGaveMeleeAttack(defender);
        }

        private void Trip(Mobile defender)
        {
            IKhaerosMobile target = defender as IKhaerosMobile;

            if (target.TrippedTimer != null)
                target.TrippedTimer.Stop();

            target.TrippedTimer = new VengefulTripTimer(defender, 3);
            target.TrippedTimer.Start();			
        }

		public VengefulSpirit( Serial serial ) : base( serial )
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
    public class VengefulTripTimer : Timer
    {
        private Mobile m_from;
        public int m_Stage;
        private int m_FeatLevel;
        public bool m_Repeat;
        public VengefulTripTimer(Mobile from, int featlevel)
            : base(TimeSpan.FromSeconds(0.4), TimeSpan.FromSeconds(0.4))
        {
            Priority = TimerPriority.TwoFiftyMS;
            m_from = from;
            m_Stage = 0;
            m_FeatLevel = featlevel;
            from.SendMessage(60, "The anger from the ghost overwhelms you..");
            CombatSystemAttachment csa = CombatSystemAttachment.GetCSA(from);
            if (from.Body.Type == BodyType.Human)
            {
                csa.Animate(21, 7, 1, true, false, 0);
            }
            else if (from.Body.Type == BodyType.Animal)
            {
                csa.Animate(8, 4, 1, true, false, 0);
            }
            else if (from.Body.Type == BodyType.Sea) // this really, eh.. can't fall down
            {
            }
            else if (from.Body.Type == BodyType.Monster)
            {
                csa.Animate(2, 4, 1, true, false, 0);
            }
            this.Interval = TimeSpan.FromSeconds(((double)m_FeatLevel) * 2.0 - 0.4);
        }

        protected override void OnTick()
        {
            if (m_from == null)
                return;

            CombatSystemAttachment csa = CombatSystemAttachment.GetCSA(m_from);

            if (m_Stage == 0)
            {
                CombatSystemAttachment.TripLoopCallback(csa);
                this.Interval = TimeSpan.FromSeconds(0.4);
                this.Delay = TimeSpan.FromSeconds(((double)m_FeatLevel) * 2.0 - 0.4);
            }
            else if (m_Stage == 1)
            {
                csa.StopAnimating(false);
                if (m_from.Body.Type == BodyType.Human)
                {
                    csa.Animate(21, 6, 1, false, false, 0);
                }
                else if (m_from.Body.Type == BodyType.Animal)
                {
                    csa.Animate(8, 4, 1, false, false, 0);
                }
                else if (m_from.Body.Type == BodyType.Sea) // this really, eh.. can't fall down
                {
                }
                else if (m_from.Body.Type == BodyType.Monster)
                {
                    csa.Animate(2, 4, 1, false, false, 0);
                }
                this.Delay = this.Interval = TimeSpan.FromSeconds(0.4);
            }
            else if (m_Stage == 2)
            {
                CombatSystemAttachment.TripGetUpCallback(csa);
                Stop();
                return;
            }

            if (!m_Repeat)
                m_Stage++;
        }
    }
}


