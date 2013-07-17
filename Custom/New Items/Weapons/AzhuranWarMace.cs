using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class SpikedMace : BaseBashing
	{
		public override string NameType{ get{ return "spiked mace"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Disarm; } }
		public override int SheathedMaleBackID{ get{ return 15181; } }
		public override int SheathedFemaleBackID{ get{ return 15182; } }
		
		public override int AosStrengthReq{ get{ return 35; } }
		public override double OverheadPercentage{ get{ return 0.5; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 11; } }
		public override int AosMaxDamage{ get{ return 11; } }
		public override double AosSpeed{ get{ return 2.75; } }

		public override int OldStrengthReq{ get{ return 20; } }
		public override int OldMinDamage{ get{ return 8; } }
		public override int OldMaxDamage{ get{ return 32; } }
		public override int OldSpeed{ get{ return 30; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		[Constructable]
		public SpikedMace() : base( 0x3DEF )
		{
			Weight = 6.0;
			Name = "spiked mace";
			AosElementDamages.Blunt = 40;
			AosElementDamages.Slashing = 60;
		}

		public SpikedMace( Serial serial ) : base( serial )
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
