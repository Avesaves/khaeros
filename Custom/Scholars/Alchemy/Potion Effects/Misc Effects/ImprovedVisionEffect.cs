using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class ImprovedVisionEffect : CustomPotionEffect
    {
        public override string Name { get { return "Improved Vision"; } }

        public override void ApplyEffect(Mobile to, Mobile source, int intensity, Item itemSource)
        {
            if (to.BeginAction(typeof(LightCycle)))
            {
                int oldLevel = to.LightLevel;

                int level = TimeSystem.EffectsEngine.GetNightSightLevel(to, oldLevel);

                if (level > -1)
                {
                    to.LightLevel = level;

                    TimeSystem.EffectsEngine.SetNightSightOn(to, oldLevel);
                }
                else
                {
                    to.EndAction(typeof(LightCycle));

                    to.SendMessage("The potion seems to have no effect.");
                }
            }
        }

        public override bool CanDrink(Mobile mobile)
        {
            return true;
        }
    }
}

