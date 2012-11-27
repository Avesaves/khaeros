using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	[FlipableAttribute( 0x2D2B, 0x2D1F )]
	public class CompositeShortbow : BaseRanged
	{
		public override int EffectID{ get{ return 0xF42; } }
		public override Type AmmoType{ get{ return typeof( Arrow ); } }
		public override Item Ammo{ get{ return new Arrow(); } }

		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.LightningArrow; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.PsychicAttack; } }

		public override int AosStrengthReq{ get{ return 35; } }
		public override double OverheadPercentage{ get{ return 0; } }
		public override double SwingPercentage{ get{ return 0; } }
		public override double ThrustPercentage{ get{ return 0; } }
		public override double RangedPercentage{ get{ return 1; } }
		public override int AosMinDamage{ get{ return 3; } }
		public override int AosMaxDamage{ get{ return 3; } }
		public override double AosSpeed{ get{ return 3; } }

		public override int OldStrengthReq{ get{ return 45; } }
		public override int OldMinDamage{ get{ return 14; } }
		public override int OldMaxDamage{ get{ return 16; } }
		public override int OldSpeed{ get{ return 38; } }

		public override int DefMaxRange{ get{ return 8; } }

		public override int InitMinHits{ get{ return 41; } }
		public override int InitMaxHits{ get{ return 90; } }
		
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

		[Constructable]
		public CompositeShortbow() : base( 0x2D2B )
		{
			Weight = 6.0;
			Name = "Composite Shortbow";
			AosElementDamages.Piercing = 100;
		}

		public CompositeShortbow( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
			
			if( version < 2 )
			{
				Animation = WeaponAnimation.ShootBow;
			}
		}
	}
}
