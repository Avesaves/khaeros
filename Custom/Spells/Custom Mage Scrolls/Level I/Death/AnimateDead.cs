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
    public class AnimateCorpseScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new AnimateCorpseSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public AnimateCorpseScroll()
            : base()
        {
            Hue = 2687;
            Name = "An Animate Corpse scroll";
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

            BaseCustomSpell.SpellInitiator( new AnimateCorpseSpell( m, 1 ) );
        }

        public AnimateCorpseScroll( Serial serial )
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
    public class AnimateCorpseSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new AnimateCorpseSpell();
        }

        public override bool CustomScripted { get { return true; } }
		public override bool IsMageSpell { get { return true; } }
        public override Type ScrollType { get { return typeof( OrigamiPaper ); } }
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return false; } }
        public override bool AffectsItems { get { return true; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return true; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Animate Corpse"; } }
        public override int ManaCost { get { return 50; } }
        public override int BaseRange { get { return 12; } }

        public AnimateCorpseSpell()
            : this( null, 1 )
        {
        }

        public AnimateCorpseSpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6166;
            Range = 12;
            CustomName = "Animate Corpse";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas( new FeatList[] { FeatList.DeathI, FeatList.DeathII } );
            }
        }

        public override void Effect()
        {
            if( TargetCanBeAffected && TargetItem is Corpse && CasterHasEnoughMana )
            {
                BaseCreature creature = ( (Corpse)TargetItem ).Owner as BaseCreature;

                if( ( (Corpse)TargetItem ).Owner == null || !( ( (Corpse)TargetItem ).Owner is BaseCreature ) )
                {
                    Caster.SendMessage( "Error: corpse's owner not found." );
                    return;
                }

                if( creature.Controlled )
                    Caster.SendMessage( "This spell cannot be used on controlled creatures." );

                else if( creature is IUndead || creature.Hue == 12345678 || creature.Hue == 2964 )
                    Caster.SendMessage( "This spell cannot be used on undead creatures." );

                else if( (Caster.Followers + 1) >= Caster.FollowersMax )
                    Caster.SendMessage( "You need at least two free follower slots to cast this spell." );

                else if( !Caster.InLOS( TargetItem ) || Caster.Map != TargetItem.Map || Caster.GetDistanceToSqrt( TargetItem ) > 5 || !Caster.CanSee( TargetItem ) )
                    Caster.SendMessage( "That is too far away." );

                else if( ( (Corpse)TargetItem ).Channeled )
                    Caster.SendMessage( "That corpse has already been desecrated." );

                else
                {
                    Caster.Mana -= ManaCost;
                    FinalEffect( Caster, (Corpse)TargetItem, creature );
                    Success = true;
                }
            }
        }

        public static void FinalEffect( Mobile caster, Corpse corpse, BaseCreature creature )
        {
            int power = 1;

            if( creature.Fame > 5000 )
                power = creature.Fame / 5000;

            GenericWarrior summoned = new GenericWarrior();

            summoned.RawHits = 75 * power;
            summoned.Hits = summoned.RawHits;
            summoned.DamageMin = 15 + ( 2 * power );
            summoned.DamageMax = 20 + ( 2 * power );
            summoned.BodyValue = creature.BodyValue;
            summoned.BaseSoundID = 471;
            summoned.Hue = 2964;
            summoned.Name = "An Animated Corpse";
            summoned.ControlSlots = 2;

            if( summoned.BodyValue == 400 || summoned.BodyValue == 401 )
            {
                Club club = new Club();
                summoned.EquipItem( club );
                summoned.EquipItem( new ShortPants( 2594 ) );
                summoned.EquipItem( new Shirt( 2594 ) );
                summoned.EquipItem( new WaistSash( 2581 ) );
                summoned.HairItemID = creature.HairItemID;
                summoned.HairHue = 2964;
                summoned.FacialHairItemID = creature.FacialHairItemID;
                summoned.FacialHairHue = 2964;
                summoned.HasNoCorpse = true;
            }

            corpse.Channeled = true;
            Summon( caster, summoned, 30, 534, false );
            summoned.Location = corpse.Location;
            summoned.Emote( "*is raised from the dead to serve " + caster.Name + "*" );
        }
    }
}
