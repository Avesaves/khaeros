using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Protean : BaseFeat
    {
        public override string Name { get { return "Protean"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Protean; } }
        public override FeatCost CostLevel { get { return FeatCost.Medium; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.VampireAbilities }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "You have low-light vision."; } }
        public override string SecondDescription { get { return "You can use your blood to heal."; } }
        public override string ThirdDescription { get { return "You can use your blood to create claws with which to attack your enemies."; } }

        public override string FirstCommand { get { return ".vp vampsight"; } }
        public override string SecondCommand { get { return ".vp autoheal | .vp heal"; } }
        public override string ThirdCommand { get { return ".vp claws"; } }

        public override string FullDescription { get { return GetFullDescription( this ); } }

        public static void Initialize() { }

        public Protean() { }

        public override bool MeetsOurRequirements( PlayerMobile m )
        {
            if( !m.IsVampire )
                return false;

            return base.MeetsOurRequirements( m );
        }

        public override bool ShouldDisplayTo( PlayerMobile m )
        {
            if( !m.IsVampire )
                return false;

            return base.ShouldDisplayTo( m );
        }

        public static void HandleClaws( PlayerMobile m )
        {
            if( m.Feats.GetFeatLevel( FeatList.Protean ) < 3 )
            {
                m.SendMessage( "You need the third level of Protean to use this ability." );
                return;
            }

            if( m.BPs < 2 )
            {
                m.SendMessage( "You lack enough blood points to use this ability." );
                return;
            }

            if( m.Claws != null )
            {
                m.SendMessage( "You have already grown claws." );
                return;
            }

            Item gloves = m.FindItemOnLayer(Layer.Gloves);

            if( gloves != null )
                m.Backpack.DropItem( gloves );

            m.ClearHands();

            Items.VampireClaws claws = new Server.Items.VampireClaws();
            m.EquipItem( claws );
            m.BPs -= 2;
            m.Emote( "*monstrous claws grow from the tips of " + m.GetPossessivePronoun() + " fingers*" );
            claws.DelayDelete();
            m.Claws = claws;
            m.PlaySound( 86 );
            m.Send( new MobileStatus( m, m ) );
        }
    }
}
