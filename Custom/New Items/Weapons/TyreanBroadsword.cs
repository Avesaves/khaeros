using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13B9, 0x13Ba )]
	public class Broadsword : BaseSword
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.CrushingBlow; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }
		public override string NameType { get { return "broadsword"; } }
		
		public override int SheathedMaleWaistID{ get{ return 15218; } }
		public override int SheathedFemaleWaistID{ get{ return 15219; } }
		
		public override int AosStrengthReq{ get{ return 45; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.2; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 15; } }
		public override int AosMaxDamage{ get{ return 15; } }
		public override double AosSpeed{ get{ return 3.75; } }

		public override int OldStrengthReq{ get{ return 40; } }
		public override int OldMinDamage{ get{ return 6; } }
		public override int OldMaxDamage{ get{ return 34; } }
		public override int OldSpeed{ get{ return 30; } }

		public override int DefHitSound{ get{ return 0x237; } }
		public override int DefMissSound{ get{ return 0x23A; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 100; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash2H; } }

		[Constructable]
		public Broadsword() : base( 0x3CF5 )
		{
			Weight = 7.0;
			AosElementDamages.Slashing = 90;
			AosElementDamages.Blunt = 10;
			Layer = Layer.OneHanded;
			Name = "broadsword";
		}

		public Broadsword( Serial serial ) : base( serial )
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
