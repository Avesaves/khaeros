using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a water elemental corpse" )]
	public class WaterElemental : BaseCreature, IElemental, IEnraged, IIncorporeal
	{
		public override double DispelDifficulty{ get{ return 117.5; } }
		public override double DispelFocus{ get{ return 45.0; } }

		[Constructable]
		public WaterElemental () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a water elemental";
			BodyValue = 799;
			BaseSoundID = 278;

			SetStr( 126, 255 );
			SetDex( 66, 85 );
			SetInt( 35 );

			SetHits( 276, 293 );

			SetDamage( 15, 18 );

			SetDamageType( ResistanceType.Blunt, 80 );
			SetDamageType( ResistanceType.Cold, 20 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 10, 25 );
			SetResistance( ResistanceType.Cold, 10, 25 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.Invocation, 60.1, 75.0 );
			SetSkill( SkillName.Magery, 60.1, 75.0 );
			SetSkill( SkillName.MagicResist, 100.1, 115.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.UnarmedFighting, 50.1, 70.0 );

			Fame = 6500;
			Karma = -6500;

			VirtualArmor = 50;
			ControlSlots = 3;
			CanSwim = true;
		}

		public override bool BleedImmune{ get{ return true; } }
		
		public override void OnThink()
		{
			if( this.Combatant != null && this.Combatant.Alive && this.Combatant.Map == this.Map && 
			   this.InRange( this.Combatant, 10 ) && Utility.RandomMinMax( 1, 100 ) > 95 && 
			  this.GetDistanceToSqrt( this.Combatant ) > 1 )
			{
				Map map = this.Map;

				if ( map == null )
					return;
			
				bool validLocation = false;
				Point3D loc = this.Combatant.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, Combatant.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}
				
				this.Emote( "*shifts itself*" );
				this.Location = loc;
				this.Direction = this.GetDirectionTo( this.Combatant );
			}
			
			base.OnThink();
		}

        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new PureWater());
            bpc.DropItem(new ElementalGoop());
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 1 );
		}

		public WaterElemental( Serial serial ) : base( serial )
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
