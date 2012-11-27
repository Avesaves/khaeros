using System;
using Server.Mobiles;

namespace Server.Items
{
	public class GuildFlag : BaseClothing
	{
		private CustomGuildStone m_Guild;
		private int m_Charges;
		private DateTime m_ExpirationDate;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public CustomGuildStone Guild{ get{ return m_Guild; } set{ m_Guild = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Charges{ get{ return m_Charges; } set{ m_Charges = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime ExpirationDate{ get{ return m_ExpirationDate; } set{ m_ExpirationDate = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan ExpiresOn
		{
			get
			{
				if( DateTime.Compare( DateTime.Now, ExpirationDate ) > 0 )
					return TimeSpan.MinValue;
				
				return (ExpirationDate - DateTime.Now);
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get
			{
				if( DateTime.Compare( DateTime.Now, ExpirationDate ) > 0 )
					return false;
				
				if( RootParentEntity != null && RootParentEntity is PlayerMobile )
				{
					PlayerMobile pm = RootParentEntity as PlayerMobile;
					
					if( Guild != null && pm.CustomGuilds.ContainsKey(Guild) )
						return true;
				}
				
				return false;
			}
		}
		
		[Constructable]
		public GuildFlag() : base( 15290, Layer.TwoHanded )
		{
			Weight = 5.0;
			Name = "A blank guild flag";
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !(from is PlayerMobile) )
				return;
			
			PlayerMobile pm = from as PlayerMobile;
			
			if( Charges < 1 )
			{
				from.SendMessage( "You will need to recharge this item first. Recharging costs 500 copper." );
				from.SendMessage( "Please target your guildstone if you wish to recharge this item." );
				from.Target = new Misc.GuildFlagTarget( this );
			}
			
			else if( DateTime.Compare( ExpirationDate, DateTime.Now ) > 0 )
				from.SendMessage( "That flag is already active." );
			
			else if( Guild != null && !pm.CustomGuilds.ContainsKey(Guild) )
				from.SendMessage( "You are not a member of that flag's guild." );
			
			else
			{
				from.SendMessage( "This flag will be active for 5 minutes. Make sure to equip or get a guild member to do so." );
				Charges--;
				ExpirationDate = DateTime.Now + TimeSpan.FromMinutes( 5 );
			}
		}
		
		public void TryToRecharge( Mobile from, CustomGuildStone target )
		{
			if( from is PlayerMobile && target is CustomGuildStone )
			{
				PlayerMobile pm = from as PlayerMobile;
				CustomGuildStone guild = target as CustomGuildStone;
				
				if( Guild != null && pm.CustomGuilds.ContainsKey(Guild) )
				{
					if( !CustomGuildStone.IsGuildOfficer(pm, guild) )
						return;
					
					if( !guild.OfficialGuild && (guild.Treasury == null || guild.Treasury.Deleted || !guild.Treasury.ConsumeTotal(typeof(Copper), 500)) )
        				pm.SendMessage( "Your guild's treasury is either unexistant or does not have 500 copper coins in it." );
					
					else
					{
						Charges = 5;
						ExpirationDate = DateTime.MinValue;
						Name = "A standard of " + (String.IsNullOrEmpty(guild.Name) == true ? "an unnamed guild" : guild.Name) + " guild";
						Guild = guild;
						
						if( guild.ClothingHue >= 0 )
							Hue = guild.ClothingHue;
					}
				}
			}
			
			return;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			
			list.Add( 1060658, "Charges Left\t{0}", Charges );
			
			if( DateTime.Compare( ExpirationDate, DateTime.Now ) > 0 )
			{
				int seconds = ExpiresOn.Seconds;
				int minutes = ExpiresOn.Minutes;
			
				string timeLeft = "" + (minutes > 0 ? (minutes + "m ") : "") + (seconds > 0 ? (seconds + "s") : "" );
				
				list.Add( 1060660, "{0}\t{1}", "Active for", timeLeft ); // ~1_val~: ~2_val~
			}
		}
		
		public GuildFlag( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
			writer.Write( (CustomGuildStone) m_Guild );
			writer.Write( (int) m_Charges );
			writer.Write( (DateTime) m_ExpirationDate );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Guild = (CustomGuildStone)reader.ReadItem();
			m_Charges = reader.ReadInt();
			m_ExpirationDate = reader.ReadDateTime();
		}
	}
}
