﻿using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class TattooArtist : BaseFeat
    {
        public override string Name { get { return "Tattoo Artist"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.TattooArtist; } }
        public override FeatCost CostLevel { get { return FeatCost.Low; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription
        {
            get
            {
                return "This skill will allow you to work with tattoos. In order to access it, you must use the tattoo artist's tool. " +
					"[Can remove and add tattoos]";
            }
        }
        public override string SecondDescription { get { return "[Can color tattoos]"; } }
        public override string ThirdDescription { get { return "[Can set names and descriptions for tattoos]"; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription( this ); } }

        public static void Initialize() { WriteWebpage( new TattooArtist() ); }

        public TattooArtist() { }
    }
}
