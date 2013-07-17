using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class BarbarianAxe : BaseAxe
	{
		public override string NameType{ get{ return "barbarian's axe"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.DoubleStrike; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
		public override int SheathedMaleBackID{ get{ return 15172; } }

		public override int AosStrengthReq{ get{ return 75; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 19; } }
		public override int AosMaxDamage{ get{ return 19; } }
		public override double AosSpeed{ get{ return 4.75; } }

		public override int OldStrengthReq{ get{ return 45; } }
		public override int OldMinDamage{ get{ return 5; } }
		public override int OldMaxDamage{ get{ return 35; } }
		public override int OldSpeed{ get{ return 37; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }

		[Constructable]
		public BarbarianAxe() : base( 0x3CC2 )
		{
			Weight = 10.0;
			Name = "barbarian's axe";
			AosElementDamages.Slashing = 15;
			AosElementDamages.Blunt = 85;
		}

		public BarbarianAxe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
