using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class KhemetarFalchion : BaseSword
	{
		public override string NameType{ get{ return "Khemetar Falchion"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }

		public override int SheathedMaleBackID{ get{ return 15156; } }
		public override int SheathedFemaleBackID{ get{ return 15157; } }

		public override int AosStrengthReq{ get{ return 45; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 18; } }
		public override int AosMaxDamage{ get{ return 18; } }
		public override double AosSpeed{ get{ return 4.5; } }

		public override int OldStrengthReq{ get{ return 25; } }
		public override int OldMinDamage{ get{ return 5; } }
		public override int OldMaxDamage{ get{ return 33; } }
		public override int OldSpeed{ get{ return 35; } }

		public override int DefHitSound{ get{ return 0x23B; } }
		public override int DefMissSound{ get{ return 0x239; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }

		[Constructable]
		public KhemetarFalchion() : base( 0x3CF7 )
		{
			Weight = 7.0;
			Name = "Khemetar Falchion";
			AosElementDamages.Slashing = 100;
		}

		public KhemetarFalchion( Serial serial ) : base( serial )
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
