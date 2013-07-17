using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{

	public class HeavyWarMace : BaseBashing
	{
		public override string NameType{ get{ return "heavy war mace"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Disarm; } }
		public override int SheathedMaleBackID{ get{ return 15181; } }
		public override int SheathedFemaleBackID{ get{ return 15182; } }

		public override int AosStrengthReq{ get{ return 65; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.2; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 17; } }
		public override int AosMaxDamage{ get{ return 17; } }
		public override double AosSpeed{ get{ return 4.25; } }

		public override int OldStrengthReq{ get{ return 20; } }
		public override int OldMinDamage{ get{ return 8; } }
		public override int OldMaxDamage{ get{ return 32; } }
		public override int OldSpeed{ get{ return 30; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		[Constructable]
		public HeavyWarMace() : base( 0x3DF1 )
		{
			Weight = 9.0;
			Name = "heavy war mace";
			AosElementDamages.Blunt = 60;
			AosElementDamages.Piercing = 40;
		}

		public HeavyWarMace( Serial serial ) : base( serial )
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
