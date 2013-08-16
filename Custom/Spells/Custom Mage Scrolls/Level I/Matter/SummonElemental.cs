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
    public class SummonElementalcroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new SummonElementalpell();
            }
            set
            {
            }
        }

        [Constructable]
        public SummonElementalcroll()
            : base()
        {
            Hue = 2615;
            Name = "A Summon Elemental Scroll";
        }

        public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
        {
        }

        public override void OnDoubleClick( Mobile m )
        {
            if( !this.IsChildOf( m.Backpack ) )
                return;

            if( !IsMageCheck( m, true ) )
                return;

            BaseCustomSpell.SpellInitiator( new SummonElementalpell( m, 1 ) );
        }

        public SummonElementalcroll( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    }

    [PropertyObject]
    public class SummonElementalpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new SummonElementalpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return false; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return false; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Summon Elemental"; } }
        public override int ManaCost { get { return 120; } }
        public override int BaseRange { get { return 0; } }

        public SummonElementalpell()
            : this( null, 1 )
        {
        }

        public SummonElementalpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6130;
            Range = 0;
            CustomName = "Summon Elemental";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas( new FeatList[] { 
				FeatList.MatterI
				} );
            }
        }
        public virtual bool ConsumeReagents()
        {

            Container pack = Caster.Backpack;

            if (pack == null)
                return false;

            if (pack.ConsumeTotal(typeof(Diamond), 1))
                return true;
            return false;



        }
        public override void Effect()
        {
		 if( CasterHasEnoughMana )
		 {
                             if (!ConsumeReagents())
                {
                    Caster.Emote("*digs in {0} pocket, but finds no usable gemstones...*", Caster.Female == true ? "her" : "his", Caster.Name);
                    Success = false;
                }
               		 else if ( (Caster.Followers + 3) >= Caster.FollowersMax )
               		 {
               			 Caster.SendMessage( "You need at least four free follower slots to cast this spell." );
               			 Success = false;
               		 }
                             else
                             {
                                Caster.Mana -= TotalCost;
                                PlayerMobile caster = Caster as PlayerMobile;
                                BaseCreature summoned = new Chicken() as BaseCreature;
                                Random random = new Random();
                                int randomN = random.Next(1, 10);
                                 if (randomN == 1)
                                     summoned = new LesserFireElemental();
                                 else if (randomN == 2)
                                     summoned = new LesserWaterElemental();
                                 else if (randomN == 3)
                                     summoned = new WaterElemental();
                                 else if (randomN == 4)
                                     summoned = new LesserEnergyElemental();
                                 else if (randomN == 5)
                                     summoned = new EnergyElemental();
                                 else if (randomN == 6)
                                     summoned = new LesserEarthElemental();
                                 else if (randomN == 7)
                                     summoned = new EarthElemental();
                                 else if (randomN == 8)
                                     summoned = new LesserCrystalElemental();
                                 else if (randomN == 9)
                                     summoned = new CrystalElemental();
                                 else
                                     summoned = new LesserAirElemental();



                                 summoned.ControlSlots = 2;
                                 summoned.HasNoCorpse = true;
                                 Summon(caster, summoned, 30, 534, false);
                                Caster.Emote("*A creature expands from the gem in {0} hand...*", Caster.Female == true ? "her" : "his", Caster.Name);
                                Success = true;
                             }
			}
        }
    }
}
