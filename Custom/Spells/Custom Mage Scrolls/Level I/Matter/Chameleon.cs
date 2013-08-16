using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ChameleonScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new ChameleonSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public ChameleonScroll()
            : base()
        {
            Hue = 600;
            Name = "A Chameleon scroll";
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (!this.IsChildOf(m.Backpack))
                return;

            if (!IsMageCheck(m, true))
                return;

            BaseCustomSpell.SpellInitiator(new ChameleonSpell(m, 1));
        }

        public ChameleonScroll(Serial serial)
            : base(serial)
        {
        }

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
    }

    [PropertyObject]
    public class ChameleonSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new ChameleonSpell();
        }

        public override bool CustomScripted { get { return true; } }
        public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof(OrigamiPaper); } }
        public override bool CanTargetSelf { get { return true; } }
        public override bool AffectsMobiles { get { return true; } }
        public override bool AffectsItems { get { return true; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return true; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Chameleon"; } }
        public override int ManaCost { get { return 50; } }
        public override int BaseRange { get { return 2; } }

        public ChameleonSpell()
            : this(null, 1)
        {
        }

        public ChameleonSpell(Mobile caster, int featLevel)
            : base(caster, featLevel)
        {
            IconID = 6120;
            Range = 0;
            CustomName = "Chameleon";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas(new FeatList[] { 
				FeatList.MatterI
				});
            }
        }


        public int OriginalBody;


        public override void Effect()
        {
           
            if (Caster.HueMod != -1)
                {
                    
                    
                    Caster.Mana -= 5;

                    Caster.HueMod = -1;
                    Caster.Emote("*The colour drains out of {0} body*", Caster.Female == true ? "her" : "his");
                    Success = true;
                }
            else if (TargetMobile is Mobile && CasterHasEnoughMana )
                {
                    Mobile target = TargetMobile as Mobile;
                    Caster.Mana -= TotalCost;
                    Caster.HueMod = target.Hue;
                    Caster.Emote("*{0} body shimmers as its colour changes to match the creature*", Caster.Female == true ? "her" : "his", Caster.Name);
                    Success = true;

                }
                else if (TargetItem is Item && CasterHasEnoughMana)
                {
                    Item target = TargetItem as Item;
                    
                    Caster.Mana -= TotalCost;
                    Caster.HueMod = target.Hue;
                    Caster.Emote("*{0} body shimmers as its colour changes to match the item*", Caster.Female == true ? "her" : "his", Caster.Name);
                    Success = true;
                }
                else
                    Caster.SendMessage("Invalid target.");
            }
        }
    }
