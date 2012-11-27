using System;
using Server;

namespace Server.Items
{
	public class CureEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Cure"; } }

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( intensity < 0 || !to.Poisoned )
				return;

			// handled by the poisoning system
			if ( !PoisonEffect.Cure( to, BasePotion.Scale( to, intensity ) ) )
				to.SendMessage( "The potion was not strong enough to cure the poison!" );
		}

		public override bool CanDrink( Mobile mobile )
		{
			return mobile.Poisoned;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
		}
	}
}
