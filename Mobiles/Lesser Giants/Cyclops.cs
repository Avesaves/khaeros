using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a cyclops corpse" )]
	public class Cyclops : BaseCreature, ILargePredator, IHasReach, IEnraged, IGiant
	{
		public override int Height{ get{ return 35; } }
		[Constructable]
		public Cyclops() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a cyclops";
			Body = 75;
			BaseSoundID = 604;

			SetStr( 336, 385 );
			SetDex( 26, 35 );
			SetInt( 35 );

			SetHits( 402, 431 );
			SetMana( 0 );

			SetDamage( 15, 20 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 25, 30 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 30.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.UnarmedFighting, 80.1, 90.0 );
			SetSkill( SkillName.Macing, 80.1, 90.0 );
			
			this.RangeFight = 3;

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 30;
		}
		
		public override int Meat{ get{ return 18; } }
		public override int Bones{ get{ return 18; } }
		public override int Hides{ get{ return 9; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor);
		}

		public Cyclops( Serial serial ) : base( serial )
		{
		}
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 90 && this.CanUseSpecial )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );
					
					if( m != null && this.InRange( m, 9 ) && this.CanSee( m ) )
					{
						GiantRock rock = new GiantRock();
						rock.ItemID += Utility.RandomMinMax( 0, 3 );
						rock.MoveToWorld( m.Location );
		                rock.Map = m.Map;
						this.PlaySound( 66 );
						this.Emote( "*flings a large rock at " + m.Name + "*" );
						this.CanUseSpecial = false;
						
						if( this.Combatant is PlayerMobile && ((PlayerMobile)this.Combatant).Evaded() )
			            	return;
						
						Spells.SpellHelper.Damage( TimeSpan.FromTicks( 1 ), m, this, Utility.RandomMinMax( 15, 20 ) );
					}
				}
			}
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
