using System;
using Server.Mobiles;

namespace Server.Items
{
	public class StableTicket : Item
	{
		private Mobile m_StabledPet;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile StabledPet
		{
			get{ return m_StabledPet; }
			set{ m_StabledPet = value; }
		}
		
		[Constructable]
		public StableTicket() : base( 0xE17 )
		{
			Weight = 0.1;
            Name = "Stable Ticket";
            Hue = 2401;
		}

		public StableTicket( Serial serial ) : base( serial )
		{
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			
			if ( StabledPet != null && StabledPet is Mercenary && StabledPet.Map == Map.Internal && StabledPet.Name != null && StabledPet.Name.Length > 0 )
				list.Add( 1060847, "{0}\t{1}", "  " + "Mercenary's Name: " + m_StabledPet.Name, " " ); // ~1_val~ ~2_val~
			
			if ( StabledPet != null && StabledPet.Map == Map.Internal && StabledPet.Name != null && StabledPet.Name.Length > 0 )
				list.Add( 1060847, "{0}\t{1}", "  " + "Animal's Name: " + m_StabledPet.Name, " " ); // ~1_val~ ~2_val~
			
			else
				list.Add( 1060847, "{0}\t{1}", "  " + "Expired", " " ); // ~1_val~ ~2_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
			writer.Write( (Mobile) m_StabledPet );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_StabledPet = reader.ReadMobile();
		}
	}
}
