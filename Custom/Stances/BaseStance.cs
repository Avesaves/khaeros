using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class BaseStance : object
	{
		public virtual string Name{ get{ return "no stance"; } }
		public virtual double DamageBonus{ get{ return 0; } }
		public virtual int AccuracyBonus{ get{ return 0; } }
		public virtual double SpeedBonus{ get{ return 0; } }
		public virtual int DefensiveBonus{ get{ return 0; } }
		public virtual string TurnedOnEmote{ get{ return ""; } }
		public virtual string TurnedOffEmote{ get{ return ""; } }
		public virtual bool Melee{ get{ return true; } }
		public virtual bool Ranged{ get{ return true; } }
		public virtual bool Armour{ get{ return true; } }
		public virtual bool MartialArtistStance{ get{ return false; } }
		
		private int m_FeatLevel;
		public int FeatLevel{ get{ return m_FeatLevel; } set{ m_FeatLevel = value; } }
		
		public BaseStance( int featlevel )
		{
			m_FeatLevel = featlevel;
		}
		
		public BaseStance() : this( 0 )
		{
		}
		
		public virtual bool CanUseThisStance( Mobile mob )
		{
			if( mob != null && !mob.Deleted && mob is IKhaerosMobile && mob.Alive )
				return true;
			
			return false;
		}
	}
}
