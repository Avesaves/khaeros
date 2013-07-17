using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Items
{
	public class CompositeRecurveBow : BaseRanged
	{
		public override string NameType{ get{ return "composite recurve bow"; } }
		public override int EffectID{ get{ return 0xF42; } }
		public override Type AmmoType{ get{ return typeof( Arrow ); } }
		public override Item Ammo{ get{ return new Arrow(); } }

		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MovingShot; } }

		public override int AosStrengthReq{ get{ return 25; } }
		public override double OverheadPercentage{ get{ return 0; } }
		public override double SwingPercentage{ get{ return 0; } }
		public override double ThrustPercentage{ get{ return 0; } }
		public override double RangedPercentage{ get{ return 1; } }
		public override int AosMinDamage{ get{ return 2; } }
		public override int AosMaxDamage{ get{ return 2; } }
		public override double AosSpeed{ get{ return 2.75; } }

		public override int OldStrengthReq{ get{ return 25; } }
		public override int OldMinDamage{ get{ return 15; } }
		public override int OldMaxDamage{ get{ return 17; } }
		public override int OldSpeed{ get{ return 32; } }		

		public override int DefMaxRange{ get{ return 10; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

		[Constructable]
		public CompositeRecurveBow() : base( 0x3E14 )
		{
			Weight = 5.0;
			Name = "composite recurve bow";
			AosElementDamages.Piercing = 100;
		}

		public CompositeRecurveBow( Serial serial ) : base( serial )
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
