using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0xf45, 0xf46 )]
	public class HaftedAxe : BaseAxe
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.BleedAttack; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MortalStrike; } }
		public override int SheathedMaleBackID{ get{ return 15179; } }
		public override int SheathedFemaleBackID{ get{ return 15180; } }
		
		public override string NameType { get { return "hafted axe"; } }
		public override int AosStrengthReq{ get{ return 55; } }
		public override double OverheadPercentage{ get{ return 0.3; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.3; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 17; } }
		public override int AosMaxDamage{ get{ return 17; } }
		public override double AosSpeed{ get{ return 4.25; } }

		public override int OldStrengthReq{ get{ return 35; } }
		public override int OldMinDamage{ get{ return 6; } }
		public override int OldMaxDamage{ get{ return 33; } }
		public override int OldSpeed{ get{ return 37; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		[Constructable]
		public HaftedAxe() : base( 0xF45 )
		{
			Weight = 8.0;
			Layer = Layer.TwoHanded;
			AosElementDamages.Blunt = 15;
            AosElementDamages.Slashing = 85;
            Name = "hafted axe";
		}

		public HaftedAxe( Serial serial ) : base( serial )
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
