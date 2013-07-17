using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class BastardSword : BaseSword
	{
        private enum WieldingStyle
        {
            OneHanded,
            TwoHanded
        }

        private WieldingStyle wieldingStyle;

		public override string NameType{ get{ return "bastard sword"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }
		
		public override int SheathedMaleBackID{ get{ return 15193; } }
		public override int SheathedFemaleBackID{ get{ return 15194; } }
		
		public override bool BastardWeapon{ get { return true; } }

		public override int AosStrengthReq{ get{ return 35; } }
		public override double OverheadPercentage{ get{ return 0.3; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.2; } }
		public override double RangedPercentage{ get{ return 0; } }
        public override int AosMinDamage
        {
            get
            {
                if (this.wieldingStyle == WieldingStyle.OneHanded)
                    return 14;
                else
                    return 16;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                if (this.wieldingStyle == WieldingStyle.OneHanded)
                    return 14;
                else
                    return 16;
            }
        }
		public override double AosSpeed{ get{ return 3.5; } }

		public override int OldStrengthReq{ get{ return 25; } }
		public override int OldMinDamage{ get{ return 5; } }
		public override int OldMaxDamage{ get{ return 33; } }
		public override int OldSpeed{ get{ return 35; } }

		public override int DefHitSound{ get{ return 0x23B; } }
		public override int DefMissSound{ get{ return 0x239; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }

		[Constructable]
		public BastardSword() : base( 0x3CFD )
		{
			Weight = 7.0;
			Name = "bastard sword";
			AosElementDamages.Slashing = 100;
            this.wieldingStyle = WieldingStyle.OneHanded;
            this.Layer = Layer.OneHanded;
		}

		public BastardSword( Serial serial ) : base( serial )
		{
		}

        public override void OnDoubleClick(Mobile from)
        {
            this.ChangeWieldingStyle();
            from.SendMessage("Wielding bastard swords in " + this.HandsUsedToWield() + ".");
        }

        private string HandsUsedToWield()
        {
            if (wieldingStyle == WieldingStyle.OneHanded)
                return "one hand";
            else
                return "both hands";
        }

        private void ChangeWieldingStyle()
        {
            if (this.wieldingStyle == WieldingStyle.OneHanded)
            {
                this.wieldingStyle = WieldingStyle.TwoHanded;
                this.Layer = Layer.TwoHanded;
            }
            else
            {
                this.wieldingStyle = WieldingStyle.OneHanded;
                this.Layer = Layer.OneHanded;
            }
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
