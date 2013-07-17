using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2D20, 0x2D2C )]
	public class Kukri : BaseKnife
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.PsychicAttack; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.BleedAttack; } }

		public override string NameType { get { return "kukri"; } }
		
		public override int SheathedMaleWaistID{ get{ return 15212; } }
		public override int SheathedFemaleWaistID{ get{ return 15213; } }
		
		public override int AosStrengthReq{ get{ return 10; } }
		public override double OverheadPercentage{ get{ return 0.2; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.4; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 12; } }
		public override int AosMaxDamage{ get{ return 12; } }
		public override double AosSpeed{ get{ return 3; } }

		public override int OldStrengthReq{ get{ return 35; } }
		public override int OldMinDamage{ get{ return 12; } }
		public override int OldMaxDamage{ get{ return 14; } }
		public override int OldSpeed{ get{ return 44; } }

		public override int DefMissSound{ get{ return 0x239; } }

		public override int InitMinHits{ get{ return 30; } } // TODO
		public override int InitMaxHits{ get{ return 60; } } // TODO
		
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash1H; } }
		
		public override SkillName DefSkill{ get{ return SkillName.ExoticWeaponry; } }

		[Constructable]
		public Kukri() : base( 0x2D20 )
		{
			Weight = 2.0;
			Layer = Layer.OneHanded;
			Name = "kukri";
			AosElementDamages.Piercing = 30;
			AosElementDamages.Slashing = 70;
		}

		public Kukri( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
