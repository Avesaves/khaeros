using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class HorsemanAxe : BaseAxe
	{
		public override string NameType{ get{ return "horseman's axe"; } }
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.BleedAttack; } }
		public override int SheathedMaleBackID{ get{ return 15172; } }

		public override int AosStrengthReq{ get{ return 45; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 18; } }
		public override int AosMaxDamage{ get{ return 18; } }
		public override double AosSpeed{ get{ return 4.5; } }

		public override int OldStrengthReq{ get{ return 40; } }
		public override int OldMinDamage{ get{ return 6; } }
		public override int OldMaxDamage{ get{ return 38; } }
		public override int OldSpeed{ get{ return 30; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		[Constructable]
		public HorsemanAxe() : base( 0x3CAF )
		{
			Weight = 7.0;
			Name = "horseman's axe";
			AosElementDamages.Blunt = 20;
			AosElementDamages.Slashing = 80;

		}

		public HorsemanAxe( Serial serial ) : base( serial )
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
