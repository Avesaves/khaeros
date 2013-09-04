using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.XmlSpawner2;
using System.Collections;

namespace Server.Mobiles
{
	public class WesternGuard : BaseKhaerosMobile, IRacialGuard, IWestern
	{
        private DateTime m_speechInterval;
        private Nation guardNation;

		[Constructable]
        public WesternGuard() : this( 0 )
        {
        }
        
		[Constructable]
		public WesternGuard( int choice ) : base( Nation.Western ) 
		{
			SetStr( 150 );
			SetDex( 75 );
			SetInt( 75 );

			SetDamage( 10, 15 );
			
			SetHits( 400 );
			
			SetDamageType( ResistanceType.Blunt, 100 );
			
			SetResistance( ResistanceType.Blunt, 10 );
			SetResistance( ResistanceType.Piercing, 10 );
			SetResistance( ResistanceType.Slashing, 10 );

			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Archery, 100.0 );
			SetSkill( SkillName.Fencing, 100.0 );
			SetSkill( SkillName.Macing, 100.0 );
			SetSkill( SkillName.Swords, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Polearms, 100.0 );
			SetSkill( SkillName.ExoticWeaponry, 100.0 );
			SetSkill( SkillName.Axemanship, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 100.0 );
			
			this.Fame = 12000;
			
			this.VirtualArmor = 0;
			
			if( choice > 3 || choice < 1 )
				choice = 0;
			
			BaseKhaerosMobile.RandomGuardEquipment( this, Nation.Western, choice );
            m_speechInterval = DateTime.Now; // Initiatlizing speech delay in the constructor for criminal recognition system.
            ((IKhaerosMobile)this).Feats.SetFeatLevel(FeatList.Alertness, Utility.Random(4)); // Giving guards the potential to detect hidden mobiles.
            guardNation = Nation.Western;
		}

        // Criminal recognition begins here.

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            XmlAttachment attachment = null;
            attachment = XmlAttach.FindAttachment(attacker, typeof(XmlCriminal));

            if (attachment == null)
            {
                XmlAttach.AttachTo(attacker, new XmlCriminal(guardNation));
                this.Say(RandomAttackSpeech());
            }

            base.OnGotMeleeAttack(attacker);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            XmlAttachment attachment = null;
            attachment = XmlAttach.FindAttachment(from, typeof(XmlCriminal));

            if (attachment == null)
            {
                XmlAttach.AttachTo(from, new XmlCriminal(guardNation));
                this.Say(RandomAttackSpeech());
            }

            base.OnDamage(amount, from, willKill);
        }

        public override bool CanSee(Mobile m)
        {
            XmlAttachment attachment = null;
            attachment = XmlAttach.FindAttachment(m, typeof(XmlCriminal));

            if ((m is PlayerMobile) // if the seen mobile is a player
                && (m.Combatant != this) // if the guard isn't in combat
                && (m.InRange(this, 14)) // if the seen mobile is within 14 tiles of the guard
                && (this.InFieldOfVision(m)) // if the seen mobile passes the guard's direction(s)
                && (this.InLOS(m)) // if the seen mobile is within the guard's LOS
                && (m.AccessLevel < AccessLevel.Counselor)) // if the seen mobile is not staff
            {
                if (attachment == null) // if there is no Criminal attachment
                {
                    if (((PlayerMobile)m).CriminalActivity) // Is the seen PlayerMobile participating in criminal activity?
                    {
                        XmlAttach.AttachTo(m, new XmlCriminal(guardNation));
                        m.SendMessage("The Westerns have been alerted of your crimes!");

                        ((PlayerMobile)m).CriminalActivity = false;
                    }
                    else if (((PlayerMobile)m).Combatant is IMhordul)// Is the seen PlayerMobile fighting someone of the guard's race?
                    {
                        if (((PlayerMobile)m).Nation != guardNation) // Is the attacking PlayerMobile not of the guard's race?
                        {
                            XmlAttach.AttachTo(m, new XmlCriminal(guardNation));
                            m.SendMessage("The Mhordul have been alerted of your assault!");
                        }
                    }
                    else if (m.Combatant is PlayerMobile)
                    {
                        PlayerMobile currentCombatant = m.Combatant as PlayerMobile;
                        if ((currentCombatant.Nation == guardNation) && (((PlayerMobile)m).Nation != guardNation))
                        {
                            XmlAttach.AttachTo(m, new XmlCriminal(guardNation));
                            m.SendMessage("The Westerns have been alerted of your assault!");
                        }
                    }
                }

                // The mobile is added to the guard's aggressors list if the mobile has the XmlCriminal attachment, if the guard is not 
                // currently in battle, and if the mobile is not staff.
                if ((attachment is XmlCriminal) && (this.Combatant == null) && (m.AccessLevel < AccessLevel.Counselor))
                {
                    if (((XmlCriminal)attachment).CriminalNation == guardNation)
                    {
                        this.Say(RandomAttackSpeech());
                        m.RevealingAction();
                        this.AddAggressor = m;
                    }
                }
            }

            return base.CanSee(m);
        }

        // A random attacking phrase generator; the timer is set so that guards do not endlessly spout there attack phrases.
        private String RandomAttackSpeech()
        {
            int randomAttackPhrase = Utility.RandomMinMax(1, 8);

            if (DateTime.Now >= (m_speechInterval + TimeSpan.FromSeconds(30)))
            {
                switch (randomAttackPhrase)
                {
                    case 1: { m_speechInterval = DateTime.Now; return "Victory to Xipotec, the Westerns are here!"; }
                    case 2: { m_speechInterval = DateTime.Now; return "A fine sacrifice."; }
                    case 3: { m_speechInterval = DateTime.Now; return "Stop right there, criminal scum!"; }
                    case 4: { m_speechInterval = DateTime.Now; return "Xipotec is great!"; }
                    case 5: { m_speechInterval = DateTime.Now; return "Long live the Ataloa!"; }
                    case 6: { m_speechInterval = DateTime.Now; return "You'll know only darkness."; }
                    case 7: { m_speechInterval = DateTime.Now; return "Ua! Ua! Ua!"; }
                    case 8: { m_speechInterval = DateTime.Now; return "Xipotec! Xipotec! Xipotec!"; }
                }
            }

            return null;
        }
        //End of criminal recognition system.

		public WesternGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
