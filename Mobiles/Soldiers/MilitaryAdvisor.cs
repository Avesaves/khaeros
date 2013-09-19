using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.XmlSpawner2;
using System.Collections;
using System.Collections.Generic;
using Server.Targeting;
using Server.Targets;
using Server.Gumps;
using Server.Prompts;

namespace Server.Mobiles
{
    public class MilitaryAdvisor : BaseKhaerosMobile
    {
        [Constructable]
        public MilitaryAdvisor(GovernmentEntity g)
            : base(g.Nation)
        {
            Blessed = true;
            Government = g;
            Nation = g.Nation;
            RandomRichClothes(this, this.Nation);

            string advisorTitle = "";

            switch (Nation)
            {
                case Nation.Southern: advisorTitle = "Tribune"; Soldier.EquipSouthern((Armament)1, this); break;
                case Nation.Western: advisorTitle = "Keeper"; Soldier.EquipWestern((Armament)1, this); break;
                case Nation.Haluaroc: advisorTitle = "Havildar"; Soldier.EquipHaluaroc((Armament)1, this); break;
                case Nation.Mhordul: advisorTitle = "Bambaici"; Soldier.EquipMhordul((Armament)1, this); break;
                case Nation.Tirebladd: advisorTitle = "Drengr"; Soldier.EquipTirebladd((Armament)1, this); break;
                case Nation.Northern: if (Female) { advisorTitle = "Sister"; } else { advisorTitle = "Brother"; } Soldier.EquipNorthern((Armament)1, this); break;
/*                 case Nation.Imperial: advisorTitle = "Tribune"; Soldier.EquipImperial((Armament)1, this); break;
                case Nation.Sovereign: if (Female) { advisorTitle = "Warmistress"; } else { advisorTitle = "Warmaster"; } Soldier.EquipSovereign((Armament)1, this); break;
                 case Nation.Society: advisorTitle = "Advisor"; Soldier.EquipSociety((Armament)1, this); break;
				case Nation.Insularii: advisorTitle = "Maestor"; Soldier.EquipInsularii((Armament)1, this); break; */
            }

            Name = advisorTitle + " " + Name + " of " + Government.Name.ToString();
        }
    
        #region Speech and Dialogue
        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(this.Location, 14))
                return true;

            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (Government == null || Government.Deleted)
                return;

            if (!e.Handled && e.Mobile.InRange(this.Location, 3))
            {
                if (e.Mobile is PlayerMobile && CustomGuildStone.IsGuildOfficer(e.Mobile as PlayerMobile, Government))
                {
                    if (e.Speech.ToLower().Contains("report") || e.Speech.ToLower().Contains("report"))
                    {
                        e.Mobile.CloseGump(typeof(GovernmentReportsGump));
                        e.Mobile.SendGump(new GovernmentReportsGump(e.Mobile as PlayerMobile, Government, 0));

                        switch (Utility.Random(10))
                        {
                            case 0: Say("At your command."); break;
                            case 1: Say("As you wish."); break;
                            case 2: Say("Here are the reports you asked for."); break;
                            case 3: Say("The day's reports are in."); break;
                            case 4: Say("I trust all is well?"); break;
                            case 5:
                                {
                                    switch (Nation)
                                    {
                                        case Nation.Southern:
                                            Say("I have compiled my spies' reports."); break;
                                        case Nation.Western:
                                            Say("All to preserve the Balance."); break;
                                        case Nation.Haluaroc:
                                            Say("In the light!"); break;
                                        case Nation.Mhordul:
                                            Say("Blood and thunder!"); break;
                                        case Nation.Tirebladd:
                                            Say("Until the end."); break;
                                        case Nation.Northern:
                                            if(e.Mobile.Female)
                                                Say("At once, Sister.");
                                            else
                                                Say("At once, Brother.");
                                            break;
/*                                         case Nation.Imperial:
                                            Say("The Emperor's will be done."); break;
                                        case Nation.Sovereign:
                                            Say("Our enemies will fear us."); break;
                                         case Nation.Society:
                                             if(e.Mobile.Female)
                                                Say("At once, my Lady.");
                                            else
                                                Say("At once, my Lord.");				
											break; 
										case Nation.Insularii:
                                             if(e.Mobile.Female)
                                                Say("At once, my Lady.");
                                            else
                                                Say("At once, my Lord.");				
											break; */
                                        default: break;
                                    }
                                    break;
                                }
                        }
                    }
                    else if (e.Speech.ToLower().Contains("policy") || e.Speech.ToLower().Contains("policies"))
                    {
                        if (!CustomGuildStone.IsGuildMilitary(e.Mobile as PlayerMobile, Government))
                        {
                            this.Say("Only military members of " + this.Government.Name.ToString() + " may set military policy.");
                            return;
                        }

                        e.Mobile.SendGump(new MilitaryPolicyGump(e.Mobile as PlayerMobile, Government));
                    }
                    else if (e.Speech.ToLower().Contains("direction") || e.Speech.ToLower().Contains("face"))
                    {
                        if (!CustomGuildStone.IsGuildLeader(e.Mobile as PlayerMobile, Government))
                            this.Say("Only the leader of " + Government.Name.ToString() + " may give me orders.");
                        else
                        {
                            if (e.Speech.ToLower().Contains("up") || e.Speech.ToLower().Contains("northwest"))
                                this.Direction = Direction.Up;
                            else if (e.Speech.ToLower().Contains("right") || e.Speech.ToLower().Contains("northeast"))
                                this.Direction = Direction.Right;
                            else if (e.Speech.ToLower().Contains("down") || e.Speech.ToLower().Contains("southeast"))
                                this.Direction = Direction.Down;
                            else if (e.Speech.ToLower().Contains("left") || e.Speech.ToLower().Contains("southwest"))
                                this.Direction = Direction.Left;
                            else if (e.Speech.ToLower().Contains("north"))
                                this.Direction = Direction.North;
                            else if (e.Speech.ToLower().Contains("east"))
                                this.Direction = Direction.East;
                            else if (e.Speech.ToLower().Contains("south"))
                                this.Direction = Direction.South;
                            else if (e.Speech.ToLower().Contains("west"))
                                this.Direction = Direction.West;

                            else
                                e.Mobile.SendMessage("Say north, northeast, east, southeast, south, southwest, west, or northwest to have " +
                                    "the military advisor face a direction.");

                            if (e.Mobile.Female)
                                Say("Yes ma'am.");
                            else
                                Say("Yes sir.");
                        }
                    }
                }
            }
        }
        #endregion

        public MilitaryAdvisor(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class AddMilitaryAdvisorTarget : Target
    {
        GovernmentEntity m_Government;

        public AddMilitaryAdvisorTarget(GovernmentEntity g) : base(4, true, TargetFlags.None)
        {
            m_Government = g;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Government != null && !m_Government.Deleted && m_Government.MilitaryAdvisor != null && !m_Government.MilitaryAdvisor.Deleted)
            {
                from.SendMessage(m_Government.Name.ToString() + " already has a military advisor stationed.");
                return;
            }

            if (CustomGuildStone.IsGuildLeader(from as PlayerMobile, m_Government))
            {
                MilitaryAdvisor newAdvisor = new MilitaryAdvisor(m_Government);

                if (targeted is Item)
                    newAdvisor.MoveToWorld((targeted as Item).Location, (targeted as Item).Map);
                else if (targeted is Mobile)
                    newAdvisor.MoveToWorld((targeted as Mobile).Location, (targeted as Mobile).Map);
                else if (targeted is LandTarget)
                    newAdvisor.MoveToWorld((targeted as LandTarget).Location, from.Map);
                else
                    newAdvisor.MoveToWorld(from.Location, from.Map);

                m_Government.MilitaryAdvisor = newAdvisor;
                newAdvisor.Home = newAdvisor.Location;
                newAdvisor.RangeHome = 0;
            }

            base.OnTarget(from, targeted);
        }
    }

    public class RemoveMilitaryAdvisorTarget : Target
    {        
        private GovernmentEntity m_Government;

        public RemoveMilitaryAdvisorTarget(GovernmentEntity gov)
            : base(4, false, TargetFlags.None)
        {
            m_Government = gov;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {

            if (m_Government.MilitaryAdvisor == null || m_Government.MilitaryAdvisor.Deleted)
            {
                from.SendMessage("You do not have a military advisor deployed by " + m_Government.Name.ToString() + ".");
                return;
            }

            if (!(targeted is MilitaryAdvisor))
            {
                from.SendMessage("That is not a military advisor.");
                return;
            }

            if ((targeted as MilitaryAdvisor).Government != m_Government)
            {
                from.SendMessage("You may only remove military advisors belonging to " + m_Government.Name.ToString() + ".");
                return;
            }

            (targeted as MilitaryAdvisor).Delete();
            m_Government.MilitaryAdvisor = null;

            base.OnTarget(from, targeted);
        }
    }
}
