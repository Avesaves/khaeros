using System;
using System.Collections.Generic;

namespace Server.Items
{

    public class FairyWing : BaseIngredient, IDrinkIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        { 
            get
            {
                return new KeyValuePair<CustomEffect, int>[]
                { 
				    new KeyValuePair<CustomEffect, int>(CustomEffect.InfluenzaCure, 40)
                };
            }
        }
                    

        [Constructable]
        public FairyWing()
            : base(0xF78)
        {
            Weight = 1.0;
            Name = "hag hair";
            Hue = 682;
        }

        public FairyWing(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override int SkillRequired { get { return 400; } }


        bool IDrinkIngredient.CanUse(Mobile mobile)
        {
            return true;
        }

        int IDrinkIngredient.PotionBooster { get { return 0; } }
    }
}

