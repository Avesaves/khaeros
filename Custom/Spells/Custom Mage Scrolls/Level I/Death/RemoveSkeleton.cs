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
    public class RemoveSkeletonScroll : CustomSpellScroll
    {
        public override CustomMageSpell Spell
        {
            get
            {
                return new RemoveSkeletonSpell();
            }
            set
            {
            }
        }

        [Constructable]
        public RemoveSkeletonScroll()
            : base()
        {
            Hue = 2687;
            Name = "A Remove Skeleton scroll";
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

            BaseCustomSpell.SpellInitiator( new RemoveSkeletonSpell( m, 1 ) );
        }

        public RemoveSkeletonScroll(Serial serial)
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
    public class RemoveSkeletonSpell : CustomMageSpell
    {
        public override CustomMageSpell GetNewInstance()
        {
            return new RemoveSkeletonSpell();
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
        public override string Name { get { return "RemoveSkeleton"; } }
        public override int ManaCost { get { return 50; } }
        public override int BaseRange { get { return 12; } }

        public RemoveSkeletonSpell()
            : this( null, 1 )
        {
        }

        public RemoveSkeletonSpell( Mobile caster, int featLevel )
            : base( caster, featLevel )
        {
            IconID = 6102;
            Range = 12;
            CustomName = "Remove Skeleton";
        }

        public override bool CanBeCast
        {
            get
            {
                return base.CanBeCast && HasRequiredArcanas( new FeatList[] { FeatList.DeathI } );
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

                if (creature.Controlled)
                    Caster.SendMessage("This spell cannot be used on controlled creatures.");

                else if (creature is IUndead || creature.Hue == 12345678 || creature.Hue == 2964)
                    Caster.SendMessage("This spell cannot be used on undead creatures.");

                else if ((Caster.Followers + 1) >= Caster.FollowersMax)
                    Caster.SendMessage("You need at least two free follower slots to cast this spell.");

                else if (!Caster.InLOS(TargetItem) || Caster.Map != TargetItem.Map || Caster.GetDistanceToSqrt(TargetItem) > 5 || !Caster.CanSee(TargetItem))
                    Caster.SendMessage("That is too far away.");

                else if (((Corpse)TargetItem).Channeled)
                    Caster.SendMessage("That corpse has already been desecrated.");

                else if (creature.BodyValue > 401 || creature.BodyValue < 400)
                    Caster.SendMessage("The skeleton of this creature is too difficult to remove.");

                else
                {
                    Caster.Mana -= ManaCost;
                    FinalEffect(Caster, (Corpse)TargetItem, creature);
                    Success = true;
                }
            }
        }

        public static void FinalEffect( Mobile caster, Corpse corpse, BaseCreature creature )
        {



            BaseCreature summoned = new Chicken() as BaseCreature;
            Random random = new Random();
            int randomN = random.Next(1, 20);
            if (randomN == 1)
                summoned = new SkeletalLord();
            else if (randomN >= 2 && randomN < 7)
                summoned = new SkeletalSoldier();
            else 
                summoned = new Skeleton();


            summoned.Name = "A skeleton";
            summoned.ControlSlots = 2;



            corpse.Channeled = true;
            Summon( caster, summoned, 30, 534, false );
            summoned.Location = corpse.Location;
            summoned.Emote( "*the grizzly bones of the desecrated corpse rise to serve " + caster.Name + "*" );
        }
    }
}
