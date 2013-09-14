using System;
using System.Collections;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Spells.Necromancy
{
	public class AnimateDeadSpell : NecromancerSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Animate Dead", "Uus Corp",
				SpellCircle.Fourth, // 0.5 + 1.0 = 1.5s base cast delay
				203,
				9031,
				Reagent.GraveDust,
				Reagent.DaemonBlood
			);

		public override double RequiredSkill{ get{ return 40.0; } }
		public override int RequiredMana{ get{ return 23; } }

		public AnimateDeadSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendLocalizedMessage( 1061083 ); // Animate what corpse?
		}

		private class CreatureGroup
		{
			public Type[] m_Types;
			public SummonEntry[] m_Entries;

			public CreatureGroup( Type[] types, SummonEntry[] entries )
			{
				m_Types = types;
				m_Entries = entries;
			}
		}

		private class SummonEntry
		{
			public Type[] m_ToSummon;
			public int m_Requirement;

			public SummonEntry( int requirement, params Type[] toSummon )
			{
				m_ToSummon = toSummon;
				m_Requirement = requirement;
			}
		}

		private static CreatureGroup FindGroup( Type type )
		{
			for ( int i = 0; i < m_Groups.Length; ++i )
			{
				CreatureGroup group = m_Groups[i];
				Type[] types = group.m_Types;

				bool contains = ( types.Length == 0 );

				for ( int j = 0; !contains && j < types.Length; ++j )
					contains = ( types[j] == type );

				if ( contains )
					return group;
			}

			return null;
		}

		private static CreatureGroup[] m_Groups = new CreatureGroup[]
			{
				// Undead group--empty
				new CreatureGroup( SlayerGroup.GetEntryByName( SlayerName.Silver ).Types, new SummonEntry[0] ),
				// Insects
				new CreatureGroup( new Type[]
				{
					typeof( AssassinSpider ), typeof( AmbusherSpider ), typeof( GiantWolfSpider ), typeof( FunnelWebSpider ),
					typeof( FormianMyrmarch ), typeof( FormianWarrior ),
					typeof( FormianWorker ),
					typeof( DriderQueen ), typeof( DriderWorker ), typeof( DriderCleric ),
					typeof( DriderWarrior )
					// TODO: Giant beetle? Ant lion? Ophidians?
				},
				new SummonEntry[]
				{
					new SummonEntry( 0, typeof( FleshJelly ) )
				} ),
				// Mounts
				new CreatureGroup( new Type[]
				{
					typeof( Horse ), typeof( Unicorn )
				}, new SummonEntry[]
				{
					new SummonEntry( 10000, typeof( GallowayHorse ) ),
					new SummonEntry(     0, typeof( KudaHorse ) )
				} ),
				// Elementals
				new CreatureGroup( new Type[]
				{
					typeof( EarthElemental ), 
					typeof( FireElemental ), 
					typeof( AirElemental ), typeof( WaterElemental ),
				}, new SummonEntry[]
				{
					new SummonEntry( 5000, typeof( Banshee ) ),
					new SummonEntry(    0, typeof( Wraith ) )
				} ),
				// Dragons
				new CreatureGroup( new Type[]
				{
					typeof( Dragon ), typeof( SkeletalDragon ), typeof( Wyvern )
				}, new SummonEntry[]
				{
					new SummonEntry( 18000, typeof( SkeletalDragon ) ),
					new SummonEntry( 10000, typeof( FleshGolem ) ),
					new SummonEntry(  5000, typeof( Zombie ) ),
					new SummonEntry(  3000, typeof( SkeletalSoldier ) ),
					new SummonEntry(  2000, typeof( Mummy ) ),
					new SummonEntry(  1000, typeof( SkeletalLord ) ),
					new SummonEntry(     0, typeof( LesserBoneGolem ) )
				} ),
				// Default group
				new CreatureGroup( new Type[0], new SummonEntry[]
				{
					new SummonEntry( 18000, typeof( Skeleton ) ),
					new SummonEntry( 10000, typeof( FleshGolem ) ),
					new SummonEntry(  5000, typeof( Zombie ) ),
					new SummonEntry(  3000, typeof( SkeletalSoldier ) ),
					new SummonEntry(  2000, typeof( Mummy ) ),
					new SummonEntry(  1000, typeof( SkeletalLord ) ),
					new SummonEntry(     0, typeof( LesserBoneGolem ) ) 
				} ),
			};

		public void Target( object obj )
		{
			Corpse c = obj as Corpse;

			if ( c == null )
			{
				Caster.SendLocalizedMessage( 1061084 ); // You cannot animate that.
			}
			else
			{
				Type type = null;

				if ( c.Owner != null )
					type = c.Owner.GetType();

				if ( c.ItemID != 0x2006 || c.Channeled || type == typeof( PlayerMobile ) || type == null || (c.Owner != null && c.Owner.Fame < 100) || ((c.Owner != null) && (c.Owner is BaseCreature) && (((BaseCreature)c.Owner).Summoned || ((BaseCreature)c.Owner).IsBonded)) )
				{
					Caster.SendLocalizedMessage( 1061085 ); // There's not enough life force there to animate.
				}
				else
				{
					CreatureGroup group = FindGroup( type );

					if ( group != null )
					{
						if ( group.m_Entries.Length == 0 )
						{
							Caster.SendLocalizedMessage( 1061086 ); // You cannot animate undead remains.
						}
						else if ( CheckSequence() )
						{
							Point3D p = c.GetWorldLocation();
							Map map = c.Map;

							if ( map != null )
							{
								Effects.PlaySound( p, map, 0x1FB );
								Effects.SendLocationParticles( EffectItem.Create( p, map, EffectItem.DefaultDuration ), 0x3789, 1, 40, 0x3F, 3, 9907, 0 );

								Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback( SummonDelay_Callback ), new object[]{ Caster, c, p, map, group } );
							}
						}
					}
				}
			}

			FinishSequence();
		}

		private static Hashtable m_Table = new Hashtable();

		public static void Unregister( Mobile master, Mobile summoned )
		{
			if ( master == null )
				return;

			ArrayList list = (ArrayList)m_Table[master];

			if ( list == null )
				return;

			list.Remove( summoned );

			if ( list.Count == 0 )
				m_Table.Remove( master );
		}

		public static void Register( Mobile master, Mobile summoned )
		{
			if ( master == null )
				return;

			ArrayList list = (ArrayList)m_Table[master];

			if ( list == null )
				m_Table[master] = list = new ArrayList();

			for ( int i = list.Count - 1; i >= 0; --i )
			{
				if ( i >= list.Count )
					continue;

				Mobile mob = (Mobile)list[i];

				if ( mob.Deleted )
					list.RemoveAt( i-- );
			}

			list.Add( summoned );

			if ( list.Count > 3 )
				Timer.DelayCall( TimeSpan.Zero, new TimerCallback( ((Mobile)list[0]).Kill ) );

			Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback( Summoned_Damage ), summoned );
		}

		private static void Summoned_Damage( object state )
		{
			Mobile mob = (Mobile)state;

			if ( mob.Hits > 0 )
				--mob.Hits;
			else
				mob.Kill();
		}

		private static void SummonDelay_Callback( object state )
		{
			object[] states = (object[])state;

			Mobile caster = (Mobile)states[0];
			Corpse corpse = (Corpse)states[1];
			Point3D loc = (Point3D)states[2];
			Map map = (Map)states[3];
			CreatureGroup group = (CreatureGroup)states[4];

			if ( corpse.ItemID != 0x2006 )
				return;

			Mobile owner = corpse.Owner;

			if ( owner == null )
				return;

			double necromancy = caster.Skills[SkillName.Polearms].Value;
			double spiritSpeak = caster.Skills[SkillName.Linguistics].Value;

			int casterAbility = 0;

			casterAbility += (int)(necromancy * 30);
			casterAbility += (int)(spiritSpeak * 70);
			casterAbility /= 10;
			casterAbility *= 18;

			if ( casterAbility > owner.Fame )
				casterAbility = owner.Fame;

			if ( casterAbility < 0 )
				casterAbility = 0;

			Type toSummon = null;
			SummonEntry[] entries = group.m_Entries;

			for ( int i = 0; toSummon == null && i < entries.Length; ++i )
			{
				SummonEntry entry = entries[i];

				if ( casterAbility < entry.m_Requirement )
					continue;

				Type[] animates = entry.m_ToSummon;

				if ( animates.Length >= 0 )
					toSummon = animates[Utility.Random( animates.Length )];
			}

			if ( toSummon == null )
				return;

			Mobile summoned = null;

			try{ summoned = Activator.CreateInstance( toSummon ) as Mobile; }
			catch{}

			if ( summoned == null )
				return;

			if ( summoned is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)summoned;

				// to be sure
				bc.Tamable = false;

				if ( bc is BaseMount )
					bc.ControlSlots = 1;
				else
					bc.ControlSlots = 0;

				Effects.PlaySound( loc, map, bc.GetAngerSound() );

				BaseCreature.Summon( (BaseCreature)summoned, false, caster, loc, 0x28, TimeSpan.FromDays( 1.0 ) );
			}

			if ( summoned is SkeletalDragon )
				Scale( (SkeletalDragon)summoned, 50 ); // lose 50% hp and strength

			summoned.Fame = 0;
			summoned.Karma = -1500;

			summoned.MoveToWorld( loc, map );

			corpse.ProcessDelta();
			corpse.SendRemovePacket();
			corpse.ItemID = Utility.Random( 0xECA, 9 ); // bone graphic
			corpse.Hue = 0;
			corpse.ProcessDelta();

			Register( caster, summoned );
		}

		private static void Scale( BaseCreature bc, int scalar )
		{
			int toScale;

			toScale = bc.RawStr;
			bc.RawStr = AOS.Scale( toScale, scalar );

			toScale = bc.HitsMaxSeed;

			if ( toScale > 0 )
				bc.HitsMaxSeed = AOS.Scale( toScale, scalar );

			bc.Hits = bc.Hits; // refresh hits
		}

		private class InternalTarget : Target
		{
			private AnimateDeadSpell m_Owner;

			public InternalTarget( AnimateDeadSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				m_Owner.Target( o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
