using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class Boomerang : BaseRanged
	{
		public override string NameType{ get{ return "Boomerang"; } }
		
		public override int EffectID{ get{ return 0x2AF2; } }
		public override Type AmmoType{ get{ return typeof( Boomerang ); } }
		public override Item Ammo{ get{ return new Boomerang(); } }

		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MovingShot; } }

		public override int AosStrengthReq{ get{ return 10; } }
		public override double OverheadPercentage{ get{ return 0; } }
		public override double SwingPercentage{ get{ return 0; } }
		public override double ThrustPercentage{ get{ return 0; } }
		public override double RangedPercentage{ get{ return 1; } }
		public override int AosMinDamage{ get{ return 4; } }
		public override int AosMaxDamage{ get{ return 4; } }
		public override double AosSpeed{ get{ return 3.25; } }

		public override int OldStrengthReq{ get{ return 45; } }
		public override int OldMinDamage{ get{ return 15; } }
		public override int OldMaxDamage{ get{ return 17; } }
		public override int OldSpeed{ get{ return 25; } }

		public override int DefMaxRange{ get{ return 8; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Bash1H; } }
		public override SkillName DefSkill{ get{ return SkillName.Throwing; } }
		public override WeaponType DefType{ get{ return WeaponType.Ranged; } }
		public override int DefHitSound{ get{ return 0x233; } }
		public override int DefMissSound{ get{ return 0x238; } }

		[Constructable]
		public Boomerang() : base( 0x3B27 )
		{
			Weight = 2.0;
			Name = "Boomerang";
			AosElementDamages.Blunt = 100;
		}

		public Boomerang( Serial serial ) : base( serial )
		{
		}
		
		public override bool OnFired( Mobile attacker, Mobile defender )
		{
			attacker.MovingEffect( defender, EffectID, 9, 1, false, false );
			defender.MovingEffect( attacker, EffectID, 9, 1, false, false );
			
			return true;
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
