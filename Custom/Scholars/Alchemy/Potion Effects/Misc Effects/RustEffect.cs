using System;
using Server;

namespace Server.Items
{
	public class RustEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Rust"; } }
		private const double Divisor = 0.45; // max 0-45 durability damage at 100 intensity
		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( source != to )
				source.DoHarmful( to );
			int chance = Utility.RandomMinMax( 1, 8 );
			string sundname = "";
			
			BaseArmor sundered = null;
			Layer layer = Layer.FirstValid;
			
			switch( chance )
			{
				case 1: layer = Layer.InnerTorso; sundname = "armour"; break;
				case 2: layer = Layer.InnerLegs; sundname = "leggings"; break;
				case 3: layer = Layer.TwoHanded; sundname = "shield"; break;
				case 4: layer = Layer.Neck; sundname = "gorget"; break;
				case 5: layer = Layer.Gloves; sundname = "gauntlets"; break;
				case 6: layer = Layer.Helm; sundname = "helm"; break;
				case 7: layer = Layer.Arms; sundname = "arm pads"; break;
				case 8: layer = Layer.OneHanded; sundname = "weapon"; break;
			}
			
			if( to.FindItemOnLayer( layer ) != null && to.FindItemOnLayer( layer ) is BaseArmor )
				sundered = to.FindItemOnLayer( layer ) as BaseArmor;

			if( sundered != null )
			{
				int amt = (int)(intensity * Divisor);
				if ( amt <= 0 )
					amt = 0;

				sundered.HitPoints -= Utility.Random( amt ) + 1;

				if( sundered.HitPoints < 0 )
				{
					sundered.MaxHitPoints += sundered.HitPoints;
					sundered.HitPoints = 0;

					if( sundered.MaxHitPoints < 0 )
					{
						sundered.Delete();
						to.Emote( "*got {0} {1} destroyed by {2}*", to.Female == true ? "her" : "his", sundname, source.Name );
					}
				}

				to.Emote( "*got {0} {1} damaged by {2}*", to.Female == true ? "her" : "his", sundname, source.Name );
			}
		}

		public override bool CanDrink( Mobile mobile )
		{
			return true;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
		}
	}
}
