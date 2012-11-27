using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a chicken corpse" )]
	public class Chicken : BaseCreature, ISmallPrey
	{
		public override bool ParryDisabled{ get{ return true; } }
		private DateTime m_LastEgg;
		
		[CommandProperty( AccessLevel.GameMaster )]
        public DateTime LastEgg
        {
            get { return m_LastEgg; }
            set { m_LastEgg = value; }
        }
        
		[Constructable]
		public Chicken() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a chicken";
			Body = 0xD0;
			BaseSoundID = 0x6E;

			SetStr( 5 );
			SetDex( 15 );
			SetInt( 5 );

			SetHits( 1 );
			SetMana( 0 );

			SetDamage( 1 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 1, 5 );

			SetSkill( SkillName.MagicResist, 0.0 );
			SetSkill( SkillName.Tactics, 5.0 );
			SetSkill( SkillName.UnarmedFighting, 5.0 );

			Fame = 50;
			Karma = 0;

			VirtualArmor = 2;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 5.0;
		}

		public override int Meat{ get{ return 1; } }
		public override int Bones{ get{ return 1; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new RawChickenLeg( 2 ) );
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			int eggs = 0;
			
			if( !this.InRange( from, 1 ) )
		   	{
			   	from.SendMessage( "You are too far away to do that." );
			   	return;
		   	}
			
			if( from != null )
			{
				if ( DateTime.Compare( DateTime.Now, ( this.LastEgg + TimeSpan.FromHours( 1 ) ) ) > 0 )
				{
					eggs++;
					
					if ( DateTime.Compare( DateTime.Now, ( this.LastEgg + TimeSpan.FromHours( 2 ) ) ) > 0 )
					{
						eggs++;
						
						if ( DateTime.Compare( DateTime.Now, ( this.LastEgg + TimeSpan.FromHours( 3 ) ) ) > 0 )
						{
							eggs++;
						}
					}
					
					if ( from is PlayerMobile )
                    {
						PlayerMobile pm = from as PlayerMobile;
                        Container pack = pm.Backpack;
                        Eggs neweggs = new Eggs();
                        neweggs.Amount = eggs;
                        pack.DropItem( neweggs );
                    }

					this.LastEgg = DateTime.Now;
					from.SendMessage( "You collect some of the chicken's eggs." );
				}
				
				else
				{
					from.SendMessage( "It is too early to get any more eggs from this chicken." );
				}
			}
		}

		public override FoodType FavoriteFood{ get{ return FoodType.GrainsAndHay; } }

		public override int Feathers{ get{ return 5; } }

		public Chicken(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 1);
			
			writer.Write( (DateTime) m_LastEgg );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			
			if( version > 0 )
			{
				m_LastEgg = reader.ReadDateTime();
			}
		}
	}
}
