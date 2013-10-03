using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;


namespace Server.Mobiles
{
	[CorpseName( "a skeletal corpse" )]
	public class LavaHurler : BaseCreature, IUndead, IEnraged
	{
		[Constructable]
		public LavaHurler() : base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lava hurler";
			Body = 50;
			BaseSoundID = 0x48D;
			Hue = 2618;

			SetStr( 56, 80 );
			SetDex( 56, 65 );
			SetInt( 5 );

			SetHits( 134, 148 );

			SetDamage( 8, 10 );

			SetDamageType( ResistanceType.Fire, 100 );

			SetResistance( ResistanceType.Blunt, 15, 20 );
			SetResistance( ResistanceType.Piercing, 50, 60 );
			SetResistance( ResistanceType.Slashing, 50, 60 );
			SetResistance( ResistanceType.Fire, 100 );
			SetResistance( ResistanceType.Cold, 25, 40 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 5, 15 );

			SetSkill( SkillName.MagicResist, 45.1, 60.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.UnarmedFighting, 45.1, 55.0 );

			Fame = 3650;
			Karma = -3650;

			VirtualArmor = 40;
			
			PackItem( new Bone( 3 ) );
			EquipItem( new LightSource() );
		}

		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 95 )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );
					
					if( m != null && this.InRange( m, 12 ) && this.CanSee( m ) )
					{
						this.MovingParticles( m, 0x36D4, 7, 0, false, true, 0, 0, 0, 0x36CB, 0x1DE, 0x100 ); 
	//MovingParticleEffect(  IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, EffectLayer layer, int unknown )
						this.Emote( "*flings lava at " + m.Name + "*" );
						
						if( this.Combatant is PlayerMobile && ((PlayerMobile)this.Combatant).Evaded() )
			            	return;
						
						AOS.Damage( m, this, Utility.RandomMinMax( 15, 18 ), false, 0, 100, 0, 0, 0, 0, 0, 0, false );
					}
				}
			}
		}
		
		public override bool BleedImmune{ get{ return true; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor, 1 );
		}

		public LavaHurler( Serial serial ) : base( serial )
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
