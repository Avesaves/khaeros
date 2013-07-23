using Server;
using Server.Mobiles;

namespace Khaeros.Scripts.Khaeros.Mobiles.Soldiers
{
    public class Guard : Soldier
    {
        [Constructable]
        public Guard()
        {}

        public Guard(Serial serial) : base(serial){}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            BaseCreature creature = this.AIObject.m_Mobile;

            if ((creature.Controlled) || (e.Mobile is PlayerMobile && Soldier.IsGovernmentSuperior(creature, (PlayerMobile)e.Mobile)))
            {
                creature.DebugSay("Listening...");
                AIObject.m_Speaker = e.Mobile;

                bool isOwner = false;
                bool isFriend = false;

                isOwner = (e.Mobile is PlayerMobile && Soldier.IsGovernmentSuperior(creature, (PlayerMobile)e.Mobile));

                if (e.Mobile.Alive && (isOwner || isFriend))
                {

                    int[] keywords = e.Keywords;
                    string speech = e.Speech;

                    if (isOwner && !(creature is Soldier) && OrderInfo.WasOrdered(e, creature))
                        e.Mobile.SendGump(new ViewOrdersGump(e.Mobile as PlayerMobile, creature));

                    if (GroupInfo.TryGetGroupGump(creature, e))
                        e.Mobile.SendGump(new GroupGump(e.Mobile as PlayerMobile, (e.Mobile as PlayerMobile).Group));

                    // First, check the all*
                    for (int i = 0; i < keywords.Length; ++i)
                    {
                        int keyword = keywords[i];

                        switch (keyword)
                        {
                            case 0x016A: // all report
                                {
                                    return;
                                }

                            case 0x164: // all come
                                {
                                    return;
                                }
                            case 0x165: // all follow
                                {
                                    return;
                                }
                            case 0x166: // all guard
                                {
                                    return;
                                }
                            case 0x167: // all stop
                                {
                                    creature.ControlTarget = null;
                                    creature.ControlOrder = OrderType.Stop;
                                    creature.Aggressors.Clear();
                                    creature.Combatant = null;
                                    creature.Warmode = false;
                                    creature.FocusMob = null;

                                    return;
                                }
                            case 0x168: // all kill
                            case 0x169: // all attack
                                {
                                    return;
                                }
                            case 0x16B: // all guard me
                                {
                                    return;
                                }
                            case 0x16C: // all follow me
                                {
                                    return;
                                }
                            case 0x170: // all stay
                                {
                                    creature.ControlTarget = null;
                                    creature.ControlOrder = OrderType.Stay;

                                    return;
                                }
                        }
                    }

                    // Next check for group commands

                    if (e.Mobile is PlayerMobile && GroupInfo.IsGroupMember(creature, e.Mobile as PlayerMobile))
                    {
                        string groupSpeech = speech.ToLower().Replace(".", "").Replace(",", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("-", "");
                        if (!groupSpeech.Contains("your") && (groupSpeech.Contains("group") || groupSpeech.Contains("squad") || groupSpeech.Contains((e.Mobile as PlayerMobile).Group.Name.ToLower().Replace(".", "").Replace(",", "").Replace("!", "").Replace(";", "").Replace(":", "").Replace("-", ""))))
                        {
                            if (groupSpeech.Contains("come"))
                            {
                                return;
                            }
                            else if (groupSpeech.Contains("follow") && !groupSpeech.Contains("me"))
                            {
                                return;
                            }
                            else if (groupSpeech.Contains("guard"))
                            {
                                return;
                            }
                            else if (groupSpeech.Contains("kill") || groupSpeech.Contains("attack") || groupSpeech.Contains("strike"))
                            {
                                return;
                            }
                            else if (groupSpeech.Contains("stop"))
                            {
                                creature.ControlTarget = null;
                                creature.ControlOrder = OrderType.Stop;
                                creature.Aggressors.Clear();
                                creature.Combatant = null;
                                creature.Warmode = false;

                                return;
                            }
                            else if (groupSpeech.Contains("follow") && groupSpeech.Contains("me"))
                            {
                                return;
                            }
                            else if (groupSpeech.Contains("stay"))
                            {
                                creature.ControlTarget = null;
                                creature.ControlOrder = OrderType.Stay;
                                return;
                            }
                        }
                    }

                    // No all*, no squad, so check *command
                    for (int i = 0; i < keywords.Length; ++i)
                    {
                        int keyword = keywords[i];

                        switch (keyword)
                        {
                            case 0x0160: // report
                                {
                                    return;
                                }

                            case 0x155: // *come
                                {
                                    return;
                                }
                            case 0x156: // *drop
                                {
                                    return;
                                }
                            case 0x15A: // *follow
                                {
                                    return;
                                }
                            case 0x15B: // *friend
                                {
                                    return;
                                }
                            case 0x15C: // *guard
                                {
                                    return;
                                }
                            case 0x15D: // *kill
                            case 0x15E: // *attack
                                {
                                    return;
                                }
                            case 0x15F: // *patrol
                                {
                                    return;
                                }
                            case 0x161: // *stop
                                {
                                    if (AIObject.WasNamed(speech))
                                    {
                                        creature.ControlTarget = null;
                                        creature.ControlOrder = OrderType.Stop;
                                        creature.Aggressors.Clear();
                                        creature.Combatant = null;
                                        creature.Warmode = false;
                                    }

                                    return;
                                }
                            case 0x163: // *follow me
                                {
                                    return;
                                }
                            case 0x16D: // *release
                                {
                                    return;
                                }
                            case 0x16E: // *transfer
                                {
                                    return;
                                }
                            case 0x16F: // *stay
                                {
                                    if (AIObject.WasNamed(speech))
                                    {
                                        creature.ControlTarget = null;
                                        creature.ControlOrder = OrderType.Stay;
                                    }
                                    return;
                                }
                        }
                    }
                }
                base.OnSpeech(e);
            }
            else
            {
                if (e.Mobile.AccessLevel >= AccessLevel.GameMaster || ((creature.CreatureGroup == CreatureGroup.Undead || creature is IUndead) &&
                    e.Mobile is PlayerMobile && ((PlayerMobile)e.Mobile).Feats.GetFeatLevel(FeatList.ControlUndead) > 0))
                {
                    if (e.Mobile is PlayerMobile && e.Mobile.AccessLevel < AccessLevel.GameMaster)
                    {
                        int featlevel = ((PlayerMobile)e.Mobile).Feats.GetFeatLevel(FeatList.ControlUndead);

                        if (featlevel < 3)
                        {
                            if (featlevel < 2 && creature.Fame > 5000)
                                return;
                            else if (creature.Fame > 15000)
                                return;
                        }
                    }

                    creature.DebugSay("It's from a GM");

                    if (creature.FindMyName(e.Speech, true))
                    {
                        string[] str = e.Speech.Split(' ');
                        int i;

                        for (i = 0; i < str.Length; i++)
                        {
                            string word = str[i];

                            if (Insensitive.Equals(word, "obey"))
                            {
                                creature.SetControlMaster(e.Mobile);

                                if (creature.Summoned)
                                    creature.SummonMaster = e.Mobile;

                                return;
                            }
                        }
                    }
                }
                base.OnSpeech(e);
            }
        }
    }
}
