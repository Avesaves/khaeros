using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Jelly Ooze corpse" )] // TODO: Corpse name?
	public class JellyOoze : BaseCreature, IMediumPredator
	{
		public override bool ParryDisabled{ get{ return true; } }
		[Constructable]
		public JellyOoze() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Jelly Ooze";
			BodyValue = 260;
			BaseSoundID = 898;

			SetStr( 81, 90 );
			SetDex( 81, 90 );
			SetInt( 10 );
			
			SetHits( 180, 200 );

			SetMana( 0 );

			SetDamage( 16, 22 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 10 );
			SetResistance( ResistanceType.Piercing, 10, 20 );
			SetResistance( ResistanceType.Slashing, 10, 20 );
			SetResistance( ResistanceType.Poison, 100 );

			SetSkill( SkillName.Tactics, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 70.1, 80.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 44;
			PackItem( new BlueSlime( 1 ) );
		}
		
		public void SpawnJellyOozeSpawns( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			int newJellyOozeSpawns = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < newJellyOozeSpawns; ++i )
			{
				JellyOozeSpawn gelatinousBlobSpawn = new JellyOozeSpawn();

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
        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new GlowingGoop());

        }
		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( 0.3 >= Utility.RandomDouble() )
				SpawnJellyOozeSpawns( caster );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.3 >= Utility.RandomDouble() && this.CanUseSpecial )
			{
				this.CanUseSpecial = false;
				SpawnJellyOozeSpawns( attacker );
			}
		}

		public JellyOoze( Serial serial ) : base( serial )
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
