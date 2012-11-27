using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BaseSpear : BaseMeleeWeapon
	{
		public override int DefHitSound{ get{ return 0x23C; } }
		public override int DefMissSound{ get{ return 0x238; } }

		public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce2H; } }

        public override bool Throwable { get { return true; } }
        private int m_FeatLevel;

		public BaseSpear( int itemID ) : base( itemID )
		{
		}

		public BaseSpear( Serial serial ) : base( serial )
		{
		}

        public override bool OnEquip( Mobile from )
        {
            if( from is PlayerMobile )
            {
                PlayerMobile pm = from as PlayerMobile;
                if (pm != null)
                {
                    ArmourBackpack backpack = pm.FindItemOnLayer(Layer.Backpack) as ArmourBackpack;
                    if (backpack != null)
                    {
                        m_FeatLevel = pm.Feats.GetFeatLevel(FeatList.PolearmsMastery) * 10;
                        backpack.Attributes.WeaponSpeed += m_FeatLevel;
                        backpack.Attributes.AttackChance += m_FeatLevel;
                        backpack.OnEquip(pm);
                    }
                }
            }

            return base.OnEquip( from );
        }

        public override void OnRemoved( object parent )
        {
            if( parent is PlayerMobile )
            {
                PlayerMobile pm = parent as PlayerMobile;
                if (pm != null)
                {
                    ArmourBackpack backpack = pm.FindItemOnLayer(Layer.Backpack) as ArmourBackpack;
                    if (backpack != null)
                    {
                        backpack.Attributes.WeaponSpeed -= m_FeatLevel;
                        backpack.Attributes.AttackChance -= m_FeatLevel;
                        backpack.OnEquip(pm);
                        m_FeatLevel = 0;
                    }
                }
            }

            base.OnRemoved( parent );
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

            writer.Write( (int) m_FeatLevel );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            switch( version )
			{
                case 1:
				{
                    m_FeatLevel = reader.ReadInt();
					break;
				}
            }
		}

		public override void OnHit( Mobile attacker, Mobile defender )
		{
			base.OnHit( attacker, defender );

			if ( !Core.AOS && Layer == Layer.TwoHanded && (attacker.Skills[SkillName.Anatomy].Value / 400.0) >= Utility.RandomDouble() )
			{
				defender.SendMessage( "You receive a paralyzing blow!" ); // Is this not localized?
				defender.Freeze( TimeSpan.FromSeconds( 2.0 ) );

				attacker.SendMessage( "You deliver a paralyzing blow!" ); // Is this not localized?
				attacker.PlaySound( 0x11C );
			}

			if ( !Core.AOS && Poison != null && PoisonCharges > 0 )
			{
				--PoisonCharges;

				if ( Utility.RandomDouble() >= 0.5 ) // 50% chance to poison
					defender.ApplyPoison( attacker, Poison );
			}
		}
	}
}
