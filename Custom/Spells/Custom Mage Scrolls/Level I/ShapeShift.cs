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
    public class ShapeshiftScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new ShapeshiftSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public ShapeshiftScroll()
            : base()
        {
            Hue = 2964;
            Name = "A ShapeShift scroll";
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

            BaseCustomSpell.SpellInitiator(new ShapeshiftSpell(m, 1));
        }

        public ShapeshiftScroll(Serial serial)
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
    public class ShapeshiftSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new ShapeshiftSpell();
        }

        public override bool CustomScripted { get { return true; } }
        public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof(OrigamiPaper); } }
        public override bool CanTargetSelf { get { return true; } }
        public override bool AffectsMobiles { get { return true; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return true; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "ShapeShift"; } }
        public override int ManaCost { get { return 150; } }
        public override int BaseRange { get { return 12; } }

        public ShapeshiftSpell()
            : this(null, 1)
        {
        }

        public ShapeshiftSpell(Mobile caster, int featLevel)
            : base(caster, featLevel)
        {
            IconID = 6180;
            Range = 0;
            CustomName = "ShapeShift";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas(new FeatList[] { 
				FeatList.MatterI, 
				FeatList.MatterII
				});
            }
        }


        public int OriginalBody;


        public override void Effect()
        {
           
            if (Caster.NameMod != null)
                {
                    if (Caster.Female == true)
                        OriginalBody = 401;
                    else
                        OriginalBody = 400;
                    Caster.BodyValue = OriginalBody;
                    Caster.Emote("*reverts to {0} original form with a sickening grinding noise*", Caster.Female == true ? "her" : "his");
                    Caster.Mana -= TotalCost;
                    Caster.NameMod = null;
                    Caster.HueMod = -1;
                    Success = true;
                }
            else if (TargetMobile is Mobile && CasterHasEnoughMana )
                {
                    Mobile target = TargetMobile as Mobile;
                    Caster.Mana -= TotalCost;
                    Caster.BodyValue = target.BodyValue;
                    Caster.NameMod = target.Name;
                    Caster.HueMod = target.Hue;
                    Caster.Emote("*{0} form changes to grotesquely mimic a creature nearby*", Caster.Female == true ? "her" : "his", Caster.Name);
                    Success = true;

                } 
                else
                    Caster.SendMessage("Invalid target.");
            }
        }
    }
