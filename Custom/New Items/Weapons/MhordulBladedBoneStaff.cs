using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class MhordulBladedBoneStaff : BaseSpear, IBoneArmour
	{
		public override string NameType{ get{ return "bone Bladed Staff"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.DoubleStrike; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.InfectiousStrike; } }

		public override int AosStrengthReq{ get{ return 45; } }
		public override double OverheadPercentage{ get{ return 0.2; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.3; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 14; } }
		public override int AosMaxDamage{ get{ return 14; } }
		public override double AosSpeed{ get{ return 3.5; } }

		public override int OldStrengthReq{ get{ return 50; } }
		public override int OldMinDamage{ get{ return 12; } }
		public override int OldMaxDamage{ get{ return 13; } }
		public override int OldSpeed{ get{ return 49; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }
		
		public override SkillName DefSkill{ get{ return SkillName.ExoticWeaponry; } }

		[Constructable]
		public MhordulBladedBoneStaff() : base( 0x3CD5 )
		{
			Weight = 7.0;
			Name = "Mhordul Bladed Bone Staff";
			AosElementDamages.Slashing = 100;
		}

		public MhordulBladedBoneStaff( Serial serial ) : base( serial )
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
