using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{

	public class Talwar : BaseSword
	{
		public override string NameType{ get{ return "talwar"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.DoubleStrike; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MortalStrike; } }
		
		public override int SheathedMaleBackID{ get{ return 15175; } }
		public override int SheathedFemaleBackID{ get{ return 15176; } }

		public override int AosStrengthReq{ get{ return 55; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 13; } }
		public override int AosMaxDamage{ get{ return 13; } }
		public override double AosSpeed{ get{ return 3.25; } }

		public override int OldStrengthReq{ get{ return 55; } }
		public override int OldMinDamage{ get{ return 11; } }
		public override int OldMaxDamage{ get{ return 14; } }
		public override int OldSpeed{ get{ return 47; } }

		public override int DefHitSound{ get{ return 0x23B; } }
		public override int DefMissSound{ get{ return 0x239; } }

		public override int InitMinHits{ get{ return 51; } }
		public override int InitMaxHits{ get{ return 80; } }

		[Constructable]
		public Talwar() : base( 0x3D02 )
		{
			Weight = 2.0;
			Name = "talwar";
			AosElementDamages.Slashing = 100;
		}

		public Talwar( Serial serial ) : base( serial )
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
