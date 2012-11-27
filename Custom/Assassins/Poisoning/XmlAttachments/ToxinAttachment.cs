using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Alchemy;

namespace Server.Engines.XmlSpawner2 
{
	public class ToxinAttachment : OilAttachment 
	{
		private Dictionary<PoisonEffectEnum, int> m_Effects;
		private int m_PoisonDuration;
		private int m_PoisonActingSpeed;

		public int PoisonDuration { get { return m_PoisonDuration; } set { m_PoisonDuration = value; } }
		public int PoisonActingSpeed { get { return m_PoisonActingSpeed; } set { m_PoisonActingSpeed = value; } }

		public ToxinAttachment ( ASerial serial ) : base( serial ) 
		{ 
		}

		public ToxinAttachment( Dictionary<PoisonEffectEnum, int> effects, int poisonDuration, int poisonActingSpeed, int duration, int corrosivity ) : base ( null, duration, corrosivity )
		{
			m_Effects = effects;
			m_PoisonDuration = poisonDuration;
			m_PoisonActingSpeed = poisonActingSpeed;
		}

		public override void Serialize ( GenericWriter writer ) 
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		
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
			
			m_PoisonDuration = reader.ReadInt();
			m_PoisonActingSpeed = reader.ReadInt();
			
			int c = reader.ReadInt();
			m_Effects = new Dictionary<PoisonEffectEnum, int>();
			for ( int i = 0; i < c; i++ )
				m_Effects.Add( (PoisonEffectEnum) reader.ReadInt(), reader.ReadInt() );
		}

		public override void OnWeaponHit( Mobile defender, Mobile attacker ) // the poisoned weapon wielded by attacker struck defender -> apply effects
		{
			LowerDuration();
			PoisonEffect.Poison( defender, attacker, m_Effects, m_PoisonDuration, m_PoisonActingSpeed, false );
		}
	}
}
