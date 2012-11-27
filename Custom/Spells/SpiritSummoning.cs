using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
    public class SpiritSummoning : BaseCustomSpell
    {
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsItems { get { return true; } }
        public override bool UsesTarget { get { return true; } }
        public override bool UsesFaith { get { return true; } }
        public override bool UsesFullEffect { get { return true; } }
        public override FeatList Feat { get { return FeatList.CustomMageSpell; } }
        public override string Name { get { return "Spirit Summoning"; } }
        public override int BaseCost { get { return 20; } }
        public override bool BackpackItemsOnly { get { return false; } }

        private XmlSpiritSummoning m_Att;

        public SpiritSummoning( Mobile caster, int featLevel, XmlSpiritSummoning att )
            : base( caster, featLevel )
        {
            m_Att = att;
        }

        public override void Effect()
        {
            if( TargetCanBeAffected && TargetItem is Corpse && CasterHasEnoughMana )
            {
                BaseCreature creature = ( (Corpse)TargetItem ).Owner as BaseCreature;

                if( ( (Corpse)TargetItem ).Owner == null || !(( (Corpse)TargetItem ).Owner is BaseCreature) )
                {
                    Caster.SendMessage( "Error: corpse's owner not found." );
                    return;
                }

                if( creature.Controlled )
                    Caster.SendMessage( "This spell cannot be used on controlled creatures." );

                else if( creature is IUndead || creature.Hue == 12345678 || creature.Hue == 2964 )
                    Caster.SendMessage( "This spell cannot be used on undead creatures." );

                else if( Caster.Followers >= Caster.FollowersMax )
                    Caster.SendMessage( "You need at least one free follower slot to cast this spell." );

                else if( !Caster.InLOS( TargetItem ) || Caster.Map != TargetItem.Map || Caster.GetDistanceToSqrt( TargetItem ) > 5 || !Caster.CanSee( TargetItem ) )
                    Caster.SendMessage( "That is too far away." );

                else if( ( (Corpse)TargetItem ).Channeled )
                    Caster.SendMessage( "That corpse has already been desecrated." );

                else
                {
                    if( m_Att != null )
                        Caster.Mana -= m_Att.ManaCost;

                    else
                        Caster.Mana -= TotalCost;

                    FinalEffect( Caster, (Corpse)TargetItem, creature, m_Att );
                    Success = true;
                }
            }
        }

        public static void FinalEffect( Mobile caster, Corpse corpse, BaseCreature creature, XmlSpiritSummoning att )
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
            summoned.BaseSoundID = 442;
            summoned.Hue = 12345678;
            summoned.Name = "A Summoned Spirit";

            if( summoned.BodyValue == 400 || summoned.BodyValue == 401 )
            {
                Club club = new Club();
                club.Hue = 12345678;
                summoned.EquipItem( club );
                summoned.EquipItem( new RaggedPants( 12345678 ) );
                summoned.EquipItem( new Shirt( 12345678 ) );
                summoned.HairItemID = creature.HairItemID;
                summoned.HairHue = 12345678;
                summoned.FacialHairItemID = creature.FacialHairItemID;
                summoned.FacialHairHue = 12345678;
                summoned.HasNoCorpse = true;
            }

            corpse.Channeled = true;
            Summon( caster, summoned, 30, 534, false );
            summoned.Location = corpse.Location;
            summoned.Emote( "*is summoned from the spirit realm to serve " + caster.Name + "*" );

            if( att != null )
            {
                att.NextUseAllowed = DateTime.Now + att.CoolDown;
                att.Summoned = summoned;
            }
        }
    }
}
