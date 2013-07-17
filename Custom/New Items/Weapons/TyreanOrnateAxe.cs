using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2D28, 0x2D34 )]
	public class OrnateAxe : BaseAxe
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.Disarm; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.CrushingBlow; } }
		public override int SheathedMaleBackID{ get{ return 15179; } }
		public override int SheathedFemaleBackID{ get{ return 15180; } }

		public override string NameType { get { return "ornate axe"; } }
		
		public override int AosStrengthReq{ get{ return 45; } }
		public override double OverheadPercentage{ get{ return 0.5; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 16; } }
		public override int AosMaxDamage{ get{ return 16; } }
		public override double AosSpeed{ get{ return 4; } }

		public override int OldStrengthReq{ get{ return 45; } }
		public override int OldMinDamage{ get{ return 18; } }
		public override int OldMaxDamage{ get{ return 20; } }
		public override int OldSpeed{ get{ return 26; } }

		public override int DefMissSound{ get{ return 0x239; } }

		public override int InitMinHits{ get{ return 30; } }
		public override int InitMaxHits{ get{ return 60; } }
		
		public override SkillName DefSkill{ get{ return SkillName.Axemanship; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash2H; } }

		[Constructable]
		public OrnateAxe() : base( 0x2D28 )
		{
			Weight = 7.0;
			Layer = Layer.OneHanded;
			AosElementDamages.Slashing = 90;
			AosElementDamages.Blunt = 10;
			Name = "ornate axe";
		}

		public OrnateAxe( Serial serial ) : base( serial )
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
