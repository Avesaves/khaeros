using System;
using Server.Network;
using Server.Items;
using Server.Engines.Harvest;

namespace Server.Items
{
	[FlipableAttribute( 0x27AD, 0x27F8 )]
	public class DualPicks : BaseAxe
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.DefenseMastery; } }

		public override string NameType { get { return "Dual Picks"; } }
		
		public override HarvestSystem HarvestSystem{ get{ return Mining.System; } }
		
		public override int AosStrengthReq{ get{ return 55; } }
		public override double OverheadPercentage{ get{ return 0.5; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 14; } }
		public override int AosMaxDamage{ get{ return 14; } }
		public override double AosSpeed{ get{ return 3.5; } }
		
		public override bool CannotUseOnMount{ get{ return true; } }

		public override int OldStrengthReq{ get{ return 15; } }
		public override int OldMinDamage{ get{ return 9; } }
		public override int OldMaxDamage{ get{ return 11; } }
		public override int OldSpeed{ get{ return 55; } }

		public override int DefHitSound{ get{ return 0x232; } }
		public override int DefMissSound{ get{ return 0x238; } }

		public override int InitMinHits{ get{ return 35; } }
		public override int InitMaxHits{ get{ return 60; } }

		public override SkillName DefSkill{ get{ return SkillName.ExoticWeaponry; } }
		
		//public override bool Throwable { get { return true; } }

		[Constructable]
		public DualPicks() : base( 0x27AD )
		{
			Weight = 8.0;
			Layer = Layer.TwoHanded;
			Name = "Dual Picks";
			AosElementDamages.Slashing = 90;
			AosElementDamages.Blunt = 10;
		}

		public DualPicks( Serial serial ) : base( serial )
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
