using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class RoseanHorse : Horse, IWarHorse
	{
		public override int[] Hues{ get{ return new int[]{2803,2725,2830}; } }	
		public override bool SubdueBeforeTame{ get{ return true; } }
		public override bool ParryDisabled{ get{ return true; } }
        private WarHorseAttackTimer m_WarTimer;
		
		[Constructable]
		public RoseanHorse() : this( "a Rosean Horse" )
		{
		}
		
		[Constructable]
		public RoseanHorse( string name ) : base( name )
		{
			NewBreed = "Rosean Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new RoseanHorse() );
		}

		public RoseanHorse(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 1);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			
			if( version < 1 )
			{
				this.BodyValue = 226;
				this.ItemID = 0x3EA0;
			}
						if( version < 3 )
				ControlSlots = 2;

            m_WarTimer = new WarHorseAttackTimer(this);
            m_WarTimer.Start();
		}
    public class WarHorseAttackTimer : Timer
    {
        private RoseanHorse m_Horse;

        public WarHorseAttackTimer(RoseanHorse h) : base(System.TimeSpan.FromSeconds(1), System.TimeSpan.FromMinutes(Utility.RandomMinMax(1,3)))
        {
            m_Horse = h;
            Priority = TimerPriority.OneMinute;
        }

        protected override void OnTick()
        {
            if (m_Horse == null || m_Horse.Deleted)
            {
                this.Stop();
                return;
            }

            if (m_Horse.Rider == null || m_Horse.Rider.Deleted)
            {
                Priority = TimerPriority.OneMinute;
                return;
            }
            else
                Priority = TimerPriority.FiveSeconds;

            if (m_Horse.Rider.AccessLevel > AccessLevel.Player)
                return;

            if (m_Horse.Controlled && m_Horse.Rider != null && !m_Horse.Rider.Deleted && m_Horse.Rider.Warmode)
            {
                int dmg = 1;
                string attackMessage = "";
                Mobile damaged = null;

                if (m_Horse.Rider is PlayerMobile && (m_Horse.Rider as PlayerMobile).Nation != Nation.Mhordul)
                    if (Utility.RandomBool())
                        return;

                if (m_Horse.Rider is BaseKhaerosMobile && (m_Horse.Rider as BaseKhaerosMobile).Feats.GetFeatLevel(FeatList.Riding) < 3)
                    return;

                if (m_Horse.Rider is PlayerMobile && (m_Horse.Rider as PlayerMobile).Feats.GetFeatLevel(FeatList.MountedCombat) < 1)
                    return;
                
                if (m_Horse.Rider is PlayerMobile)
                {
                    if ((m_Horse.Rider as PlayerMobile).Feats.GetFeatLevel(FeatList.MountedCombat) > 1)
                        dmg += Utility.RandomMinMax(1,(m_Horse.Rider).Mana) / 10;
                    if ((m_Horse.Rider as PlayerMobile).Feats.GetFeatLevel(FeatList.MountedCombat) > 2)
                        dmg += Utility.RandomMinMax(1,(m_Horse.Rider).Int) / 10;
                }

                IPooledEnumerable eable = m_Horse.Rider.Map.GetMobilesInRange(m_Horse.Rider.Location, 1);

                foreach (Mobile m in eable)
                {
                    if (m.Mount == null && (m.Combatant == m_Horse.Rider || m_Horse.Rider.Combatant == m) && WarHorseAttackTimer.DirectionCheck(m_Horse, m) && m_Horse.Rider.InRange(m.Location, 1))
                    {
                        if (Utility.RandomMinMax(1,100) > 50)
                        {
                            if(m is PlayerMobile)
                            {
                                if(m_Horse.Rider is PlayerMobile)
                                    dmg += Utility.RandomMinMax(m_Horse.Level, m_Horse.Level + (m_Horse.Str / Utility.RandomMinMax(2, 5 - (m_Horse.Rider as PlayerMobile).Feats.GetFeatLevel(FeatList.MountedCombat))));
                                else if (m_Horse.Rider is BaseKhaerosMobile)
                                    dmg += Utility.RandomMinMax(m_Horse.Level, m_Horse.Level + (m_Horse.Str / Utility.RandomMinMax(2, 5 - (m_Horse.Rider as BaseKhaerosMobile).Feats.GetFeatLevel(FeatList.MountedCombat))));
                                
                                dmg -= (m as PlayerMobile).BluntResistance;

                                if (dmg > 0 && m.FindItemOnLayer(Layer.TwoHanded) != null && m.FindItemOnLayer(Layer.TwoHanded) is BaseShield)
                                {
                                    dmg = dmg / 2;
                                    if (Utility.RandomBool())
                                        (m.FindItemOnLayer(Layer.TwoHanded) as BaseShield).DegradeArmor(dmg);
                                }

                                dmg -= (m as PlayerMobile).Feats.GetFeatLevel(FeatList.DamageIgnore) * 2;
                                dmg -= (m as PlayerMobile).Feats.GetFeatLevel(FeatList.PlateMastery) * (m as PlayerMobile).HeavyPieces;
                            }
                            else if (m is BaseCreature)
                            {
                                if (m_Horse.Rider is PlayerMobile)
                                    dmg += Utility.RandomMinMax(m_Horse.Level, m_Horse.Level + (m_Horse.Str / Utility.RandomMinMax(2, 5 - (m_Horse.Rider as PlayerMobile).Feats.GetFeatLevel(FeatList.MountedCombat))));
                                else if (m_Horse.Rider is BaseKhaerosMobile)
                                    dmg += Utility.RandomMinMax(m_Horse.Level, m_Horse.Level + (m_Horse.Str / Utility.RandomMinMax(2, 5 - (m_Horse.Rider as BaseKhaerosMobile).Feats.GetFeatLevel(FeatList.MountedCombat))));

                                dmg -= Utility.RandomMinMax(1, (m as BaseCreature).BluntResistSeed);

                                if (dmg > 0 && m.FindItemOnLayer(Layer.TwoHanded) != null && m.FindItemOnLayer(Layer.TwoHanded) is BaseShield)
                                {
                                    dmg = dmg / 2;
                                    if (Utility.RandomBool())
                                        (m.FindItemOnLayer(Layer.TwoHanded) as BaseShield).DegradeArmor(dmg);
                                }
                            }

                            if (dmg > 0)
                            {
                                attackMessage = ("*" + m_Horse.Name.ToString() + " rears up, kicking and stomping upon " + m.Name.ToString() + "*");
                                damaged = m;
                                continue;
                            }
                        }
                        else
                        {
                            if (m is PlayerMobile)
                            {
                                if (m_Horse.Rider is PlayerMobile)
                                    dmg += Utility.RandomMinMax(m_Horse.Level, m_Horse.Level + (m_Horse.Dex / Utility.RandomMinMax(2, 5 - (m_Horse.Rider as PlayerMobile).Feats.GetFeatLevel(FeatList.MountedCombat))));
                                else if (m_Horse.Rider is BaseKhaerosMobile)
                                    dmg += Utility.RandomMinMax(m_Horse.Level, m_Horse.Level + (m_Horse.Dex / Utility.RandomMinMax(2, 5 - (m_Horse.Rider as BaseKhaerosMobile).Feats.GetFeatLevel(FeatList.MountedCombat))));
                                
                                dmg -= (m as PlayerMobile).PiercingResistance;

                                if (dmg > 0 && (m as PlayerMobile).FindItemOnLayer(Layer.Helm) != null && (m as PlayerMobile).FindItemOnLayer(Layer.Helm) is Helmet)
                                {
                                    dmg = dmg / 2;
                                    if (Utility.RandomBool())
                                        (m.FindItemOnLayer(Layer.Helm) as Helmet).DegradeArmor(dmg);
                                }

                                dmg -= (m as PlayerMobile).Feats.GetFeatLevel(FeatList.DamageIgnore) * 2;
                                dmg -= (m as PlayerMobile).Feats.GetFeatLevel(FeatList.RangedDefense) * (m as PlayerMobile).MediumPieces;
                            }
                            else if (m is BaseCreature)
                            {
                                if (m_Horse.Rider is PlayerMobile)
                                    dmg += Utility.RandomMinMax(m_Horse.Level, m_Horse.Level + (m_Horse.Dex / Utility.RandomMinMax(2, 5 - (m_Horse.Rider as PlayerMobile).Feats.GetFeatLevel(FeatList.MountedCombat))));
                                else if (m_Horse.Rider is BaseKhaerosMobile)
                                    dmg += Utility.RandomMinMax(m_Horse.Level, m_Horse.Level + (m_Horse.Dex / Utility.RandomMinMax(2, 5 - (m_Horse.Rider as BaseKhaerosMobile).Feats.GetFeatLevel(FeatList.MountedCombat))));

                                dmg -= Utility.RandomMinMax(1, (m as BaseCreature).PiercingResistSeed);

                                if (dmg > 0 && m.FindItemOnLayer(Layer.Helm) != null && m.FindItemOnLayer(Layer.Helm) is Helmet)
                                {
                                    dmg = dmg / 2;
                                    if (Utility.RandomBool())
                                        (m.FindItemOnLayer(Layer.Helm) as Helmet).DegradeArmor(dmg);
                                }
                            }

                            if (dmg > 0)
                            {
                                attackMessage = ("*" + m_Horse.Name.ToString() + " tears at " + m.Name.ToString() + "'s face with its teeth*");
                                damaged = m;
                                continue;
                            }
                        }
                        continue;
                    }
                }
                eable.Free();

                if (dmg > 0 && damaged != null && !damaged.Deleted && m_Horse != null && !m_Horse.Deleted && !m_Horse.IsDeadBondedPet && !m_Horse.IsDeadPet && m_Horse.Rider.InRange(damaged.Location, 1))
                {
                    m_Horse.Rider.Emote(attackMessage);
                    m_Horse.Rider.PlaySound(m_Horse.GetAttackSound());
                    damaged.Damage(dmg, m_Horse, true);
                }

               // if (m_Horse.Rider != null && !m_Horse.Rider.Deleted && m_Horse.Rider is PlayerMobile && (m_Horse.Rider as PlayerMobile).Nation == Nation.Northern)
               //     Interval = System.TimeSpan.FromSeconds(Utility.RandomMinMax(60 - (m_Horse.XPScale * Utility.RandomMinMax(m_Horse.XPScale,10)), 300 - Utility.RandomMinMax(m_Horse.XPScale * 2, m_Horse.Level * 2)));
             //   else
                    Interval = System.TimeSpan.FromSeconds(Utility.RandomMinMax(60 - (m_Horse.XPScale * Utility.Random(10)), 300 - Utility.Random(m_Horse.Level)));
            }

            base.OnTick();
        }

        private static bool DirectionCheck(RoseanHorse h, Mobile m)
        {
            if (h.Rider == null || h.Rider.Deleted)
                return false;

            switch (h.Rider.GetDirectionTo(m))
            {
                case Direction.North:
                    switch (h.Rider.Direction)
                    {
                        case Direction.North: return true;
                        case Direction.Right: return true;
                        case Direction.Up: return true;
                        default: return false;
                    }
                case Direction.Right:
                    switch (h.Rider.Direction)
                    {
                        case Direction.North: return true;
                        case Direction.Right: return true;
                        case Direction.East: return true;
                        default: return false;
                    }
                case Direction.East:
                    switch (h.Rider.Direction)
                    {
                        case Direction.East: return true;
                        case Direction.Right: return true;
                        case Direction.Down: return true;
                        default: return false;
                    }
                case Direction.Down:
                    switch (h.Rider.Direction)
                    {
                        case Direction.Down: return true;
                        case Direction.East: return true;
                        case Direction.South: return true;
                        default: return false;
                    }
                case Direction.South:
                    switch (h.Rider.Direction)
                    {
                        case Direction.South: return true;
                        case Direction.Down: return true;
                        case Direction.Left: return true;
                        default: return false;
                    }
                case Direction.Left:
                    switch (h.Rider.Direction)
                    {
                        case Direction.Left: return true;
                        case Direction.South: return true;
                        case Direction.West: return true;
                        default: return false;
                    }
                case Direction.West:
                    switch (h.Rider.Direction)
                    {
                        case Direction.West: return true;
                        case Direction.Left: return true;
                        case Direction.Up: return true;
                        default: return false;
                    }
                case Direction.Up:
                    switch (h.Rider.Direction)
                    {
                        case Direction.Up: return true;
                        case Direction.West: return true;
                        case Direction.North: return true;
                        default: return false;
                    }

                default: return false;
            }
        }
	}
}
}