using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Shapeshift : BaseFeat
    {
        public override string Name { get { return "Shapeshift"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Shapeshift; } }
        public override FeatCost CostLevel { get { return FeatCost.Medium; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.VampireAbilities }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "Allows you to turn into a variety of creatures. [costs 10 blood points]"; } }
        public override string SecondDescription { get { return "[costs 5 blood points]"; } }
        public override string ThirdDescription { get { return "[costs 1 blood points]"; } }

        public override string FirstCommand { get { return ".vp shapeshift human | .vp shapeshift wolf | .vp shapeshift bat | .vp shapeshift cat | .vp shapeshift rat"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription( this ); } }

        public static void Initialize() { }

        public Shapeshift() { }

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

        public static void HandleShapeshift( PlayerMobile m, string choice )
        {
            if( choice == "human" )
            {
                if( m.BodyValue == 400 || m.BodyValue == 401 )
                    m.SendMessage( "You are already in human form." );

                else
                {
                    if( m.Female )
                        m.BodyValue = 401;

                    else
                        m.BodyValue = 400;

                    m.Emote( "*turns back into human form*" );
                    m.HueMod = -1;
                    m.NameMod = null;
                    m.InvalidateProperties();
                }
            }

            else if( choice == "wolf" || choice == "bat" || choice == "rat" || choice == "cat" )
            {
                int cost = 10;

                if( m.Feats.GetFeatLevel( FeatList.Shapeshift ) > 2 )
                    cost = 1;

                else if( m.Feats.GetFeatLevel( FeatList.Shapeshift ) == 2 )
                    cost = 5;

                if( m.BPs < cost )
                    m.SendMessage( "You lack enough blood points to use that ability." );

                else
                {

                    m.Emote( "*turns into a " + choice + "*" );
                    m.HueMod = 2881;
                    m.NameMod = "a black " + choice;

                    int body = 225;

                    if( choice == "cat" )
                        body = 201;
                    else if( choice == "rat" )
                        body = 238;
                    else if( choice == "bat" )
                        body = 317;

                    m.BodyValue = body;
                    m.BPs -= cost;
                    m.InvalidateProperties();
                }
            }

            else
                m.SendMessage( "Invalid option." );
        }
    }
}
