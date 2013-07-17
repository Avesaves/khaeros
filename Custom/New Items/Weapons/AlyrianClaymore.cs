using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x26CE, 0x26CF )]
	public class Claymore : BaseSword
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.CrushingBlow; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }

		public override string NameType { get { return "claymore"; } }
		public override int SheathedMaleBackID{ get{ return 15193; } }
		public override int SheathedFemaleBackID{ get{ return 15194; } }
		
		public override int AosStrengthReq{ get{ return 65; } }
		public override double OverheadPercentage{ get{ return 0.3; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.2; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 18; } }
		public override int AosMaxDamage{ get{ return 18; } }
		public override double AosSpeed{ get{ return 4.5; } }

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
		//public Claymore() : base( 0x26CE )
		public Claymore() : base( 0x3CDC )
		{
			Weight = 9.0;
			Layer = Layer.TwoHanded;
			AosElementDamages.Slashing = 80;
			AosElementDamages.Blunt = 20;
			Name = "claymore";
		}

		public Claymore( Serial serial ) : base( serial )
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
