using System;
using Server.Network;
using Server.Items;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
	public class HandAndAHalfSword : BaseSword
	{
        private enum WieldingStyle
        {
            OneHanded,
            TwoHanded
        }

        private WieldingStyle wieldingStyle;

		public override string NameType{ get{ return "hand-and-a-half sword"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }
		public override bool BastardWeapon{ get { return true; } }
		
		public override int SheathedMaleWaistID{ get{ return 15218; } }
		public override int SheathedFemaleWaistID{ get{ return 15219; } }

		public override int AosStrengthReq{ get{ return 55; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.2; } }
		public override double RangedPercentage{ get{ return 0; } }
        public override int AosMinDamage
        {
            get
            {
                if (this.wieldingStyle == WieldingStyle.OneHanded)
                    return 16;
                else
                    return 18;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                if (this.wieldingStyle == WieldingStyle.OneHanded)
                    return 16;
                else
                    return 18;
            }
        }
		public override double AosSpeed{ get{ return 4; } }

		public override int OldStrengthReq{ get{ return 25; } }
		public override int OldMinDamage{ get{ return 5; } }
		public override int OldMaxDamage{ get{ return 33; } }
		public override int OldSpeed{ get{ return 35; } }

		public override int DefHitSound{ get{ return 0x237; } }
		public override int DefMissSound{ get{ return 0x23A; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }

		[Constructable]
		public HandAndAHalfSword() : base( 0x13B9 )
		{
			Weight = 8.0;
			Name = "hand-and-a-half sword";
			AosElementDamages.Slashing = 90;
			AosElementDamages.Blunt = 10;
            this.wieldingStyle = WieldingStyle.OneHanded;
            this.Layer = Layer.OneHanded;
		}

		public HandAndAHalfSword( Serial serial ) : base( serial )
		{
		}

        public override void OnDoubleClick(Mobile from)
        {           
            this.ChangeWieldingStyle();
            from.SendMessage("Wielding hand-and-a-half swords in " + this.HandsUsedToWield() + ".");
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
