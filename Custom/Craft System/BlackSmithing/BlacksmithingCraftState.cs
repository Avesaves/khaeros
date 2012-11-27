using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Gumps;

namespace Server.Engines.Craft
{
    public class BlackSmithingCraftState : CraftState
    {
        private BaseAssemblyPiece m_BasePiece;        
        private CraftResource m_Resource;        

        private int m_Heat;
        private int m_Work;
        private int m_HeatTurns;
        private double m_MalabilityHeat;

        private double m_Complete;
        private int m_TotalComplete;
		private int m_Round;
        private BlackSmithingTimer m_Timer;
        private int m_TotalTurns;
        private bool m_TurnUsed = false;
        private bool m_WorkUsed = false;

        private int m_ImproveCount;
        private int m_ImproveMax;
        private int m_DamageCount;
        private int m_QuenchCount;

        private ArrayList m_QualityCount;

        public override Type ToolType { get { return typeof(BlackSmithingHammer); } }

        public BaseAssemblyPiece Piece
        {
            get { return m_BasePiece; }
            set { m_BasePiece = value; }
        }

        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; }
        }

        public int Work{ get { return m_Work; } }
        public int Heat { get { return m_Heat; } set { m_Heat = value; } }

        public int HeatTurns
        {
            get { return m_HeatTurns; }
            set { m_HeatTurns = value; }
        }

        public double Complete { get { return m_Complete; } }
        public int TotalComplete { get { return m_TotalComplete; } }
		public int Round { get { return m_Round; } }
        public int TotalTurns 
        {
            get { return m_TotalTurns; }
            set { m_TotalTurns = value; } 
        }

        public bool TurnUsed
        {
            get { return m_TurnUsed; }
            set { m_TurnUsed = value; }
        }

        public BlackSmithingCraftState(Mobile crafter, BaseTool tool)
            : base(crafter, tool, 1)
        {
            m_QualityCount = new ArrayList();
        }        

        public void SetResourceType(CraftResource resource)
        {            
            m_Resource = resource;
            m_HeatTurns = 15;
            m_Heat = SetHeat();

            switch (m_Resource)
            {
                case CraftResource.Copper: m_MalabilityHeat = 500; m_ImproveMax = 16; break;
                case CraftResource.Bronze: m_MalabilityHeat = 550; m_ImproveMax = 18; break;
                case CraftResource.Iron: m_MalabilityHeat = 600; m_ImproveMax = 20; break;
                case CraftResource.Steel: m_MalabilityHeat = 650; m_ImproveMax = 24; break;
                case CraftResource.Obsidian: m_MalabilityHeat = 700; m_ImproveMax = 28; break;
                case CraftResource.Starmetal: m_MalabilityHeat = 750; m_ImproveMax = 32; break;
                case CraftResource.Silver: m_MalabilityHeat = 600; m_ImproveMax = 5; break;
                case CraftResource.Gold: m_MalabilityHeat = 600; m_ImproveMax = 5; break;
            }            
        }

        public void SetPieceType(BaseAssemblyPiece piece)
        {
            m_BasePiece = piece;
            m_TotalComplete = piece.ResourceAmount;             
        }

        public void StartCraftState()
        {
            m_BasePiece.Resource = m_Resource;
            m_TotalComplete = m_BasePiece.ResourceAmount;
            m_HeatTurns = 15;
            m_Heat = SetHeat();
            Crafter.SendGump(new BlacksmithingGump((PlayerMobile)Crafter, this));            
            m_Timer = new BlackSmithingTimer(Crafter, this);
            m_Timer.Start();
        }

        public void EndCraftState()
        {
            if (ConsumeResources())
            {

                if (m_WorkUsed)
                {
                    Crafter.SendMessage("You have completed your work and crafted the item");
					int quality = 0;
                    foreach (int c in m_QualityCount)
                        quality += c;
                    m_BasePiece.Quality = quality / m_QualityCount.Count;
					PlayerMobile m = Crafter as PlayerMobile;
                    Misc.LevelSystem.AwardExp( m, Math.Max( ( m_BasePiece.Quality ), 100 ) );
					Misc.LevelSystem.AwardCP( m, Math.Max( ( m_BasePiece.Quality / 5), 20 ) );
                    if (m_BasePiece.Durability > 200 + ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.DurableCrafts) * 100)
                        m_BasePiece.Durability = 200 + ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.DurableCrafts) * 100;
                    
                
                    Crafter.AddToBackpack(m_BasePiece);
                    m_BasePiece.InvalidateProperties();
                }
                else
                {
                    Crafter.SendMessage("You didn't work this item enough and you only are left with a metal rod");
                    PigIron item = new PigIron();
                    Crafter.AddToBackpack(item);
                }

            }
            else
            {
                Crafter.SendLocalizedMessage(502925); // You don't have the resources required to make that item.
            }
            if (Tool.UsesRemaining <= 0)
                Tool.Delete();
            m_Timer.Stop();            
            return;
        }

        public override bool ConsumeResources()
        {
            bool consumed = false;
            switch (m_Resource)
            {
                case CraftResource.Copper: consumed = Crafter.Backpack.ConsumeTotal(typeof(CopperIngot), m_TotalComplete); break;
                case CraftResource.Bronze: consumed = Crafter.Backpack.ConsumeTotal(typeof(BronzeIngot), m_TotalComplete); break;
                case CraftResource.Iron: consumed = Crafter.Backpack.ConsumeTotal(typeof(IronIngot), m_TotalComplete); break;
                case CraftResource.Steel: consumed = Crafter.Backpack.ConsumeTotal(typeof(SteelIngot), m_TotalComplete); break;
                case CraftResource.Obsidian: consumed = Crafter.Backpack.ConsumeTotal(typeof(ObsidianIngot), m_TotalComplete); break;
                case CraftResource.Starmetal: consumed = Crafter.Backpack.ConsumeTotal(typeof(StarmetalIngot), m_TotalComplete); break;
                case CraftResource.Silver: consumed = Crafter.Backpack.ConsumeTotal(typeof(SilverIngot), m_TotalComplete); break;
                case CraftResource.Gold: consumed = Crafter.Backpack.ConsumeTotal(typeof(GoldIngot), m_TotalComplete); break;
            }
            return consumed;
        }

        public void DoWork( int level )
        {
            int w = 0;
            switch (level)
            {
                case 1: w = 10; break;
                case 2: w = 15; break;
                case 3: w = 20; break;
            }

            m_Work += (int)(w * 1.5);
            if (m_Work > 200)
                m_WorkUsed = true;
            if (m_Work > 500)
                DoDamage();
            m_Complete += w * 0.004;

            m_HeatTurns += 1;
            m_Heat = SetHeat();

            CheckImprovement();
            if (w == 20)
                CheckImprovement();
            ComputeQuality();
            TurnUsed = true;
            return;
        }

        public int SetHeat()
        {
			if (m_HeatTurns < 0)
				return (int)(1000 + Math.Pow(m_HeatTurns, 2));
			else
				return (int)(1000 - Math.Pow(m_HeatTurns, 2));
        }

        public void AddHeat( int level )
        {
            m_HeatTurns -= level;
            m_Heat = SetHeat();

            int w = 0;
            switch (level)
            {
                case 1: w = 10; break;
                case 2: w = 15; break;
                case 3: w = 20; CheckImprovement(); break;
            }

            m_Complete += w * 0.004;
            TurnUsed = true;
            ComputeQuality();
            return;
        }

        public bool HeatCheck()
        {
            if (m_Heat < 200)
            {
                Crafter.SendMessage("The metal got too cold and the piece is left deformed and unworkable.");
                OnFailure();
                return false;
            }
            if (m_Heat > 800)
            {
                if (m_Heat > 1000)
                {
                    Crafter.SendMessage("The metal grows too hot and melts into an unusable liquid");
                    OnFailure();
                    return false;
                }
                else
                {
                    double c = (m_Heat - 800) / 200;
                    if (c * .25 > Utility.RandomDouble())
                        DoDamage();
                    return true;
                }                
            }
            return true;
        }

        public void SkipTurn()
        {
            int quality = 100;
            m_QualityCount.Add(quality);
            quality = 0;
            foreach (int c in m_QualityCount)
                quality += c;

            m_BasePiece.Quality = quality / m_QualityCount.Count;
            return;
        }

        public void CheckImprovement()
        {
            int Ifactor = m_Heat + m_Work;

            if (Ifactor == 775)
                DoImprovement();
            else
            {
                double num = (Math.Abs(775 - Ifactor));
                double i = num / 775;
                double chance = 1 - (i * 2);
                if (chance <= 0)
                    chance = 0.01;                

                if (m_BasePiece is BaseArmorPiece)
                    chance *= ((((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.ArmourSmithing) + ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Blacksmithing)) * 0.18);
                else if (m_BasePiece is BaseWeaponPiece)
                    chance *= ((((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.WeaponSmithing) + ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Blacksmithing)) * 0.18); //Crafter.SendMessage("Weapon chance: " + chance.ToString());
                if (chance >= 1)
                    chance = 1;
                if (chance >= Utility.RandomDouble())
                {
                    if (chance / m_TotalComplete >= Utility.RandomDouble() || m_Resource == CraftResource.Starmetal || (m_Resource == CraftResource.Obsidian && 1 / m_TotalComplete > Utility.RandomDouble()))
                    {
                        DoImprovement();
                        return;
                    }
                }
                if (Utility.RandomDouble() > chance )
                    DoDamage();                             
            }
            return;
        }

        public void DoImprovement()
        {
            
            if (!Utility.RandomBool() && ((PlayerMobile)Crafter).CraftingSpecialization != "Blacksmithing" )
                return;            
            if (m_ImproveCount >= m_ImproveMax)
                return;

            m_ImproveCount++; m_DamageCount--;
            if (Utility.RandomBool() || m_Heat > m_MalabilityHeat)
            {                
                int i = 250 + (((((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Masterwork) + ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.RenownedMasterwork)) * 50) - 50);
                for(int x = 0; x < m_ImproveCount; x++)
                    m_QualityCount.Add(i);
            }

            if (m_BasePiece is BaseWeaponPiece)
                DoWeaponImprovement();
            if (m_BasePiece is BaseArmorPiece)
                DoArmorImprovement();

            return;
        }

        public void DoWeaponImprovement()
        {
            BaseWeaponPiece weapon = m_BasePiece as BaseWeaponPiece;
            bool dmg = Utility.RandomBool();

            if (!dmg)
            {
                if (weapon is BaseAttackPiece)
                {
                    weapon.Speed++;
                    Crafter.SendMessage("Speed increases.");
                }
                else
                {
                    m_ImproveCount++;
                    weapon.Defense++;
                    Crafter.SendMessage("Defense chance increases");
                }
            }
            else
            {
                if (weapon is BaseAttackPiece)
                {
                    weapon.Damage++;
                    Crafter.SendMessage("Damage increases");
                }
                else
                {
                    m_ImproveCount++;
                    weapon.Attack++;
                    Crafter.SendMessage("Hit chance increases");
                }
            }

            if (((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Damage) * 0.1 >= Utility.RandomDouble())
            {
                weapon.Damage += 1;
                m_ImproveCount++;
            }
            if (((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Speed) * 0.1 >= Utility.RandomDouble())
            {
                weapon.Speed += 1;
                m_ImproveCount++;
            }
            if (((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.HCI) * 0.1 >= Utility.RandomDouble())
            {
                weapon.Attack += 1;
                m_ImproveCount++;
            }
            if (((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.DCI) * 0.1 >= Utility.RandomDouble())
            {
                weapon.Defense += 1;
                m_ImproveCount++;
            }
            weapon.InvalidateProperties();
            return;
        }

        public void DoArmorImprovement()
        {
            m_ImproveCount += 2;
            BaseArmorPiece armor = m_BasePiece as BaseArmorPiece;
            int rst = Utility.RandomMinMax(1, 3);
            switch (rst)
            {
                case 1: armor.Blunt += 3; break;
                case 2: armor.Slash += 3; break;
                case 3: armor.Pierce += 3; break;
            }

            if (((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Blunt) * 0.1 >= Utility.RandomDouble())
            {
                armor.Blunt += ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Blunt);
                m_ImproveCount += ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Blunt);
            }

            if (((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Slashing) * 0.1 >= Utility.RandomDouble())
            {
                armor.Slash += ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Slashing);
                m_ImproveCount += ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Slashing);
            }

            if (((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Piercing) * 0.1 >= Utility.RandomDouble())
            {
                armor.Pierce += ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Piercing);
                m_ImproveCount += ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Piercing);
            }
            armor.InvalidateProperties();
            return;
        }

        public void DoDamage()
        {
            
            if (Utility.RandomBool() && m_Heat < m_MalabilityHeat)
                m_ImproveMax--;

            if (m_ImproveMax < m_ImproveCount)
            {
                if (m_ImproveMax < 0)
                {
                    Crafter.SendMessage("You have damaged this to the point that it is no longer workable and lose your material");
                    OnFailure();
                }
                m_ImproveCount--;
                if (m_BasePiece is BaseWeaponPiece)
                    DoWeaponDamage((BaseWeaponPiece)m_BasePiece);
                else
                    DoArmorDamage((BaseArmorPiece)m_BasePiece);
            }
            Crafter.SendMessage("You have damaged the item but it is still workable");
            m_DamageCount++;
            m_BasePiece.Durability -= (int)(m_BasePiece.Durability * 0.05);
            if (m_BasePiece.Durability <= 0)
                m_BasePiece.Durability = 10;
            
            int y = ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.RenownedMasterwork);
            if (((PlayerMobile)Crafter).CraftingSpecialization == "Blacksmithing")
                y += ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.CraftingSpecialization);
            if (y < 1)
                y = 1;
            for (int i = 0; i < m_DamageCount; i += y)
            {
                m_QualityCount.Add(50);
            }            
            return;
        }

        public void DoWeaponDamage(BaseWeaponPiece piece)
        {
            for (int i = 1; i <= 4; i++)
            {
                bool adjusted = false;
                switch (Utility.Random(4))
                {
                    case 0:
                        if (piece.Damage > 0)
                        {
                            piece.Damage--;
                            adjusted = true;
                        }
                        break;
                    case 1:
                        if (piece.Speed > 0)
                        {
                            piece.Speed--;
                            adjusted = true;
                        }
                        break;
                    case 2:
                        if (piece.Defense > 0)
                        {
                            piece.Defense--;
                            adjusted = true;
                        }
                        break;
                    case 3:
                        if (piece.Attack > 0)
                        {
                            piece.Attack--;
                            adjusted = true;
                        }
                        break;
                }
                if (adjusted)
                    break;
            }
        }

        public void DoArmorDamage(BaseArmorPiece piece)
        {
            for (int i = 1; i <= 3; i++)
            {
                bool adjusted = false;
                switch (Utility.Random(3))
                {
                    case 0:
                        if (piece.Blunt > 0)
                        {
                            piece.Blunt--;
                            adjusted = true;
                        }
                        break;
                    case 1:
                        if (piece.Slash > 0)
                        {
                            piece.Slash--;
                            adjusted = true;
                        }
                        break;
                    case 2:
                        if (piece.Pierce > 0)
                        {
                            piece.Pierce--;
                            adjusted = true;
                        }
                        break;                    
                }
                if (adjusted)
                    break;
            }
        }

        public void OnFailure()
        {
            m_TotalComplete = (int)m_Complete;
            bool b = ConsumeResources();
            m_Timer.Stop();            
            return;
        }

        public void ComputeQuality()
        {            
            double exp = ((m_TotalTurns * 0.066667 ));            
            double goal = (Math.Pow(Math.E, exp));
            exp = m_Complete ;
            double attempt = (Math.Pow(Math.E, exp));            
            int quality, i;            

            if (attempt == goal)
                quality = 500;
            else
            {
                double error = Math.Abs(goal - attempt) / goal;  
                double scalar = 1 - error;                
                if (scalar <= 0)
                    scalar = 0.002;                
                quality = (int)(scalar * 450);                
            }

            i = 250 + (((((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Masterwork) + ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.RenownedMasterwork)) * 50) - 50);
            if (m_BasePiece is BaseWeaponPiece)
                if (((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.WeaponSmithing) * 0.066667 > Utility.RandomDouble())
                    quality += 10 * ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.WeaponSmithing);
            else if (m_BasePiece is BaseArmorPiece)
                if (((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.ArmourSmithing) * 0.066667 > Utility.RandomDouble())
                    quality += 10 * ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.ArmourSmithing);
            if (m_Heat > m_MalabilityHeat && Utility.RandomBool())
                quality += 50;
            if (((PlayerMobile)Crafter).CraftingSpecialization == "Blacksmithing" && ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.CraftingSpecialization) * 0.0166667 > Utility.RandomDouble())
                m_QualityCount.Add(i);
                        
            if (quality > i)
               quality = i;

            if (quality > 450 && (Utility.RandomDouble() <= 0.2 || (m_Heat > m_MalabilityHeat && Utility.RandomDouble() <= 0.3)))
                DoImprovement();
			
			if (m_TotalTurns == 0)
				quality = 250;

            m_QualityCount.Add(quality);
            quality = 0;
            foreach (int c in m_QualityCount)
                quality += c;

            m_BasePiece.Quality = quality / m_QualityCount.Count;                
           
            return;
        }

        public void DoQuench()
        {
            double modifier = ((m_Heat - 200) * 0.001);            
            TurnUsed = true;            

            if ((m_Heat < 300 && Utility.RandomBool()) || (m_Heat > m_MalabilityHeat && Utility.RandomBool()))
            {
                Crafter.SendMessage("The metal quenches perfectly and the quality shows");
                m_BasePiece.Durability += (int)(m_Work * .1 + modifier * 250);
                m_BasePiece.Durability += 5 * ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.DurableCrafts);
                QuenchCompletionCompute();
                int q = 250 + (((((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.Masterwork) + ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.RenownedMasterwork)) * 50) - 50);
                m_QuenchCount++;
                int y = (int)(m_DamageCount / 2);
                if (y < 1)
                    y = 1;
                m_DamageCount -= y;
                y = ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.RenownedMasterwork);
                if (((PlayerMobile)Crafter).CraftingSpecialization == "Blacksmithing")
                    y += ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.CraftingSpecialization);
                for (int i = 0; i < m_QuenchCount; i += (7 - y))
                {
                    m_QualityCount.Add(q);
                }
                DoImprovement();
                
                return;
            }
            else if (modifier < Utility.RandomDouble())
            {
                m_BasePiece.Durability += (int)(m_Work * .1 + modifier * 100);
                m_BasePiece.Durability += 2 * ((IKhaerosMobile)Crafter).Feats.GetFeatLevel(FeatList.DurableCrafts);
                QuenchCompletionCompute();                
                return;
            }
            else if (Utility.RandomBool())
            {
                Crafter.SendMessage("The sudden drop in temperature causes some bubbling in the metal but the piece can still be worked");
                QuenchCompletionCompute();
                DoDamage();
                if (m_Complete >= m_TotalComplete)
                    EndCraftState();
                return;
            }
            else
            {
                Crafter.SendMessage("The sudden drop in temperature causes the metal to form bubbles and cracks, rendering this piece useless");
                OnFailure();
                return;
            }
        }

        public void QuenchCompletionCompute()
        {            
            if (m_Complete >= 9)
                m_Complete = 10;
            else if (m_Complete >= 8 && m_Complete < 9)
                m_Complete = 9;
            else if (m_Complete >= 7 && m_Complete < 8)
                m_Complete = 8;
            else if (m_Complete >= 6 && m_Complete < 7)
                m_Complete = 7;
            else if (m_Complete >= 5 && m_Complete < 6)
                m_Complete = 6;
            else if (m_Complete >= 4 && m_Complete < 5)
                m_Complete = 5;
            else if (m_Complete >= 3 && m_Complete < 4)
                m_Complete = 4;
            else if (m_Complete >= 2 && m_Complete < 3)
                m_Complete = 3;
            else if (m_Complete >= 1 && m_Complete < 2)
                m_Complete = 2;
            else if (m_Complete < 1)			
                m_Complete = 1;
				
			m_Round = (int)m_Complete;
            if (m_Complete < m_TotalComplete)
                m_WorkUsed = false;            
            
            ComputeQuality();
            
            m_HeatTurns = 15;
            m_Heat = SetHeat();
            m_Work = 0;
            return;
        }
    }    

    public class BlackSmithingTimer : Timer
    {
        private BlackSmithingCraftState m_Owner;
        private Mobile m_Crafter;

        public BlackSmithingTimer( Mobile crafter, BlackSmithingCraftState from )
            : base(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5))
        {
            m_Owner = from;
            m_Crafter = crafter;
        }

        protected override void OnTick()
        {
            m_Owner.TotalTurns++;
            if (m_Owner.HeatCheck())
            {
                if (!m_Owner.TurnUsed)
                {
                    m_Crafter.CloseGump(typeof(BlacksmithingGump));
                    m_Owner.SkipTurn();
                    m_Owner.HeatTurns++;
                    m_Owner.Heat = m_Owner.SetHeat();
                }

                if (m_Owner.Complete >= m_Owner.TotalComplete)
                {
                    m_Owner.EndCraftState();                    
                    this.Stop();
                }
                else
                {
                    m_Owner.TurnUsed = false;
                    m_Crafter.SendGump(new BlacksmithingGump((PlayerMobile)m_Crafter, m_Owner));
                }
            }           
               
        }
    }
}
