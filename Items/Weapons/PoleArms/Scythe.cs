using System;
using Server.Network;
using Server.Items;
using Server.Engines.Harvest;

namespace Server.Items
{
	[FlipableAttribute( 0x26BA, 0x26C4 )]
	public class Scythe : BaseSword
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.BleedAttack; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }
		public override SkillName DefSkill{ get{ return SkillName.ExoticWeaponry; } }
		public override string NameType { get { return "Scythe"; } }
		public override int AosStrengthReq{ get{ return 75; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 18; } }
		public override int AosMaxDamage{ get{ return 18; } }
		public override double AosSpeed{ get{ return 4.5; } }

		public override int OldStrengthReq{ get{ return 45; } }
		public override int OldMinDamage{ get{ return 15; } }
		public override int OldMaxDamage{ get{ return 18; } }
		public override int OldSpeed{ get{ return 32; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 100; } }
		
		public override int DefHitSound{ get{ return 0x237; } }
		public override int DefMissSound{ get{ return 0x238; } }
		
		public override WeaponType DefType{ get{ return WeaponType.Polearm; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash2H; } }

		//public override HarvestSystem HarvestSystem{ get{ return null; } }

		[Constructable]
		public Scythe() : base( 0x26BA )
		{
			Weight = 9.0;
			Layer = Layer.TwoHanded;
			AosElementDamages.Slashing = 80;
			AosElementDamages.Piercing = 20;
		}

		public Scythe( Serial serial ) : base( serial )
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
