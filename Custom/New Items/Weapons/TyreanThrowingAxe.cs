using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class ThrowingAxe : BaseAxe
	{
		public override string NameType{ get{ return "throwing axe"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.CrushingBlow; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Dismount; } }
		
		public override int SheathedMaleBackID{ get{ return 15173; } }
		public override int SheathedFemaleBackID{ get{ return 15174; } }

		public override int AosStrengthReq{ get{ return 25; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 13; } }
		public override int AosMaxDamage{ get{ return 13; } }
		public override double AosSpeed{ get{ return 3.25; } }

		public override int OldStrengthReq{ get{ return 35; } }
		public override int OldMinDamage{ get{ return 6; } }
		public override int OldMaxDamage{ get{ return 33; } }
		public override int OldSpeed{ get{ return 37; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }
		
		public override bool Throwable { get { return true; } }

		[Constructable]
		public ThrowingAxe() : base( 0x3CB2 )
		{
			Weight = 5.0;
			Name = "throwing axe";
			AosElementDamages.Blunt = 10;
			AosElementDamages.Slashing = 90;
		}

		public ThrowingAxe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if( version < 1 )
            {
                AosElementDamages.Blunt = 10;
                AosElementDamages.Slashing = 90;
            }
		}
	}
}
