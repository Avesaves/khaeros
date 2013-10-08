using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
	[CorpseName( "a ghost corpse" )]
	public class BurningSpirit : BaseCreature, IUndead, IEnraged, IIncorporeal
	{
		[Constructable]
		public BurningSpirit() : base( AIType.AI_Archer, FightMode.Closest, 18, 1, 0.2, 0.4 )
		{
			Name = "a burning spirit";
			BodyValue = 146;
			Hue = 2618;

			SetStr( 190, 250 );
			SetDex( 75, 95 );
			SetInt( 95 );
			
			SetHits( 700, 750 );

			SetDamage( 15, 20 );

			Fame = 50000;
			Karma = -50000;

			VirtualArmor = 40;

			SetDamageType( ResistanceType.Fire, 100 );

			SetResistance( ResistanceType.Blunt, 65, 75 );
			SetResistance( ResistanceType.Piercing, 65, 75 );
			SetResistance( ResistanceType.Slashing, 65, 75 );
			SetResistance( ResistanceType.Fire, 90, 100 );
			SetResistance( ResistanceType.Cold, 60, 80 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 60, 80 );

			SetSkill( SkillName.UnarmedFighting, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.2, 110.0 );
			SetSkill( SkillName.MagicResist, 80.2, 90.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Invocation, 120.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			
			GiveFeat = "Dodge 3";
            GiveFeat = "EnhancedDodge 3";
            GiveFeat = "PureDodge 3";
            GiveFeat = "Evade 3";
			
			this.RangeFight = 2;
            MeleeAttackType = MeleeAttackType.FrontalAOE;
			
			PackItem( new Necroplasm( 3 ) );
			EquipItem( new LightSource() );
			PackItem( new RewardToken( 3 ) );
			
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
			AddLoot( LootPack.Rich, 1 );
		}
		
		public override bool HasBreath{ get{ return true; } }

		public override void OnGaveMeleeAttack(Mobile defender)
        {
            if ( Utility.Random(100) > 75 )
            {
            XmlAttach.AttachTo(defender, new XmlFreeze(Utility.RandomMinMax(2, 3)));
            defender.PlaySound(516);
            this.Emote("*Throws embers into " + defender.Name + "'s face, stunning " + (defender.Female == true ? "her" : "him") + "!*");
            }

        }
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 90 )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );
					
					if( m != null && this.InRange( m, 12 ) && this.CanSee( m ) )
					{
						this.MovingParticles( m, 0x36D4, 7, 0, false, true, 0, 0, 0, 0x36CB, 0x1DE, 0x100 ); 
	//MovingParticleEffect(  IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, EffectLayer layer, int unknown )
						this.Emote( "*Hurls a ball of fire at " + m.Name + "*" );
						
						if( this.Combatant is PlayerMobile && ((PlayerMobile)this.Combatant).Evaded() )
			            	return;
						
						AOS.Damage( m, this, Utility.RandomMinMax( 14, 17 ), false, 0, 100, 0, 0, 0, 0, 0, 0, false );
					}
				}
			}
		}

		public BurningSpirit( Serial serial ) : base( serial )
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
