using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class OintmentEffect : CustomPotionEffect
    {
        public override string Name { get { return "Ointment"; } }

        public override void ApplyEffect(Mobile to, Mobile source, int intensity, Item itemSource)
        {
            HealthAttachment.TryTreatDisease(to, Disease.Leprosy, intensity);
        }

        public override bool CanDrink(Mobile mobile)
        {
            return true;
        }
    }
}
