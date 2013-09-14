using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a War Horse corpse" )]
	public class WarHorse : BaseMount, ILargePrey, IPlainsCreature, IEnraged, IRacialMount
	{
		public override bool ParryDisabled{ get{ return true; } }
		public override int[] Hues{ get{ return new int[]{2714,2798,2802}; } }
		
		public override bool SubdueBeforeTame{ get{ return true; } }

        private WarHorseAttackTimer m_WarTimer;
		
		[Constructable]
		public WarHorse() : this( "a War Horse" )
		{
		}
		
		[Constructable]
        public WarHorse(string name)
            : base(name, 0xE2, 0x3EA0, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{			
			BaseSoundID = 0xA8;

			SetStr( 120 );
			SetDex( 60 );
			SetInt( 25 );

			SetHits( 150 );

			SetDamage( 2, 3 );

			SetDamageType( ResistanceType.Blunt, 100 );		

			SetResistance( ResistanceType.Blunt, 20, 25 );
			SetResistance( ResistanceType.Slashing, 20, 25 );
			SetResistance( ResistanceType.Piercing, 20, 25 );
			SetResistance( ResistanceType.Poison, 25 );
			SetResistance( ResistanceType.Energy, 65, 75 );

			SetSkill( SkillName.Tactics, 67.6, 69.3 );
			SetSkill( SkillName.UnarmedFighting, 75.5, 82.5 );
			
			HueGroup = Utility.Random(3);

			Fame = 1400;
			Karma = -1400;

			VirtualArmor = 20;

			Tamable = false;
			ControlSlots = 2;
			MinTameSkill = 89.1;

            m_WarTimer = new WarHorseAttackTimer(this);
            m_WarTimer.Start();
		}
        
        public override void PrepareToGiveBirth()
		{
			GiveBirth( new WarHorse() );
		}

		public override int Meat{ get{ return 10; } }
		public override int Bones{ get{ return 10; } }
		public override int Hides{ get{ return 6; } }
		public override HideType HideType{ get{ return HideType.Thick; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public WarHorse( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 2 )
			{
				RawStr = 120;
				RawDex = 60;
				RawInt = 25;
	
				RawHits = 150;
				RawStam = 150;
				RawMana = 20;
	
				SetDamage( 2, 3 );

				SetResistance( ResistanceType.Poison, 25 );
				SetResistance( ResistanceType.Energy, 65, 75 );
				
				if( this.BodyValue == 284 )
				{
					SetResistance( ResistanceType.Blunt, 30, 35 );
					SetResistance( ResistanceType.Slashing, 30, 35 );
					SetResistance( ResistanceType.Piercing, 30, 35 );
					VirtualArmor = 35;
				}
				
				else
				{
					SetResistance( ResistanceType.Blunt, 20, 25 );
					SetResistance( ResistanceType.Slashing, 20, 25 );
					SetResistance( ResistanceType.Piercing, 20, 25 );
					VirtualArmor = 25;
				}

				int level = 1;
				
				while( level < this.Level )
				{
					this.RawStr += this.StatScale;
		            this.RawDex += this.StatScale;
		            this.RawInt += this.StatScale;
		            this.RawHits += this.StatScale;
		            this.RawStam += this.StatScale;
		            this.RawMana += this.StatScale;
		            
		            if( level % 2 != 0 )
		            	this.DamageMin++;
		            
		            else
		            	this.DamageMax++;
		            
					level++;
				}
			}
			
			if( version < 3 )
				ControlSlots = 2;

            m_WarTimer = new WarHorseAttackTimer(this);
            m_WarTimer.Start();
		}
	}

    public class WarHorseAttackTimer : Timer
    {
        private WarHorse m_Horse;

        public WarHorseAttackTimer(WarHorse h) : base(System.TimeSpan.FromSeconds(1), System.TimeSpan.FromMinutes(Utility.RandomMinMax(1,3)))
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

                if (m_Horse.Rider is PlayerMobile && (m_Horse.Rider as PlayerMobile).Nation != Nation.Northern)
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

                if (m_Horse.Rider != null && !m_Horse.Rider.Deleted && m_Horse.Rider is PlayerMobile && (m_Horse.Rider as PlayerMobile).Nation == Nation.Northern)
                    Interval = System.TimeSpan.FromSeconds(Utility.RandomMinMax(60 - (m_Horse.XPScale * Utility.RandomMinMax(m_Horse.XPScale,10)), 300 - Utility.RandomMinMax(m_Horse.XPScale * 2, m_Horse.Level * 2)));
                else
                    Interval = System.TimeSpan.FromSeconds(Utility.RandomMinMax(60 - (m_Horse.XPScale * Utility.Random(10)), 300 - Utility.Random(m_Horse.Level)));
            }

            base.OnTick();
        }

        private static bool DirectionCheck(WarHorse h, Mobile m)
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
