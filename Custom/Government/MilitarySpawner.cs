using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class MilitarySpawner : XmlSpawner
    {
        PlayerMobile m; //testing messages

        public const double RespawnDelay = 180.0;

        #region Private Variable declarations

        //Spawning Info
        //private MilitaryWayPoint m_InitialWayPoint;
        private bool m_Stationed;
        private String m_SpawnString;

        //Soldier Info
        private GovernmentEntity m_Government;        
        private int m_Arms;
        private int m_Training;
        private Direction m_FaceDirection;
        private int m_PayRate;

        #endregion

        #region Getters/Setters

        //Spawning Info
        public bool Stationed { get { return m_Stationed; } set { m_Stationed = value; } }
        public String SpawnString { get { return m_SpawnString; } set { m_SpawnString = value; } }

        //Soldier Info
        public GovernmentEntity Government { get { return m_Government; } set { m_Government = value; } }
        public int Armaments { get { return m_Arms; } set { m_Arms = value; } }
        public int Training { get { return m_Training; } set { m_Training = value; } }
        public Direction FaceDirection { get { return m_FaceDirection; } set { m_FaceDirection = value; } }
        public int PayRate { get { return m_PayRate; } set { m_PayRate = value; } }

        #endregion

        [Constructable]
        public MilitarySpawner()
            : this(null)
        {
        }

        [Constructable]
        public MilitarySpawner(GovernmentEntity government) : base()
        {            
            government.MilitarySpawners.Add(this);
            Stationed = false;

            Government = government;
            Armaments = 0;
            Training = 0;
            FaceDirection = (Direction)Utility.Random(8);
            PayRate = 0;            
            
            Name = ("A Military Spawner of " + Government.Name);
            Visible = false;
            Movable = false;

            SetSpawnTime();

            SpawnRange = 0;
            HomeRange = 0;
        }

        public void SetSpawnTime()
        {
            MaxDelay = TimeSpan.FromMinutes(RespawnDelay);

            int zeroCheck = (int)RespawnDelay - (Government.Members.Count * Government.EnemyGuilds.Count );

            if (zeroCheck < 30)
                MinDelay = TimeSpan.FromMinutes(30);
            else
                MinDelay = TimeSpan.FromMinutes(zeroCheck);
        }

        public void AddSoldier(GovernmentEntity g, int type, int training, Direction d, int pay, PlayerMobile from)
        {
            if (Stationed)
            {
                from.SendMessage("There is already a soldier stationed here. You must remove that soldier from duty first.");
                return;
            }

            if (g.Treasury != null && !g.Treasury.Deleted && g.GetTreasuryBalance() >= pay)
            {
                IsInactivated = false;
                MaxCount = 1;
                SetSpawnTime();

                Armaments = type;
                Training = training;
                FaceDirection = d;
                PayRate = pay;
                string govName = g.Name;

                if (Armaments < 1 || Armaments > 6)
                    Armaments = Utility.RandomMinMax(1, 6);

                SpawnString = "";
                SpawnString += ("Soldier," + Government.Nation + "," + Armaments.ToString() + "," + Training.ToString() + "," + govName.ToString());
                SpawnString += ("/direction/" + d.ToString());
                AddSpawn = SpawnString;

                if (PayRate > 0)
                {
                    g.WithdrawFromTreasury(PayRate);
                    from.SendMessage(PayRate.ToString() + " copper has been withdrawn for your treasury to pay for this soldier's training.");
                }
                
                Stationed = true;
            }
            else
                from.SendMessage("You do not have enough funds in your government organization's treasury to deploy that soldier.");
        }

        public static int Salary(int TrainingCost, int ArmsCost, GovernmentEntity Government)
        {
            if (TrainingCost + ArmsCost - Government.ResourceBudgetContribution() < 0)
                return 0;
            else
                return (TrainingCost + ArmsCost - Government.ResourceBudgetContribution());
        }

        public override void Delete()
        {
            if (Government.MilitarySpawners.Contains(this))
                Government.MilitarySpawners.Remove(this);

            Reset();

            base.Delete();
        }

        public override void OnDelete()
        {
            // Removing the guard assigned to this spawner.
            // GET THIS TO HAPPEN ONDELETE FUNCTION OF SOLDIER... AS WELL AS THE ONDEATH???

            if (Government.MilitarySpawners.Contains(this))
                Government.MilitarySpawners.Remove(this);

            Reset();

            base.OnDelete();
        }

        public virtual bool CanSeeMe(PlayerMobile m)
        {
            return ((Government != null && !Government.Deleted && CustomGuildStone.IsGuildMilitary(m, Government)));
        }

        public MilitarySpawner( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
 	        base.Serialize( writer );
            writer.Write( (int)1 );
            writer.Write( (GovernmentEntity)m_Government );
            writer.Write( (byte)m_FaceDirection );
            writer.Write((int)m_PayRate);
            writer.Write((bool)m_Stationed);
        }

        public override void  Deserialize( GenericReader reader )
        {
 	        base.Deserialize( reader );
            int version = reader.ReadInt();
            m_Government = (GovernmentEntity)reader.ReadItem();
            m_FaceDirection = (Direction)reader.ReadByte();
            if (version < 1)
            {
                reader.ReadItem();
                reader.ReadMobile();
            }
            m_PayRate = reader.ReadInt();
            m_Stationed = reader.ReadBool();
        }
    }

    public class AssignWayPointTarget : Target
    {
        private MilitarySpawner m_Spawner;

        public AssignWayPointTarget(MilitarySpawner spawner)
            : base(14, false, TargetFlags.None)
        {
            m_Spawner = spawner;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if ((targeted is MilitaryWayPoint) && !(m_Spawner.Deleted) && (m_Spawner != null))
            {
                m_Spawner.WayPoint = targeted as WayPoint;
                from.SendMessage("You have assigned a WayPoint to your military spawner.");
            }
            else
                from.SendMessage("You may only assign Military WayPoints belonging to your government organization.");

            base.OnTarget(from, targeted);
        }

    }
}