using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class BoneAxe : BaseAxe, IBoneArmour
	{
		public override string NameType{ get{ return "bone Axe"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.CrushingBlow; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Dismount; } }

		public override int SheathedMaleBackID{ get{ return 15170; } }
		public override int SheathedFemaleBackID{ get{ return 15171; } }
		
		public override int AosStrengthReq{ get{ return 45; } }
		public override double OverheadPercentage{ get{ return 0.5; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 17; } }
		public override int AosMaxDamage{ get{ return 17; } }
		public override double AosSpeed{ get{ return 4.25; } }

		public override int OldStrengthReq{ get{ return 35; } }
		public override int OldMinDamage{ get{ return 6; } }
		public override int OldMaxDamage{ get{ return 33; } }
		public override int OldSpeed{ get{ return 37; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }

		[Constructable]
		public BoneAxe() : base( 0x3CC8 )
		{
			Weight = 7.0;
			Name = "bone Axe";
			AosElementDamages.Slashing = 20;
			AosElementDamages.Blunt = 80;
		}

		public BoneAxe( Serial serial ) : base( serial )
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
