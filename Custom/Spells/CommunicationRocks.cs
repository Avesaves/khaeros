using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{

	public class BroadcastRock : Item
	{
		private ArrayList m_Receivers;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get{ return this.ItemID == 0x3678; }
			set
			{
				this.ItemID = value ? 0x3678 : 0x3678;
				InvalidateProperties();
			}
		}

		public ArrayList Receivers
		{
			get{ return m_Receivers; }
		}

		[Constructable]
		public BroadcastRock() : base( 0x3678 )
		{
			Hue = 2990;
			Name = "A peculiar rock";
			Light = LightType.Circle150;
			m_Receivers = new ArrayList();
		}

		public BroadcastRock( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( Receivers.Count > 0 )
				list.Add( 1060746, Receivers.Count.ToString() ); // links: ~1_val~
		}

		public override void OnSingleClick(Mobile from)
		{
			base.OnSingleClick( from );

			if ( Receivers.Count > 0 )
				LabelTo( from, 1060746, Receivers.Count.ToString() ); // links: ~1_val~
		}

		public override bool HandlesOnSpeech
		{
			get{ return Active && Receivers.Count > 0 && ( RootParent == null || RootParent is Mobile ); }
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( !Active || Receivers.Count == 0 || ( RootParent != null && !(RootParent is Mobile) ) )
				return;

			if ( e.Type == MessageType.Emote )
				return;

			Mobile from = e.Mobile;
			string speech = e.Speech;

			foreach ( ReceiverRock receiver in new ArrayList( Receivers ) )
			{
				if ( receiver.Deleted )
				{
					Receivers.Remove( receiver );
				}
				else
				{
					receiver.TransmitMessage( from, speech );
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( GetWorldLocation(), 2 ) )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				return;
			}

			from.Target = new InternalTarget( this );
		}

		private class InternalTarget : Target
		{
			private BroadcastRock m_Crystal;

			public InternalTarget( BroadcastRock crystal ) : base( 2, false, TargetFlags.None )
			{
				m_Crystal = crystal;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( !m_Crystal.IsAccessibleTo( from ) )
					return;

				if ( from.Map != m_Crystal.Map || !from.InRange( m_Crystal.GetWorldLocation(), 2 ) )
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
					return;
				}

				else if ( targeted is ReceiverRock )
				{
					ReceiverRock receiver = (ReceiverRock) targeted;

					if ( m_Crystal.Receivers.Count >= 10 )
					{
						from.SendLocalizedMessage( 1010042 ); // This broadcast crystal is already linked to 10 receivers.
					}
					else if ( receiver.Sender == m_Crystal )
					{
						from.SendLocalizedMessage( 500674 ); // This crystal is already linked with that crystal.
					}
					else if ( receiver.Sender != null )
					{
						from.SendLocalizedMessage( 1010043 ); // That receiver crystal is already linked to another broadcast crystal.
					}
					else
					{
						receiver.Sender = m_Crystal;
						from.SendLocalizedMessage( 500675 ); // That crystal has been linked to this crystal.
					}
				}
				else if ( targeted == from )
				{
					foreach ( ReceiverRock receiver in new ArrayList( m_Crystal.Receivers ) )
					{
						receiver.Sender = null;
					}

					from.SendLocalizedMessage( 1010046 ); // You unlink the broadcast crystal from all of its receivers.
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version

			writer.WriteItemList( m_Receivers );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			m_Receivers = reader.ReadItemList();
		}
	}

	public class ReceiverRock : Item
	{
		private BroadcastRock m_Sender;

		[CommandProperty( AccessLevel.GameMaster )]
		public BroadcastRock Sender
		{
			get{ return m_Sender; }
			set
			{
				if ( m_Sender != null )
				{
					m_Sender.Receivers.Remove( this );
					m_Sender.InvalidateProperties();
				}

				m_Sender = value;

				if ( value != null )
				{
					value.Receivers.Add( this );
					value.InvalidateProperties();
				}
			}
		}

		[Constructable]
		public ReceiverRock() : base( 0x136C )
		{
			Weight = 0.0;
            Name = "a glowing rock";
			Hue = 2996;
			
		}
		
		public ReceiverRock( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );
		}

		public void TransmitMessage( Mobile from, string message )
		{

			string text = String.Format( "{1}", from.Name, message );

			if ( this.RootParent is Mobile )
			{
				((Mobile)this.RootParent).SendMessage( 37, "" + text );
			}
			else if ( this.RootParent is Item )
			{
				((Item)this.RootParent).PublicOverheadMessage( MessageType.Regular, 37, false, "*glows*"  );
			}
			else
			{
				PublicOverheadMessage( MessageType.Regular, 37, false, "*glows*" );
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
				return;
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version

			writer.Write( (Item) m_Sender );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			m_Sender = (BroadcastRock) reader.ReadItem();
		}
	}
}
