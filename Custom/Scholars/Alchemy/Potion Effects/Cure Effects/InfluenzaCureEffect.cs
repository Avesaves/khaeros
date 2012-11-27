using Server.Engines.XmlSpawner2;
namespace Server.Items
{
    public class InfluenzaCureEffect : CustomPotionEffect
    {
        public override string Name { get { return "Influenza Cure"; } }

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
