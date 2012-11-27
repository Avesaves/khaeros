/*
 * Simple Statue System 
 * by Kleanthes@eurebia.net
 * inspired by the original Statue Maker System (Ultima Online, VetReward)
 * 
 * Version: 0.4 08/23/06   
 *
 * This system allows to place a human npc as a statue that will be
 * frozen in one position, so that it will not walk away or make any
 * animations. For this to work, a timer (one timer for alle statues
 * toegether) ticks every 10 seconds and updates all statues.
 * The animation that the statues do has two "tricks":
 * - The delay of the animation is 255, so that every frame of the
 *   animation will take more than 10 seconds. As the animation is 
 *   started again every 10 seconds, this will result in only one 
 *   frame being displayed all the time. 
 * - Some animations are shown backwards with a non standard number
 *   of frames. For example:
 *   If you show animation 16 backwards with a framecount of 5, it
 *   will start with the 2nd last frame. This way it's possible to
 *   show every animation frame you want. 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using Server.Network;
using Server.ContextMenus;
using System.Collections;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Items
{
    public enum StatuePoses
    {
        Ready = 0,
        Casting = 1,
        Salute = 2,
        Fighting = 3,
        AllPraiseMe = 4,
        HandsOnHips = 5
    }

    public enum StatueMaterial
    {
        None = 0x0,
        JadeX1 = 0xA15,
        BloodStoneX1 = 0xA19,
        BronzeX1 = 0xA7F,
        BronzeX2 = 0xA9A,
        AlabasterX1 = 0xA20,
        MarbleX1 = 0xA22,
        MarbleX2 = 0xA61,
        Granite1 = 0xAE4,
        AlabasterMarbleX0 = 0xB25,
        AlabasterMarble1 = 0xB37,
        AlabasterMarble2 = 0xAE3,
        AlabasterMarble3 = 0xA1A,
        Jade0 = 0xB93,
        Jade1 = 0xB94,
        Jade2 = 0xB95,
        Jade3 = 0xB96,
        Bronze0 = 0xB97,
        Bronze1 = 0xB98,
        Bronze2 = 0xB99,
        Bronze3 = 0xB9A
    }

    public class Statues
    {
        public static void Initialize() 
        {
            Timer.DelayCall( TimeSpan.FromSeconds( 10.0 ), new TimerCallback( UpdateStatues ) );            
        }        


        private static List<PlayerMadeStatue> m_list = new List<PlayerMadeStatue>();

        public static List<PlayerMadeStatue> StatuesList
        {
            get
            {
                return m_list;
            }
        }

        public static void UpdateStatues()
        {
            if (NetState.Instances.Count > 0)
            {
                foreach (PlayerMadeStatue s in m_list)
                {
                	if( s != null )
                   		s.UpdateStatue();
                }
            }
            Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(UpdateStatues));
        }
    }

    public class PlayerMadeStatue : Mobile, IKhaerosMobile
    {
        //start IKhaerosMobile mod
        public Mobile ShieldingMobile { get { return null; } set { } }
        public virtual int RideBonus { get { return 0; } }
        public int Intimidated { get { return 0; } set { } }
        public int Level { get { return 0; } set { } }
        public int RageFeatLevel { get { return 0; } set { } }
        public int ManeuverDamageBonus { get { return 0; } set { } }
        public int ManeuverAccuracyBonus { get { return 0; } set { } }
        public int TechniqueLevel { get { return 0; } set { } }
        public double ShieldValue { get { return 0; } set { } }
        public virtual void RemoveShieldOfSacrifice() { }
        public virtual void DisableManeuver() { }
        public virtual string GetPersonalPronoun() { return ""; }
        public virtual string GetReflexivePronoun() { return ""; }
        public virtual string GetPossessivePronoun() { return ""; }
        public virtual string GetPossessive() { return ""; }
        public string Technique { get { return null; } set { } }
        public bool Fizzled { get { return false; } set { } }
        public bool Enthralled { get { return false; } set { } }
        public bool CanUseMartialPower { get { return false; } }
        public bool CanUseMartialStance { get { return false; } }
        public bool CleaveAttack { get { return false; } set { } }
        public bool CanDodge { get { return false; } }
        public virtual bool IsAllyOf( Mobile mob ) { return false; }
        public virtual bool Evaded() { return false; }
        public virtual bool Dodged() { return false; }
        public virtual bool Snatched() { return false; }
        public virtual bool DeflectedProjectile() { return false; }
        public virtual bool IsTired() { return false; }
        public virtual bool IsTired( bool message ) { return false; }
        public virtual bool CanSummon() { return false; }
        public Feats Feats { get { return new Feats(); } set { } }
        public CombatStyles CombatStyles { get { return new CombatStyles(); } set { } }
        public DateTime NextFeatUse { get { return DateTime.MinValue; } set { } }
        public FeatList OffensiveFeat { get { return FeatList.None; } set { } }
        public BaseCombatManeuver CombatManeuver { get { return null; } set { } }
        public FeatList CurrentSpell { get { return FeatList.None; } set { } }
        public Timer CrippledTimer { get { return null; } set { } }
        public Timer DazedTimer { get { return null; } set { } }
        public Timer TrippedTimer { get { return null; } set { } }
        public Timer StunnedTimer { get { return null; } set { } }
        public Timer DismountedTimer { get { return null; } set { } }
        public Timer BlindnessTimer { get { return null; } set { } }
        public Timer DeafnessTimer { get { return null; } set { } }
        public Timer MutenessTimer { get { return null; } set { } }
        public Timer DisabledLegsTimer { get { return null; } set { } }
        public Timer DisabledLeftArmTimer { get { return null; } set { } }
        public Timer DisabledRightArmTimer { get { return null; } set { } }
        public Timer FeintTimer { get { return null; } set { } }
        public Timer HealingTimer { get { return null; } set { } }
        public Timer AuraOfProtection { get { return null; } set { } }
        public Timer JusticeAura { get { return null; } set { } }
        public Timer Sanctuary { get { return null; } set { } }
        public Timer RageTimer { get { return null; } set { } }
        public Timer ManeuverBonusTimer { get { return null; } set { } }
        public Timer FreezeTimer { get { return null; } set { } }
        public BaseStance Stance { get { return null; } set { } }
        public Mobile ShieldedMobile { get { return null; } set { } }
        public DateTime NextRage { get { return DateTime.MinValue; } set { } }
        private bool m_Deserialized;
        public bool Deserialized { get { return m_Deserialized; } set { m_Deserialized = value; } }
        //end IKhaerosMobile mod

        private int m_animation;
        private int m_framecount;
        private bool m_forward;

        [Constructable]
        public PlayerMadeStatue( Mobile owner ) : base()
        {
            this.Name = "Statue";
            this.BodyValue = 0x190;
            //this.Hue = 0x000;
            //this.SolidHueOverride = 0x000;
		this.Material = StatueMaterial.BronzeX1;
            RecalculateData();
            Statues.StatuesList.Add(this);
		this.Owner = owner;
		this.Blessed = true;
        }

        public PlayerMadeStatue(Serial serial) : base(serial) { }

	  private Mobile m_Owner;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return m_Owner; }
		set
		{
			m_Owner = value;
			InvalidateProperties();
		}
        }

        private String m_Engraving;

        [CommandProperty(AccessLevel.GameMaster)]
        public string Engraving
        {
            get 
            {
                if (m_Engraving != null)
                {
                    if (m_Engraving.Length > 0)
                    {
                        return m_Engraving; 
                    }
                    m_Engraving = null;
                }
                return m_Engraving;
            }
            set 
            {
                if (value != null)
                {
                    if (value.Length == 0)
                    {
                        m_Engraving = null;
                        UpdateEngraving();                            
                        return;
                    }
                }
                m_Engraving = value;
                UpdateEngraving();
            }
        }

        public void UpdateEngraving()
        {
            this.InvalidateProperties();
            if (m_Plinth != null)
            {
                m_Plinth.InvalidateProperties();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            if (Engraving != null)
            {
                list.Add(1072305, Engraving);
            }
        }

	
        private Plinth m_Plinth;
	  //private bool m_HasPlinth;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasPlinth
        {
            get { return (m_Plinth != null); }
            set
            {
                bool has = (m_Plinth != null);
                if (value != has)
                {
                    if (!value)
                    {
                        if (!m_Plinth.Deleted)
                        {
                            m_Plinth.Statue = null;
                            m_Plinth.Delete();
                        }
                        m_Plinth = null;
                        Z -= 5;
                    }
                    else
                    {
                        Z += 5;
                        m_Plinth = new Plinth(this);
                        CheckLocation();
                    }
                }
            }
                
        }
	

        private StatuePoses m_pose = StatuePoses.Ready;

        [CommandProperty(AccessLevel.GameMaster)]
        public StatuePoses Pose
        {
            get { return m_pose; }
            set 
            {
                //if (m_pose != value)
                //{
                    m_pose = value;
                    RecalculateData();
                    StartUpdateStatue();
                //}
            }
        }

        private StatueMaterial m_material = StatueMaterial.None;

        [CommandProperty(AccessLevel.GameMaster)]
        public StatueMaterial Material
        {
            get { return m_material; }
            set
            {
                if (m_material != value)
                {
                    m_material = value;
                    this.SolidHueOverride = (int)m_material;
                    this.Hue = (int)m_material;

					if (m_Plinth != null)
					{
						m_Plinth.Hue = (int)m_material;
					}
                }
         	}
     	}
       
        public void CheckLocation()
        {
            if (m_Plinth != null)
            {
                if (Map != m_Plinth .Map)
                {
                    m_Plinth.Map = Map;
                }                
                if ( (X != m_Plinth.X) || (Y != m_Plinth.Y) || (Z != (m_Plinth.Z + 5))) 
                {
                    m_Plinth.Location = new Point3D(X, Y, Z - 5);                    
                }                
            }
        }

        public override void OnAosSingleClick(Mobile from)
        {
            this.UpdateStatue();
        }

        public override void OnSingleClick(Mobile from)
        {
            this.UpdateStatue();
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);
            if (m.CanSee(this))
            {
                UpdateStatue();
            }
        }

        protected override void OnLocationChange(Point3D oldLocation)
        {
            base.OnLocationChange(oldLocation);
            if (m_Plinth != null)
            {
                CheckLocation();
            }
            StartUpdateStatue();
        }

        public override void OnDelete()
        {
        	Statues.StatuesList.Remove(this);
        	
            if (m_Plinth != null)
            {
                if (!m_Plinth.Deleted)
                {
                    m_Plinth.Statue = null;
                    m_Plinth.Delete();                    
                }
            }
            
            base.OnDelete();
        }

        public override void Delta(MobileDelta flag)
        {
            base.Delta(flag);
            if ( (flag == MobileDelta.Direction) || (flag == MobileDelta.Hue) )
            {
                StartUpdateStatue();
            }            
        }

        public void StartUpdateStatue()
        {
            Timer.DelayCall(TimeSpan.FromSeconds(0.1), new TimerCallback(UpdateStatue));
        }

        private void RecalculateData()
        {
            switch (m_pose)
            {
                case StatuePoses.AllPraiseMe:
                    {
                        m_animation = 17;
                        m_framecount = 5;
                        m_forward = false;
                        break;
                    }
                case StatuePoses.HandsOnHips:
                    {
                        m_animation = 6;
                        m_framecount = 4;
                        m_forward = false;
                        break;
                    }
                case StatuePoses.Ready:
                    {
                        m_animation = 4;
                        m_framecount = 1;
                        m_forward = true;
                        break;
                    }
                case StatuePoses.Casting:
                    {
                        m_animation = 16;
                        m_framecount = 5;
                        m_forward = false;
                        break;
                    }
                case StatuePoses.Salute:
                    {
                        m_animation = 33;
                        m_framecount = 3;
                        m_forward = false;
                        break;
                    }
                case StatuePoses.Fighting:
                    {
                        m_animation = 9;
                        m_framecount = 5;
                        m_forward = false;
                        break;
                    }
               }
        }

        public void UpdateStatue()
        {
        	if( this != null && !this.Deleted )
        	{
	            Sector sect = Map.GetSector( this );
	            if (sect.Active)
	            {
	                Animate(m_animation, m_framecount, 1, m_forward, false, 255);
	            }
        	}
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)4);

            writer.Write((int)m_pose);
            writer.Write((int)m_material);
            writer.Write(m_Plinth);
            writer.Write( (string) m_Engraving);

		writer.Write( (Mobile) m_Owner);
		//writer.Write(Plinth);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

		
            Pose = (StatuePoses)reader.ReadInt();
            Material = (StatueMaterial)reader.ReadInt();

            if (version >= 1)
            {
                m_Plinth = reader.ReadItem() as Plinth;
                //m_HasPlinth = reader.ReadBool();
            }
            if (version >= 2)
            {
                m_Engraving = reader.ReadString();
            }
            if (version >= 3)
            {
                //m_HasPlinth = reader.ReadBool();
			m_Owner = reader.ReadMobile();
            }

		//m_Owner = reader.ReadMobile();
            //m_HasPlinth = reader.ReadBool();
		//Plinth = reader.ReadBool();

            if (!Statues.StatuesList.Contains(this))
            {
                Statues.StatuesList.Add(this);
            }
            StartUpdateStatue();
		


		//read( 3 )
		/*
		switch( version )
		{
			case 3: //maybe 3, maybe 4, or more?
			{
				m_Owner = reader.ReadMobile();

				goto case 2;
			}
			case 2:
			{
				m_Engraving = reader.ReadString();
				//read some others if they were serialized when version was 2?
	
				goto case 1;
			}
			case 1:
			{
				m_Plinth = reader.ReadItem() as Plinth;
				//as above

				goto case 0;
			}
			case 0:
			{
				Pose = (StatuePoses)reader.ReadInt();
            	Material = (StatueMaterial)reader.ReadInt();
				break;
			}
		}
		*/
        }
    }

    public class Plinth : Item
    {
        public Plinth() : this(null) { }

        public Plinth( PlayerMadeStatue statue ) : base(0x32F2)
        {
            m_Statue = statue;
            Name = "statue plinth";
		Hue = statue.Hue;
		Movable = false;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            if (m_Statue != null)
            {
                if (m_Statue.Engraving != null)
                {
                    list.Add(1072305, m_Statue.Engraving);
                }
            }
        }

		public override void OnDoubleClick( Mobile from )
		{
			if( from != m_Statue.Owner && from.AccessLevel < AccessLevel.GameMaster )
				from.SendMessage("You do not own this Statue.");
			else
				from.SendGump( new PlayerStatueGump( from, m_Statue ) );
		}


        [CommandProperty(AccessLevel.GameMaster)]
        public string Engraving
        {
            get
            {
                if (m_Statue == null)
                {
                    return "No Statue";
                }
                return m_Statue.Engraving;
            }
            set
            {
                if (m_Statue != null)
                {
                    m_Statue.Engraving = value;
                }
            }
        }

        public Plinth(Serial serial) : base(serial) { }
        
        private PlayerMadeStatue m_Statue;

        public PlayerMadeStatue Statue
        {
            get { return m_Statue; }
            set 
            { 
                m_Statue = value;
		    InvalidateProperties();                
            }
        }

        public void CheckLocation()
        {
            if (m_Statue != null)
            {
                if (Map != m_Statue.Map)
                {
                    m_Statue.Map = Map;
                }
                if ((X != m_Statue.X) || (Y != m_Statue.Y) || (Z != (m_Statue.Z - 5)))
                {
                    m_Statue.Location = new Point3D(X, Y, Z + 5);
                }   
            }
        }

        public override void OnLocationChange(Point3D oldLocation)
        {
            base.OnLocationChange(oldLocation);
            CheckLocation();
        }

        public override void OnDelete()
        {
            base.OnDelete();
            if (m_Statue != null)
            {
                if (!m_Statue.Deleted)
                {
                    m_Statue.HasPlinth = false;
                }
            }
            
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
            writer.Write(m_Statue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Statue = reader.ReadMobile() as PlayerMadeStatue;
            //m_Statue = reader.ReadMobile();
        }
    }
}
