using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0xF43, 0xF44 )]
	public class Hatchet : BaseAxe
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Disarm; } }
		public override int SheathedMaleBackID{ get{ return 15173; } }
		public override int SheathedFemaleBackID{ get{ return 15174; } }
		
		public override string NameType { get { return "Hatchet"; } }
		public override int AosStrengthReq{ get{ return 10; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 15; } }
		public override int AosMaxDamage{ get{ return 15; } }
		public override double AosSpeed{ get{ return 3.75; } }

		public override int OldStrengthReq{ get{ return 15; } }
		public override int OldMinDamage{ get{ return 2; } }
		public override int OldMaxDamage{ get{ return 17; } }
		public override int OldSpeed{ get{ return 40; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }
		
		public override bool Throwable { get { return true; } }

		[Constructable]
		public Hatchet() : base( 0xF43 )
		{
			Weight = 4.0;
			Layer = Layer.TwoHanded;
			AosElementDamages.Blunt = 10;
            AosElementDamages.Slashing = 90;
		}

		public Hatchet( Serial serial ) : base( serial )
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
