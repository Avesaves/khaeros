using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class BoneSword : BaseSword, IBoneArmour
	{
		public override string NameType{ get{ return "bone Sword"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }
		public override int SheathedMaleBackID{ get{ return 15200; } }
		public override int SheathedFemaleBackID{ get{ return 15201; } }

		public override int AosStrengthReq{ get{ return 20; } }
		public override double OverheadPercentage{ get{ return 0.3; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.2; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 12; } }
		public override int AosMaxDamage{ get{ return 12; } }
		public override double AosSpeed{ get{ return 3; } }

		public override int OldStrengthReq{ get{ return 25; } }
		public override int OldMinDamage{ get{ return 5; } }
		public override int OldMaxDamage{ get{ return 33; } }
		public override int OldSpeed{ get{ return 35; } }

		public override int DefHitSound{ get{ return 0x23B; } }
		public override int DefMissSound{ get{ return 0x239; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }

		[Constructable]
		public BoneSword() : base( 0x3CD7 )
		{
			Weight = 4.0;
			Name = "bone Sword";
			AosElementDamages.Slashing = 100;
		}

		public BoneSword( Serial serial ) : base( serial )
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
