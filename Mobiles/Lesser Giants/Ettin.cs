using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ettins corpse" )]
	public class Ettin : BaseCreature, ILargePredator, IHasReach, IEnraged, IGiant
	{
		public override int Height{ get{ return 35; } }
		
		[Constructable]
		public Ettin() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ettin";
			Body = 18;
			BaseSoundID = 367;

			SetStr( 136, 165 );
			SetDex( 16, 25 );
			SetInt( 35 );

			SetHits( 352, 369 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Blunt, 100 );

			SetResistance( ResistanceType.Blunt, 25, 30 );
			SetResistance( ResistanceType.Piercing, 20, 30 );
			SetResistance( ResistanceType.Slashing, 20, 30 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 15, 25 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.MagicResist, 20.0 );
			SetSkill( SkillName.Tactics, 70.1, 80.0 );
			SetSkill( SkillName.UnarmedFighting, 70.1, 80.0 );
			SetSkill( SkillName.Macing, 70.1, 80.0 );
			
			this.RangeFight = 3;

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 28;
		}
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 95 && this.CanUseSpecial )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );
					
					if( m != null && this.InRange( m, 8 ) && this.CanSee( m ) )
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
						
						Spells.SpellHelper.Damage( TimeSpan.FromTicks( 1 ), m, this, Utility.RandomMinMax( 10, 15 ) );
					}
				}
			}
		}

		public override int Meat{ get{ return 16; } }
		public override int Bones{ get{ return 16; } }
		public override int Hides{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Thick; } }
		
		public override bool CanRummageCorpses{ get{ return true; } }

		public Ettin( Serial serial ) : base( serial )
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
