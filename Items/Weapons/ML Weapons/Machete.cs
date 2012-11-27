using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2D35, 0x2D29 )]
	public class Machete : BaseSword
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.DefenseMastery; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Bladeweave; } }
		
		public override int SheathedMaleBackID{ get{ return 15168; } }
		public override int SheathedFemaleBackID{ get{ return 15169; } }

		public override string NameType { get { return "Machete"; } }
		
		public override int AosStrengthReq{ get{ return 15; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.2; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 11; } }
		public override int AosMaxDamage{ get{ return 11; } }
		public override double AosSpeed{ get{ return 2.75; } }

		public override int OldStrengthReq{ get{ return 20; } }
		public override int OldMinDamage{ get{ return 13; } }
		public override int OldMaxDamage{ get{ return 15; } }
		public override int OldSpeed{ get{ return 41; } }

		public override int DefHitSound{ get{ return 0x23B; } }
		public override int DefMissSound{ get{ return 0x239; } }

		public override int InitMinHits{ get{ return 30; } }
		public override int InitMaxHits{ get{ return 60; } }

		[Constructable]
		public Machete() : base( 0x2D35 )
		{
			Weight = 3.0;
			Name = "Machete";
			AosElementDamages.Slashing = 100;
		}

		public Machete( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
			
			if( version < 2 )
			{
				Weight = 3.0;
				MaxDamage = 5;
				MinDamage = 3;
				StrRequirement = 15;
				Speed = 37;
			}	
		}
	}
}
