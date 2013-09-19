using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.XmlSpawner2;
using System.Collections;

namespace Server.Mobiles
{
	public class ImperialGuard : BaseKhaerosMobile, IRacialGuard, IImperial
	{
        private DateTime m_speechInterval;
        private Nation guardNation;

		[Constructable]
        public ImperialGuard() : this( 0 )
        {
        }
        
		[Constructable]
		public ImperialGuard( int choice ) : base( Utility.RandomBool() == true ? Nation.Northern : Nation.Tirebladd )
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
			TurnIntoImperialGuard( this, choice );
            m_speechInterval = DateTime.Now; // Initiatlizing speech delay in the constructor for criminal recognition system.
            ((IKhaerosMobile)this).Feats.SetFeatLevel(FeatList.Alertness, Utility.Random(4)); // Giving guards the potential to detect hidden mobiles.
/*             guardNation = Nation.Imperial; */
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
                        m.SendMessage("The Empire have been alerted of your crimes!");

                        ((PlayerMobile)m).CriminalActivity = false;
                    }
                    else if ( ((PlayerMobile)m).Combatant is INorthern || // Is the seen PlayerMobile fighting someone of the Imperial race?
                        ((PlayerMobile)m).Combatant is ITirebladd || 
                        ((PlayerMobile)m).Combatant is IHaluaroc || 
                        ((PlayerMobile)m).Combatant is IImperial )
                    {
                        if ( ((PlayerMobile)m).Nation != guardNation && // Is the attacking PlayerMobile not of the guard's race?
                            ((PlayerMobile)m).Nation != Nation.Northern && 
                            ((PlayerMobile)m).Nation != Nation.Tirebladd && 
                            ((PlayerMobile)m).Nation != Nation.Haluaroc ) 
                        {
                            XmlAttach.AttachTo(m, new XmlCriminal(guardNation));
                            m.SendMessage("The Empire have been alerted of your assault!");
                        }
                    }
                    else if (m.Combatant is PlayerMobile)
                    {
                        PlayerMobile currentCombatant = m.Combatant as PlayerMobile;
                        if ((currentCombatant.Nation == guardNation) && (((PlayerMobile)m).Nation != guardNation))
                        {
                            XmlAttach.AttachTo(m, new XmlCriminal(guardNation));
                            m.SendMessage("The Empire has been alerted of your assault!");
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
                    case 1: { m_speechInterval = DateTime.Now; return "Honorless slave!"; }
                    case 2: { m_speechInterval = DateTime.Now; return "Betrayer!"; }
                    case 3: { m_speechInterval = DateTime.Now; return "Stop right there, criminal scum!"; }
                    case 4: { m_speechInterval = DateTime.Now; return "Boghul! You will regret this!"; }
                    case 5: { m_speechInterval = DateTime.Now; return "Fool!"; }
                    case 6: { m_speechInterval = DateTime.Now; return "Your death will please Xorgoth."; }
                    case 7: { m_speechInterval = DateTime.Now; return "Your bones will make fine armor!"; }
                    case 8: { m_speechInterval = DateTime.Now; return "You're already dead."; }
                }
            }

            return null;
        }
        //End of criminal recognition system.
		
		public static void TurnIntoImperialGuard( Mobile m, int choice )
		{
			if( choice > 3 || choice < 1 )
				choice = Utility.RandomMinMax( 0, 2 );
			else
				choice--;
			
			m.HairItemID = 8251;
			
			Surcoat coat = new Surcoat();
			coat.ItemID = 15476;
			coat.Name = "Unified Northern Army Surcoat";
			coat.Hue = 2751;
			m.EquipItem( coat );
			m.EquipItem( new ElegantCloak(2751) );
			
			switch( choice )
			{
				case 0:
				{
					OrnatePlateLegs vopl = new OrnatePlateLegs();
					vopl.Resource = CraftResource.Bronze;
					vopl.Hue = 1899;
					m.EquipItem( vopl );
					
					OrnatePlateGorget vopo = new OrnatePlateGorget();
					vopo.Resource = CraftResource.Bronze;
					vopo.Hue = 1899;
					m.EquipItem( vopo );
					
					PlateSabatons ps = new PlateSabatons();
					ps.Resource = CraftResource.Bronze;
					ps.Hue = 1899;
					m.EquipItem( ps );
					
					OrnateKiteShield voks = new OrnateKiteShield();
					voks.Resource = CraftResource.Bronze;
					voks.Hue = 1899;
					m.EquipItem( voks );
					
					HorsemanWarhammer hammer = new HorsemanWarhammer();
					hammer.Resource = CraftResource.Iron;
				    m.EquipItem( hammer );
				    
				    HalfPlateChest thpc = new HalfPlateChest();
					thpc.Resource = CraftResource.Bronze;
					thpc.Hue = 1899;
					m.EquipItem( thpc );
					
					HalfPlateArms thpa = new HalfPlateArms();
					thpa.Resource = CraftResource.Bronze;
					thpa.Hue = 1899;
					m.EquipItem( thpa );
					
					HalfPlateGloves thpg = new HalfPlateGloves();
					thpg.Resource = CraftResource.Bronze;
					thpg.Hue = 1899;
					m.EquipItem( thpg );
					
					WingedHelm twh = new WingedHelm();
	            	twh.Resource = CraftResource.Bronze;
	            	twh.Hue = 1899;
					m.EquipItem( twh );
					
					break;
				}
					
				case 1:
				{
					ChainChest cc = new ChainChest();
	            	cc.Resource = CraftResource.Bronze;
	            	cc.Hue = 1899;
					m.EquipItem( cc );
					
					ChainLegs cl = new ChainLegs();
					cl.Resource = CraftResource.Bronze;
					cl.Hue = 1899;
					m.EquipItem( cl );
					
					ChainCoif co = new ChainCoif();
					co.Resource = CraftResource.Bronze;
					co.Hue = 1899;
					m.EquipItem( co );
					
					RingmailArms ra = new RingmailArms();
					ra.Resource = CraftResource.Bronze;
					ra.Hue = 1899;
					m.EquipItem( ra );
					
					RingmailGloves rg = new RingmailGloves();
					rg.Resource = CraftResource.Bronze;
					rg.Hue = 1899;
					m.EquipItem( rg );
					
					DragonKiteShield vmks = new DragonKiteShield();
					vmks.Resource = CraftResource.Bronze;
					vmks.Hue = 1899;
					m.EquipItem( vmks );
					
					LeatherBoots boots = new LeatherBoots();
					boots.Resource = CraftResource.BeastLeather;	
					boots.Hue = 1899;
					m.EquipItem( boots );
					
					OrnateAxe axe = new OrnateAxe();
					axe.Resource = CraftResource.Iron;
					m.EquipItem( axe );
					
					break;
				}
					
				case 2:
				{
					LeatherBoots boots = new LeatherBoots();
					boots.Resource = CraftResource.BeastLeather;
					boots.Hue = 1899;
					m.EquipItem( boots );
					
					WolfMask mask = new WolfMask();
					mask.Hue = 1899;
					m.EquipItem( mask );
					
					StuddedChest sc = new StuddedChest();
					sc.Resource = CraftResource.BeastLeather;
					sc.Hue = 1899;
					m.EquipItem( sc );
					
					StuddedLegs sl = new StuddedLegs();
					sl.Resource = CraftResource.BeastLeather;
					sl.Hue = 1899;
					m.EquipItem( sl );
					
					StuddedArms sa = new StuddedArms();
					sa.Resource = CraftResource.BeastLeather;
					sa.Hue = 1899;
					m.EquipItem( sa );
					
					StuddedGloves sg = new StuddedGloves();
					sg.Resource = CraftResource.BeastLeather;
					sg.Hue = 1899;
					m.EquipItem( sg );
					
					StuddedGorget so = new StuddedGorget();
					so.Resource = CraftResource.BeastLeather;
					so.Hue = 1899;
					m.EquipItem( so );
					
					LongBow bow = new LongBow();
					bow.Resource = CraftResource.Redwood;
					bow.Hue = 0;
					
					m.EquipItem( bow );
					
					if( m is BaseCreature )
		            {
			            BaseCreature bc = m as BaseCreature;
			            bc.AI = AIType.AI_Archer;
			            bc.PackItem( new Arrow( Utility.RandomMinMax( 10, 20 ) ) );
					}
					break;
				}
			}
		}

		public ImperialGuard(Serial serial) : base(serial)
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
