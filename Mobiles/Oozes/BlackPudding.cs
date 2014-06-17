using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Black Pudding corpse" )] // TODO: Corpse name?
	public class BlackPudding : BaseCreature, IMediumPredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public BlackPudding() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Black Pudding";
			BodyValue = 41;
			BaseSoundID = 898;
			Hue = 2989;

			SetStr( 141, 150 );
			SetDex( 81, 90 );
			SetInt( 10 );
			
			SetHits( 352, 383 );

			SetMana( 0 );

			SetDamage( 20, 25 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 10 );
			SetResistance( ResistanceType.Piercing, 10, 20 );
			SetResistance( ResistanceType.Slashing, 10, 20 );
			SetResistance( ResistanceType.Poison, 100 );

			SetSkill( SkillName.Tactics, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 70.1, 80.0 );

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 44;
			PackItem( new BlackSlime( 1 ) );
		}
        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new GlowingGoop());

        }
		public void SpawnBlackPuddingSpawns( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newBlackPuddingSpawns = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < newBlackPuddingSpawns; ++i )
			{
				BlackPuddingSpawn gelatinousBlobSpawn = new BlackPuddingSpawn();

				gelatinousBlobSpawn.Team = this.Team;
				gelatinousBlobSpawn.FightMode = FightMode.Closest;

				bool validLocation = false;
				Point3D loc = this.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				gelatinousBlobSpawn.MoveToWorld( loc, map );
				gelatinousBlobSpawn.Combatant = target;
				gelatinousBlobSpawn.VanishTime = DateTime.Now + TimeSpan.FromHours( 1 );
				gelatinousBlobSpawn.VanishEmote = "*decays into a puddle of motionless slime*";
				PlaySound( 898 );
			}
			
			this.Emote( "*some of its severed pieces start to move about and fight to defend it*" );
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( 0.3 >= Utility.RandomDouble() )
				SpawnBlackPuddingSpawns( caster );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			
			DegradeWeapon( attacker );

			if ( 0.3 >= Utility.RandomDouble() && this.CanUseSpecial )
			{
				this.CanUseSpecial = false;
				SpawnBlackPuddingSpawns( attacker );
			}
		}
		
		public override void OnGaveMeleeAttack( Mobile defender )
		{
			DegradeArmour( defender );
			
			base.OnGaveMeleeAttack( defender );
		}
		
		public virtual void DegradeWeapon( Mobile defender )
		{
			BaseWeapon sundered = defender.Weapon as BaseWeapon;

            if( !( sundered is Fists ) )
            {
            	sundered.HitPoints -= Utility.Random( 3 );

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
            	sundered.HitPoints -= Utility.Random( 3 );

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

		public BlackPudding( Serial serial ) : base( serial )
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
