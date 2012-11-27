using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class ProphetDiviningRod : BaseStaff
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.Block; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ForceOfNature; } }

		public override string NameType { get { return "Prophet's Divining Rod"; } }
		
		public override int AosStrengthReq{ get{ return 20; } }
		public override double OverheadPercentage{ get{ return 0.3; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.2; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 12; } }
		public override int AosMaxDamage{ get{ return 12; } }
		public override double AosSpeed{ get{ return 3; } }

		public override int OldStrengthReq{ get{ return 15; } }
		public override int OldMinDamage{ get{ return 10; } }
		public override int OldMaxDamage{ get{ return 12; } }
		public override int OldSpeed{ get{ return 48; } }

		public override int InitMinHits{ get{ return 30; } }
		public override int InitMaxHits{ get{ return 60; } }
		
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash1H; } }

		[Constructable]
		public ProphetDiviningRod() : base( 0x3DD0 )
		{
			Weight = 4.0;
			AosElementDamages.Blunt = 100;
			Name = "Prophet's Divining Rod";
		}

		public ProphetDiviningRod( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
