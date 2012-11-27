using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class InfluenzaEffect : CustomPotionEffect
    {
        public override string Name { get { return "Potion"; } }

        public override void ApplyEffect(Mobile to, Mobile source, int intensity, Item itemSource)
        {
            HealthAttachment.TryTreatDisease(to, Disease.Influenza, intensity);
        }

        public override bool CanDrink(Mobile mobile)
        {
            return true;
        }
    }
}
