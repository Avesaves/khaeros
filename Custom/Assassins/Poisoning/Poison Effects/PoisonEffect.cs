using System;
using System.Collections.Generic;
using Server;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public enum PoisonEffectEnum
	{
		StrengthDecrease = 1,
		DexterityDecrease,
		IntelligenceDecrease,
		HealthDecrease,
		StaminaDecrease,
		ManaDecrease,
		DamageMana,
		DamageStamina,
		DamageHealth
	}

	public abstract class PoisonEffect : CustomPotionEffect
	{
		private static Dictionary<PoisonEffectEnum, PoisonEffect> m_Registry = new Dictionary<PoisonEffectEnum, PoisonEffect>();
		
		public virtual void ApplyPoison( Mobile to, Mobile source, int intensity )
		{
		}
		
		public virtual void CureEffect( Mobile mobile )
		{
		}
		
		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( to == null )
				return;
			double poisonResistance = (to.PoisonResistance)/100.0;
			if ( poisonResistance > 1.0 )
				poisonResistance = 1.0;
			intensity -= (int)(poisonResistance*intensity);
			if ( intensity > 0 )
				ApplyPoison( to, source, intensity );
		}

		public static void Register( PoisonEffect instance, PoisonEffectEnum id )
		{
			if ( m_Registry.ContainsKey( id ) )
				Console.WriteLine( "WARNING: PoisonEffect found with duplicate ID (Name: {0}, ID: {1}) -- The effect was NOT registered.", instance.Name, id );
			else
				m_Registry[id] = instance;
		}

		public static PoisonEffect GetEffect( PoisonEffectEnum id )
		{
			return m_Registry[id];
		}
		
		// static methods for poisoning mobiles
		public static bool Poison( Mobile who, Mobile poisoner, KeyValuePair<PoisonEffectEnum, int>[] effects, int duration, int actingSpeed, bool ingested )
		{
			Dictionary<PoisonEffectEnum, int> tmp = new Dictionary<PoisonEffectEnum, int>();
			foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in effects )
				tmp[kvp.Key]=kvp.Value;
			
			return Poison( who, poisoner, tmp, duration, actingSpeed, ingested );
		}
		public static bool Poison( Mobile who, Mobile poisoner, Dictionary<PoisonEffectEnum, int> effects, int duration, int actingSpeed, bool ingested )
		{
			if ( who == null )
				return false;
			
			if( who is PlayerMobile && ((PlayerMobile)who).IsVampire )
				return false;
				
			if ( ingested )
			{
				List<PoisonEffectEnum> keyList = new List<PoisonEffectEnum>();
				foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in effects )
					keyList.Add( kvp.Key );
				foreach ( PoisonEffectEnum K in keyList )
					effects[K] *= 3;
			}
			
			PoisonAttachment attachment = XmlAttach.FindAttachment( who, typeof( PoisonAttachment ) ) as PoisonAttachment;
			if ( attachment != null )
			{
				int strength = 0;
				foreach( KeyValuePair<PoisonEffectEnum, int> kvp in effects )
					strength += kvp.Value;
				if ( attachment.PoisonStrength >= strength ) // already has stronger or equally strong poison applied
					return false;
				else
					attachment.Delete(); // this poison is stronger
			}
			attachment = new PoisonAttachment( effects, duration, actingSpeed, poisoner );
			XmlAttach.AttachTo( who, attachment );
			
			int level = 0;
			if ( attachment.PoisonStrength > 400 )
				level = 4;
			else if ( attachment.PoisonStrength > 300 )
				level = 3;
			else if ( attachment.PoisonStrength > 200 )
				level = 2;
			else if ( attachment.PoisonStrength > 100 )
				level = 1;
			
			if ( !who.Hidden )
				who.PublicOverheadMessage( MessageType.Regular, 0x22, 1042858 + (level * 2), who.Name );
			return true;
		}
		public static void Cure( Mobile who )
		{
			if ( who == null )
				return;
			PoisonAttachment attachment = XmlAttach.FindAttachment( who, typeof( PoisonAttachment ) ) as PoisonAttachment;
			if ( attachment != null )
			{
				attachment.Delete();
				who.SendMessage( 83, "You have been cured of poison." );
				who.Delta( MobileDelta.Flags ); // update bar
			}
		}
		public static bool Cure( Mobile who, int intensity )
		{
			if ( who == null )
				return false;

			PoisonAttachment attachment = XmlAttach.FindAttachment( who, typeof( PoisonAttachment ) ) as PoisonAttachment;
			if ( attachment != null )
			{
				if ( (intensity - (attachment.PoisonStrength/2))*0.01 > Utility.RandomDouble() )
				{
					Cure( who );
					return true;
				}
			}
			
			return false;
		}
	}
}
