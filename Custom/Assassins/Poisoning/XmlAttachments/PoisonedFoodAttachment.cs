using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Alchemy;

namespace Server.Engines.XmlSpawner2 
{
	public class PoisonedFoodAttachment : XmlAttachment 
	{
		private Dictionary<PoisonEffectEnum, int> m_Effects;
		private int m_PoisonDuration;
		private int m_PoisonActingSpeed;
		private Mobile m_Poisoner;

		public Dictionary<PoisonEffectEnum, int> Effects { get { return m_Effects; } set { m_Effects = value; } }
		public int PoisonDuration { get { return m_PoisonDuration; } set { m_PoisonDuration = value; } }
		public int PoisonActingSpeed { get { return m_PoisonActingSpeed; } set { m_PoisonActingSpeed = value; } }
		public Mobile Poisoner { get { return m_Poisoner; } set { m_Poisoner = value; } }

		public PoisonedFoodAttachment ( ASerial serial ) : base( serial ) 
		{ 
		}

		public PoisonedFoodAttachment( Dictionary<PoisonEffectEnum, int> effects, int poisonDuration, int poisonActingSpeed, Mobile poisoner )
		{
			m_Effects = effects;
			m_PoisonDuration = poisonDuration;
			m_PoisonActingSpeed = poisonActingSpeed;
			m_Poisoner = poisoner;
		}
		
		public override void OnAttach() 
		{
			if ( !(AttachedTo is Item) )
				this.Delete();
			base.OnAttach();

			(AttachedTo as Item).InvalidateProperties();
		}

		public override void Serialize ( GenericWriter writer ) 
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		
			writer.Write( m_Poisoner );
			writer.Write( (int) m_PoisonDuration );
			writer.Write( (int) m_PoisonActingSpeed );
			
			writer.Write( (int) m_Effects.Count );

			foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in m_Effects ) 
			{
				writer.Write( (int)kvp.Key );
				writer.Write( (int)kvp.Value );
			}
		}

		public override void Deserialize ( GenericReader reader ) 
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			m_Poisoner = reader.ReadMobile();
			m_PoisonDuration = reader.ReadInt();
			m_PoisonActingSpeed = reader.ReadInt();
			
			int c = reader.ReadInt();
			m_Effects = new Dictionary<PoisonEffectEnum, int>();
			for ( int i = 0; i < c; i++ )
				m_Effects.Add( (PoisonEffectEnum) reader.ReadInt(), reader.ReadInt() );
		}

		public void OnConsumed( Mobile consumer ) // ate/drank the stuff
		{
			PoisonEffect.Poison( consumer, m_Poisoner, m_Effects, m_PoisonDuration, m_PoisonActingSpeed, true );
		}
		
		public void OnOtherwiseInjected( Mobile consumer )
		{
			PoisonEffect.Poison( consumer, m_Poisoner, m_Effects, m_PoisonDuration, m_PoisonActingSpeed, false );
		}
	}
}
