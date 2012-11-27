using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class LightHammer : BaseBashing
	{
		public override string NameType{ get{ return "Light Hammer"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.CrushingBlow; } }
		public override int SheathedMaleBackID{ get{ return 15181; } }
		public override int SheathedFemaleBackID{ get{ return 15182; } }

		public override int AosStrengthReq{ get{ return 25; } }
		public override double OverheadPercentage{ get{ return 0.5; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 10; } }
		public override int AosMaxDamage{ get{ return 10; } }
		public override double AosSpeed{ get{ return 2.5; } }

		public override int OldStrengthReq{ get{ return 40; } }
		public override int OldMinDamage{ get{ return 8; } }
		public override int OldMaxDamage{ get{ return 36; } }
		public override int OldSpeed{ get{ return 31; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Bash1H; } }

		[Constructable]
		public LightHammer() : base( 0x3DE8 )
		{
			Weight = 5.0;
			Name = "Light Hammer";
			AosElementDamages.Blunt = 100;
		}

        public LightHammer(Serial serial)
            : base( serial )
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
