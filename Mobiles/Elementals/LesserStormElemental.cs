using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Lesser energy elemental corpse" )]
	public class LesserEnergyElemental : BaseCreature, IElemental, IEnraged, IIncorporeal
	{
		public override double DispelDifficulty{ get{ return 117.5; } }
		public override double DispelFocus{ get{ return 45.0; } }
		
		public override bool DeleteCorpseOnDeath
		{
			get{ return true; }
		}

		[Constructable]
		public LesserEnergyElemental () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lesser energy elemental";
			BodyValue = 196;
			Hue = 2967;
			BaseSoundID = 655;

			SetStr( 26, 155 );
			SetDex( 66, 85 );
			SetInt( 35 );

			SetHits( 176, 193 );

			SetDamage( 8, 12 );

			SetDamageType( ResistanceType.Energy, 100 );

			SetResistance( ResistanceType.Blunt, 35, 45 );
			SetResistance( ResistanceType.Piercing, 30, 40 );
			SetResistance( ResistanceType.Slashing, 30, 40 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.Invocation, 60.1, 75.0 );
			SetSkill( SkillName.Magery, 60.1, 75.0 );
			SetSkill( SkillName.MagicResist, 60.1, 75.0 );
			SetSkill( SkillName.Tactics, 60.1, 80.0 );
			SetSkill( SkillName.UnarmedFighting, 60.1, 80.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 30;
			ControlSlots = 2;
		}

		public override bool BleedImmune{ get{ return true; } }
		
		public override void OnAfterMove( Point3D oldLocation )
		{
			if( Utility.Random( 100 ) > 85 && this.CanUseSpecial )
			{
				if( this.Combatant != null )
				{
					Mobile m = World.FindMobile( this.Combatant.Serial );
					
					if( m != null && this.InRange( m, 6 ) && this.CanSee( m ) )
					{
						this.MovingEffect( m, 14265, 12, 1, false, false );
						this.PlaySound( 552 );
						this.CanUseSpecial = false;
						
						if( this.Combatant is PlayerMobile && ((PlayerMobile)this.Combatant).Evaded() )
			            	return;
						
						Spells.SpellHelper.Damage( TimeSpan.FromTicks( 1 ), m, this, Utility.RandomMinMax( 12, 14 ) );
					}
				}
			}
		}
        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new ElementalGoop());
        }
		public LesserEnergyElemental( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 263 )
				BaseSoundID = 655;
		}
	}
}
