using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a storm Giants corpse" )]
	public class StormGiant : BaseCreature, ILargePredator, IGiant, IEnraged, IPeacefulPredator
	{
		public override int Height{ get{ return 40; } }
		[Constructable]
		public StormGiant() : base( AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a storm Giant";
			BodyValue = 36;
			BaseSoundID = 609;
			Hue = 2582;

			SetStr( 426, 465 );
			SetDex( 46, 75 );
			SetInt( 11, 25 );

			SetHits( 942, 961 );

			SetDamage( 35, 40 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 35, 50 );
			SetResistance( ResistanceType.Slashing, 35, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Invocation, 85.1, 100.0 );
			SetSkill( SkillName.Magery, 85.1, 100.0 );
			SetSkill( SkillName.MagicResist, 70.0 );
			SetSkill( SkillName.Tactics, 110.1, 120.0 );
			SetSkill( SkillName.UnarmedFighting, 110.1, 120.0 );
			SetSkill( SkillName.Macing, 110.1, 120.0 );
			
			this.RangeFight = 3;
            MeleeAttackType = MeleeAttackType.FrontalAOE;

			Fame = 25000;
			Karma = 25000;

			VirtualArmor = 52;
            PackItem( new RewardToken( 2 ) );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new GiantsSkull() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

		public override int Meat{ get{ return 50; } }
		public override int Bones{ get{ return 40; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Beast; } }

		public StormGiant( Serial serial ) : base( serial )
		{
		}
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 95 && this.CanUseSpecial )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );
					
					if( m != null && this.InRange( m, 12 ) && this.CanSee( m ) )
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
						
						Spells.SpellHelper.Damage( TimeSpan.FromTicks( 1 ), m, this, Utility.RandomMinMax( 40, 45 ) );
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
