using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a rust monster corpse" )] // TODO: Corpse name?
	public class RustMonster : BaseCreature, IMediumPredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public RustMonster() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a rust monster";
			BodyValue = 242;
			BaseSoundID = 0;
			Hue = 2964;

			SetStr( 141, 150 );
			SetDex( 41, 50 );
			SetInt( 10 );

			SetMana( 0 );

			SetDamage( 10, 15 );
			
			SetHits( 112, 153 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 10 );
			SetResistance( ResistanceType.Piercing, 10, 20 );
			SetResistance( ResistanceType.Slashing, 10, 20 );
			SetResistance( ResistanceType.Poison, 100 );

			SetSkill( SkillName.Tactics, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 70.1, 80.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 24;

			PackItem( new RustyDebris() );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new RustGland() );
		}
		
		public override int GetAngerSound()
		{
			return 0x4F3;
		}

		public override int GetIdleSound()
		{
			return 0x4F2;
		}

		public override int GetAttackSound()
		{
			return 0x4F1;
		}

		public override int GetHurtSound()
		{
			return 0x4F4;
		}

		public override int GetDeathSound()
		{
			return 0x4F0;
		}
		
		public override int Meat{ get{ return 6; } }

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			
			DegradeWeapon( attacker );
		}
		
		public override void OnGaveMeleeAttack( Mobile defender )
		{
			DegradeArmour( defender );
			
			base.OnGaveMeleeAttack( defender );
		}
		
		public virtual void DegradeWeapon( Mobile defender )
		{
			BaseWeapon sundered = defender.Weapon as BaseWeapon;

			if( !( sundered is Fists ) && !( sundered is BaseRanged ) )
            {
            	sundered.HitPoints -= Utility.Random( 5 );

                if( sundered.HitPoints < 0 )
                {
                    sundered.MaxHitPoints += sundered.HitPoints;
                    sundered.HitPoints = 0;

                    if( sundered.MaxHitPoints < 0 )
                    {
                        sundered.Delete();
                        defender.Emote( "*got {0} weapon destroyed by {1}*", defender.Female == true ? "her" : "his", this.Name );
                    }
                }

                defender.Emote( "*got {0} weapon damaged by {1}*", defender.Female == true ? "her" : "his", this.Name );
            }
		}
		
		public virtual void DegradeArmour( Mobile defender )
		{
			int chance = Utility.RandomMinMax( 1, 7 );
			string sundname = "";
			
			BaseArmor sundered = null;
			Layer layer = Layer.FirstValid;
			
			switch( chance )
			{
				case 1: layer = Layer.InnerTorso; sundname = "armour"; break;
				case 2: layer = Layer.InnerLegs; sundname = "leggings"; break;
				case 3: layer = Layer.TwoHanded; sundname = "shield"; break;
				case 4: layer = Layer.Neck; sundname = "gorget"; break;
				case 5: layer = Layer.Gloves; sundname = "gauntlets"; break;
				case 6: layer = Layer.Helm; sundname = "helm"; break;
				case 7: layer = Layer.Arms; sundname = "arm pads"; break;
			}
			
			if( defender.FindItemOnLayer( layer ) != null && defender.FindItemOnLayer( layer ) is BaseArmor )
        	{
        		sundered = defender.FindItemOnLayer( layer ) as BaseArmor;
        	}

            if( sundered != null )
            {
            	sundered.HitPoints -= Utility.Random( 5 );

                if( sundered.HitPoints < 0 )
                {
                    sundered.MaxHitPoints += sundered.HitPoints;
                    sundered.HitPoints = 0;

                    if( sundered.MaxHitPoints < 0 )
                    {
                        sundered.Delete();
                        defender.Emote( "*got {0} {1} destroyed by {2}*", defender.Female == true ? "her" : "his", sundname, this.Name );
                    }
                }

                defender.Emote( "*got {0} {1} damaged by {2}*", defender.Female == true ? "her" : "his", sundname, this.Name );
            }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public RustMonster( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
