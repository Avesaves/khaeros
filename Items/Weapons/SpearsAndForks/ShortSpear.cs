using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1403, 0x1402 )]
	public class ShortSpear : BasePoleArm
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ShadowStrike; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MortalStrike; } }
		public override int SheathedMaleBackID{ get{ return 15204; } }
		public override int SheathedFemaleBackID{ get{ return 15205; } }
		
		public override string NameType { get { return "Short Spear"; } }

		public override bool Unwieldy{ get{ return false; } }
		public override bool CanThrustOnMount{ get{ return true; } }
		public override bool CanUseDefensiveFormation{ get{ return false; } }
		public override bool CannotUseOnMount{ get{ return false; } }
		
		public override int AosStrengthReq{ get{ return 35; } }
		public override double OverheadPercentage{ get{ return 0.1; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.5; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 11; } }
		public override int AosMaxDamage{ get{ return 11; } }
		public override double AosSpeed{ get{ return 2.75; } }

		public override int OldStrengthReq{ get{ return 15; } }
		public override int OldMinDamage{ get{ return 4; } }
		public override int OldMaxDamage{ get{ return 32; } }
		public override int OldSpeed{ get{ return 50; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }
		
		public override int DefHitSound{ get{ return 0x23C; } }
		public override int DefMissSound{ get{ return 0x238; } }
		
		public override bool Throwable { get { return true; } }
		
		public override SkillName DefSkill{ get{ return SkillName.Polearms; } }
		
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public ShortSpear() : base( 0x1403 )
		{
			Weight = 6.0;
			AosElementDamages.Piercing = 100;
			Layer = Layer.OneHanded;
			MaxRange = 1;
		}

		public ShortSpear( Serial serial ) : base( serial )
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
			
			switch ( version )
			{
				case 1: goto case 0;
				case 0:
				{
					if ( MaxRange >= 2 )
						MaxRange = 1;
					break;
				}
			}
		}
	}
}
