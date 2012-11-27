using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x27AF, 0x27FA )]
	public class DualDaggers : BaseKnife
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.Block; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ArmorPierce; } }

		public override string NameType { get { return "Dual Daggers"; } }
		
		public override int SheathedMaleWaistID{ get{ return 15212; } }
		public override int SheathedFemaleWaistID{ get{ return 15213; } }
		
		public override int AosStrengthReq{ get{ return 15; } }
		public override double OverheadPercentage{ get{ return 0.3; } }
		public override double SwingPercentage{ get{ return 0.3; } }
		public override double ThrustPercentage{ get{ return 0.4; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 12; } }
		public override int AosMaxDamage{ get{ return 12; } }
		public override double AosSpeed{ get{ return 3; } }
		
		public override bool CannotUseOnMount{ get{ return true; } }

		public override int OldStrengthReq{ get{ return 15; } }
		public override int OldMinDamage{ get{ return 9; } }
		public override int OldMaxDamage{ get{ return 11; } }
		public override int OldSpeed{ get{ return 55; } }

		public override int InitMinHits{ get{ return 55; } }
		public override int InitMaxHits{ get{ return 60; } }

		public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public DualDaggers() : base( 0x27AF )
		{
			Weight = 3.0;
			Layer = Layer.TwoHanded;
			Name = "Dual Daggers";
			AosElementDamages.Piercing = 100;
		}

		public DualDaggers( Serial serial ) : base( serial )
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
