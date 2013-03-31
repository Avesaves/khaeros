using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items; 
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Items
{
	public class CustomGuildStone : Item 
	{
		public static List<CustomGuildStone> Guilds = new List<CustomGuildStone>();
		
		private PlayerMobile m_Owner;
		private string m_OwnersName;
		private List<Mobile> m_Members = new List<Mobile>();
		private List<Mobile> m_Applicants = new List<Mobile>();
		private int m_ArmourHue = -1;
		private int m_ClothingHue = -1;
		private List<Item> m_AlliedGuilds = new List<Item>();
		private List<Item> m_EnemyGuilds = new List<Item>();
		private DateTime m_NextPay;
		private int m_PayCycleInRLDays = 30;
		private Container m_Treasury;
		private bool m_OfficialGuild;
		
		private Dictionary<int, GuildRankInfo> m_Ranks = new Dictionary<int, GuildRankInfo>();
		public Dictionary<int, GuildRankInfo> Ranks
		{ 
			get
			{
				if( m_Ranks.Count < 1 )
					AddNewRank( 1 );
				
				return m_Ranks;
			}
			set{ m_Ranks = value; } 
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool OfficialGuild{ get{ return m_OfficialGuild; } set{ m_OfficialGuild = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int PayCycleInRLDays{ get{ return m_PayCycleInRLDays; } set{ m_PayCycleInRLDays = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextPay{ get{ return m_NextPay; } set{ m_NextPay = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Container Treasury{ get{ return m_Treasury; } set{ m_Treasury = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public PlayerMobile Owner{ get{ return m_Owner; } set{ m_Owner = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string OwnersName{ get{ return m_OwnersName; } set{ m_OwnersName = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
        public int ArmourHue { get { return m_ArmourHue; } set { m_ArmourHue = value; GovernmentEntity.ChangeHue(this, m_ArmourHue, false); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
        public int ClothingHue { get { return m_ClothingHue; } set { m_ClothingHue = value; GovernmentEntity.ChangeHue(this, m_ClothingHue, true); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool PurgeInvalidMembers
		{ 
			get{ return false; }
			set
			{
				if( value == true )
					RemoveMembersWithInvalidRanks();
			}
		}
		
		public void RemoveMembersWithInvalidRanks()
		{
			List<Mobile> toRemove = new List<Mobile>();
			
			foreach( Mobile m in Members )
			{
				if( !((PlayerMobile)m).CustomGuilds.ContainsKey(this) || !Ranks.ContainsKey(((PlayerMobile)m).CustomGuilds[this].RankID) )
					toRemove.Add( m );
			}
			
			for( int i = 0; i < toRemove.Count; i++ )
			{
				Members.Remove( toRemove[i] );
				
				if( ((PlayerMobile)toRemove[i]).CustomGuilds.ContainsKey(this) )
					((PlayerMobile)toRemove[i]).CustomGuilds.Remove( this );

                UpdateTitle( toRemove[i] );
			}
		}
		
		public List<Mobile> Members{ get{ ValidateList( m_Members ); return m_Members; } set{ m_Members = value; } }
		public List<Mobile> Applicants{ get{ ValidateList( m_Applicants ); return m_Applicants; } set{ m_Applicants = value; } }
		
		public List<Item> AlliedGuilds{ get{ return m_AlliedGuilds; } set{ m_AlliedGuilds = value; } }
		public List<Item> EnemyGuilds{ get{ return m_EnemyGuilds; } set{ m_EnemyGuilds = value; } }
		
		public void ValidateList( List<Mobile> mobs )
		{
			List<Mobile> toRemove = new List<Mobile>();
			
			foreach( Mobile m in mobs )
			{
				PlayerMobile pm = m as PlayerMobile;
				
				if( m == null || m.Deleted || !pm.CustomGuilds.ContainsKey(this) )
					toRemove.Add( m );
			}
			
			for( int i = 0; i < toRemove.Count; i++ )
				mobs.Remove( toRemove[i] );
		}
		
		public void UpdateTitles( int rank )
		{
            foreach( Mobile m in Members )
                UpdateTitle( (PlayerMobile)m );
		}
		
		public void UpdateTitle( Mobile m )
		{
            m.InvalidateProperties();
            m.Delta( MobileDelta.Name );
		}
		
		public string ValidateTitleOrPrefix( string titleOrPrefix )
		{
			if( String.IsNullOrEmpty(titleOrPrefix) || titleOrPrefix.ToLower() == "none" )
				return null;
			
			return titleOrPrefix;
		}
		
		public override void OnDelete()
		{
			for( int i = 0; i < Members.Count; i++ )
			{
				Mobile m = Members[i];
				Members.Remove( m );
				
				if( ((PlayerMobile)m).CustomGuilds.ContainsKey(this) )
					((PlayerMobile)m).CustomGuilds.Remove( this );

                UpdateTitle( m );
			}
			
			if( Guilds.Contains(this) )
				Guilds.Remove( this );
			
  			base.OnDelete();
		}
		
		[Constructable]
		public CustomGuildStone() : base( 0xED4 ) 
		{ 
			Movable = false;
        	Hue = 2406;
        	Name = "New Organization";
        	NextPay = DateTime.Now + TimeSpan.FromDays( PayCycleInRLDays );
        	Guilds.Add( this );
		} 

		public override void OnDoubleClick( Mobile m ) 
		{
            if( m != null && this != null && !m.Deleted && !this.Deleted && m is PlayerMobile )
            {
                if( HasViewingRights( (PlayerMobile)m, this ) )
                    m.SendGump( new OrganizationGump( (PlayerMobile)m, this ) );

                else
                    TryToApply( (PlayerMobile)m );
            }
        }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool StartPaying
		{
			get{ return false; }
			set
			{
				if( value == true )
					DoPayment();
			}
		}
		
		public void TryToApply( PlayerMobile m )
		{
			if( IsGuildMember(m, this) )
				m.SendMessage( "You are already a member of this organization." );
			
			else if( Applicants.Contains(m) )
			{
				if( m.CustomGuilds.ContainsKey(this) )
					m.CustomGuilds.Remove( this );
				
				Applicants.Remove(m);
				m.SendMessage( "You removed your application from this organization." );
			}
			
			else if( HasMemberFromAccount(m.Account) )
				m.SendMessage( "You are already a member of this organization on another character of your account." );
			
			else if( HasApplicantFromAccount(m.Account) )
				m.SendMessage( "You are already applying for a position in this organization on another character of your account." );
			
			else
			{
				if( m.CustomGuilds.ContainsKey(this) )
					m.CustomGuilds.Remove( this );
				
				AddGuildInfoTo( m );
				Applicants.Add( m );
				m.SendMessage( "You have applied for a position in this organization." );
			}
		}
		
		public void TryToAccept( PlayerMobile m, PlayerMobile applicant )
		{
			if( IsGuildOfficer(m, this, true) && applicant != null && Applicants.Contains(applicant) && !Members.Contains(applicant) )
			{
				if( Ranks.Count < 1 )
					m.SendMessage( "You need to add ranks to your organization before accepting applicants into it." );
				
				else
				{
					if( !applicant.CustomGuilds.ContainsKey(this) )
						AddGuildInfoTo( applicant );
					
					applicant.CustomGuilds[this].RankID = 1;
					Applicants.Remove( applicant );
					Members.Add( applicant );
                    UpdateTitle( applicant );
					m.SendMessage( "You have accepted " + applicant.CustomGuilds[this].RegistrationName + " into your organization." );
				}
			}
		}
		
		public void TryToDeny( PlayerMobile m, PlayerMobile applicant )
		{
			if( IsGuildOfficer(m, this, true) && applicant != null && Applicants.Contains(applicant) )
			{
				if( applicant.CustomGuilds.ContainsKey(this) )
					applicant.CustomGuilds.Remove( this );

				Applicants.Remove( applicant );
				m.SendMessage( "You have denied their application." );
			}
		}
		
		public void AddGuildInfoTo( PlayerMobile m )
		{
			m.CustomGuilds.Add( this, new CustomGuildInfo() );
			m.CustomGuilds[this].RegistrationName = m.Name;
			m.CustomGuilds[this].GuildStone = this;
		}
		
		public void TryToResign( PlayerMobile m )
		{
			if( !IsGuildMember(m, this) )
				m.SendMessage( "You are not a member of this organization." );
			
			else
			{
				m.CustomGuilds.Remove( this );
				Members.Remove( m );
                UpdateTitle( m );
				m.SendMessage( "You have resigned from this organization." );
			}
		}
		
		public void TryToRemoveMember( PlayerMobile m, PlayerMobile member )
		{
			if( !IsGuildLeader(m, this) || !IsGuildMember(member, this) || !Outranks(m, member, this) )
				return;
			
			member.CustomGuilds.Remove( this );
			Members.Remove( member );
            UpdateTitle( member );
			m.SendMessage( "You have removed them from this organization." );
		}
		
		public void TryToRemoveRank( PlayerMobile m, int rank )
		{
			if( !IsGuildLeader(m, this, true) )
				return;
			
			if( rank == 1 )
			{
				m.SendMessage( "You cannot remove Rank 1." );
				return;
			}
			
			if( Ranks.ContainsKey(rank) )
				Ranks.Remove( rank );
			
			else
				return;
			
			List<GuildRankInfo> list = new List<GuildRankInfo>();
			int count = Ranks.Count + 2;

			for( int i = 1; i < count; i++ )
			{
				if( i > rank && i != rank )
				{
					GuildRankInfo info = Ranks[i];
					info.Rank = i - 1;
					list.Add( info );
					Ranks.Remove( i );
				}
			}
			
			foreach( GuildRankInfo info in list )
				TryToAddRank( info.Rank, info );
			
			LowerRanksFrom( rank );
			RemoveMembersWithInvalidRanks();
		}
		
		public void TryToAddRankAfter( PlayerMobile m, int rank )
		{
			if( !IsGuildLeader(m, this, true) )
				return;
			
			List<GuildRankInfo> list = new List<GuildRankInfo>();
			int count = Ranks.Count + 1;
			
			for( int i = 1; i < count; i++ )
			{
				if( i > rank )
				{
					GuildRankInfo info = Ranks[i];
					info.Rank = i + 1;
					list.Add( info );
					Ranks.Remove( i );
				}
			}
			
			AddNewRank( rank + 1 );
			
			foreach( GuildRankInfo info in list )
				TryToAddRank( info.Rank, info );
			
			RaiseRanksAfter( rank );
			RemoveMembersWithInvalidRanks();
		}
		
		public void RaiseRanksAfter( int rank )
		{
			foreach( Mobile m in Members )
			{
				PlayerMobile pm = m as PlayerMobile;
				
				if( pm.CustomGuilds[this].RankID > rank && Ranks.ContainsKey(pm.CustomGuilds[this].RankID + 1) )
					pm.CustomGuilds[this].RankID++;

                UpdateTitle( m );
			}
		}
		
		public void LowerRanksFrom( int rank )
		{
			foreach( Mobile m in Members )
			{
				PlayerMobile pm = m as PlayerMobile;
				
				if( pm.CustomGuilds[this].RankID >= rank && Ranks.ContainsKey(pm.CustomGuilds[this].RankID - 1) )
					pm.CustomGuilds[this].RankID--;

                UpdateTitle( m );
			}
		}
		
        		
        public void TryToEditRank( PlayerMobile m, GuildRankInfo rank, string name, string title, string prefix, string pay, string fee, string isOfficer)
        {
            if (!IsGuildLeader(m, this, true))
                return;

            if (!Ranks.ContainsKey(rank.Rank))
                return;

            int payVal = 0;
            int feeVal = 0;

            if (!ValidateString(m, name, "Name"))
                return;

            if (!ValidateString(m, pay, "Pay") || !ValidateInt(m, pay, "Pay", ref payVal))
                return;

            if (!ValidateString(m, fee, "Fee") || !ValidateInt(m, fee, "Fee", ref feeVal))
                return;

            if (!ValidateString(m, isOfficer, "Is Officer?") || !ValidateQuestion(m, isOfficer, "Is Officer?"))
                return;
			
			rank.Name = name;
			rank.Title = title;
			rank.Prefix = prefix;
			rank.Pay = payVal;
			rank.Fee = feeVal;
			rank.IsOfficer = isOfficer == "Yes" ? true : false;
			UpdateTitles( rank.Rank );
		}

        //For governments using the IsMilitary boolean and IsEconomic boolean.
		public void TryToEditRank( PlayerMobile m, GuildRankInfo rank, string name, string title, string prefix, string pay, string fee, string isOfficer, string isMilitary, string isEconomic )
		{
            if (!IsGuildLeader(m, this, true))
                return;

            if (!Ranks.ContainsKey(rank.Rank))
                return;

            int payVal = 0;
            int feeVal = 0;

            if (!ValidateString(m, name, "Name"))
                return;

            if (!ValidateString(m, pay, "Pay") || !ValidateInt(m, pay, "Pay", ref payVal))
                return;

            if (!ValidateString(m, fee, "Fee") || !ValidateInt(m, fee, "Fee", ref feeVal))
                return;

            if (!ValidateString(m, isOfficer, "Is Officer?") || !ValidateQuestion(m, isOfficer, "Is Officer?"))
				return;

            if (!ValidateString(m, isMilitary, "Is Military?") || !ValidateQuestion(m, isMilitary, "Is Military?"))
                return;

            if (!ValidateString(m, isEconomic, "Is Economic?") || !ValidateQuestion(m, isEconomic, "Is Economic?"))
                return;
			
			rank.Name = name;
			rank.Title = title;
			rank.Prefix = prefix;
			rank.Pay = payVal;
			rank.Fee = feeVal;
			rank.IsOfficer = isOfficer == "Yes" ? true : false;
            rank.IsMilitary = isMilitary == "Yes" ? true : false;
            rank.IsEconomic = isEconomic == "Yes" ? true : false;
			UpdateTitles( rank.Rank );
		}
		
        //For CustomGuildStones without the IsMilitary boolean.
		public void TryToEditMember( PlayerMobile m, PlayerMobile member, string rank, string activeTitle, string withdraw, string deposit )
		{
			if( (!IsGuildLeader(m, this) || !IsGuildMember(member, this) || !Outranks(m, member, this)) && m != member )
				m.SendMessage( "Your current rank does not allow you to edit this member's status." );

			int rankVal = 0;
			
			if( !ValidateString(m, rank, "Rank") || !ValidateInt(m, rank, "Rank", ref rankVal) )
			   return;

			if( !ValidateString(m, activeTitle, "Active Title?") || !ValidateQuestion(m, activeTitle, "Active Title?") )
				return;
			
			if( !ValidateString(m, withdraw, "Withdraw Funds?") || !ValidateQuestion(m, withdraw, "Withdraw Funds?") )
				return;
			
			if( !ValidateString(m, deposit, "Deposit Funds?") || !ValidateQuestion(m, deposit, "Deposit Funds?") )
				return;
			
			if( !Ranks.ContainsKey(rankVal) )
			{
				m.SendMessage( "This organization has no rank entry for Rank " + rank + "." );
				return;
			}
			
			if( m == member )
			{
				m.CustomGuilds[this].ActiveTitle = activeTitle == "Yes" ? true : false;
				
				if( withdraw == "Yes" )
					TryToWithdraw( m );
				
				if( deposit == "Yes" )
					m.Target = new DepositCopperTarget( m, this );
			}
			
			if( IsGuildLeader(m, this) )
				member.CustomGuilds[this].RankID = rankVal;
			
			UpdateTitle( member );
		}
		
		public bool ValidateString( PlayerMobile m, string st, string name )
		{
			if( String.IsNullOrEmpty(st) )
			{
				m.SendMessage( "Field \"" + name + "\" cannot be empty." );
				return false;
			}
			
			return true;
		}
		
		public bool ValidateInt( PlayerMobile m, string st, string name, ref int parsed )
		{
			if( !int.TryParse(st, out parsed) )
			{
				m.SendMessage( "Field \"" + name + "\" needs to be a valid number." );
				return false;
			}
			
			return true;
		}
		
		public bool ValidateQuestion( PlayerMobile m, string st, string name )
		{
			if( st != "Yes" && st != "No" )
			{
				m.SendMessage( "Field \"" + name + "\" only accepts \"Yes\" or \"No\" as values." );
				return false;
			}
			
			return true;
		}
		
		public void AddNewRank( int rank )
		{
			m_Ranks.Add( rank, new GuildRankInfo() );
			m_Ranks[rank].Rank = rank;
			m_Ranks[rank].Name = "New Rank";
		}
		
		public void TryToAddAlly( PlayerMobile m, CustomGuildStone g )
		{
			if( !IsGuildOfficer(m, this, true) )
				return;
			
			if( !AlliedGuilds.Contains(g) )
				AlliedGuilds.Add( g );
			
			if( EnemyGuilds.Contains(g) )
				EnemyGuilds.Remove( g );
		}
		
		public void TryToAddNeutral( PlayerMobile m, CustomGuildStone g )
		{
			if( !IsGuildOfficer(m, this, true) )
				return;
					
			if( AlliedGuilds.Contains(g) )
				AlliedGuilds.Remove( g );
			
			if( EnemyGuilds.Contains(g) )
				EnemyGuilds.Remove( g );
		}
		
		public void TryToAddEnemy( PlayerMobile m, CustomGuildStone g )
		{
			if( !IsGuildOfficer(m, this, true) )
				return;
			
			if( !EnemyGuilds.Contains(g) )
				EnemyGuilds.Add( g );
			
			if( AlliedGuilds.Contains(g) )
				AlliedGuilds.Remove( g );
		}
		
		public bool HasMemberFromAccount( Account account )
		{
			foreach( Mobile mob in Members )
				if( mob.Account == account )
				return true;
			
			return false;
		}
		
		public bool HasApplicantFromAccount( Account account )
		{
			foreach( Mobile mob in Applicants )
				if( mob.Account == account )
				return true;
			
			return false;
		}
		
		public virtual void DoPayment()
		{
            //ArrayList deleteList = new ArrayList();
            //ArrayList addList = new ArrayList();
            //foreach (Item item in Treasury.Items)
            //{
            //    Copper c = new Copper();

            //    if (item is Silver)
            //        c.Amount = item.Amount * 10;
            //    else if (item is Gold)
            //        c.Amount = item.Amount * 100;

            //    addList.Add(c);
            //    deleteList.Add(item);
            //}
            //foreach (Item copper in addList)
            //{
            //    Treasury.AddItem(copper);
            //}
            //foreach (Item coin in deleteList)
            //{
            //    Treasury.RemoveItem(coin);
            //}

            //int amount = 0;
            //int count = Treasury.Items.Count;
            //List<Item> invalidCoins = new List<Item>();
            //for (int i = 0; i < count; i++) // converting silver and gold to copper
            //{
            //    if (Treasury.Items[i] is Silver)
            //    {
            //        amount += (Treasury.Items[i].Amount * 10);
            //        invalidCoins.Add(Treasury.Items[i];
            //    }
            //    if (Treasury.Items[i] is Gold)
            //    {
            //        amount += (Treasury.Items[i].Amount * 100);
            //        invalidCoins.Add(Treasury.Items[i];
            //    }
            //}

            //if (amount > 0)
            //    Treasury.DropItem(new Copper(amount)); // Dropping converted coins into the treasury.

			for( int i = 0; i < Members.Count; i++ )
			{
				PlayerMobile pm = Members[i] as PlayerMobile;
				
				if( !IsGuildMember(pm, this) || pm.CustomGuilds[this].RankInfo == null )
				{
					if( pm.CustomGuilds.ContainsKey(this) )
						pm.CustomGuilds.Remove( this );
					
					if( Members.Contains(pm) )
						Members.Remove( pm );
				}
				
				pm.CustomGuilds[this].Balance += pm.CustomGuilds[this].RankInfo.Pay - pm.CustomGuilds[this].RankInfo.Fee;
			}
			
			NextPay = DateTime.Now + TimeSpan.FromDays( PayCycleInRLDays );
		}

        //public int WithdrawFromTreasury(int withdrawal)
        //{
        //    if (Treasury == null || Treasury.Deleted)
        //        return 0;

        //    int amount = 0;
        //    ArrayList list = new ArrayList();

        //    foreach (Item item in Treasury.Items)
        //    {
        //        if (item is Copper)
        //        {
        //            amount += item.Amount;
        //            list.Add(item);
        //        }
        //        else if (item is Silver)
        //        {
        //            Copper c = new Copper();
        //            c.Amount = item.Amount * 10;
        //            Treasury.Items.Add(c);
        //            item.Delete();
        //            list.Add(c);
        //            amount += c.Amount;
        //        }
        //        else if (item is Gold)
        //        {
        //            Copper c = new Copper();
        //            c.Amount = item.Amount * 100;
        //            Treasury.Items.Add(c);
        //            item.Delete();
        //            list.Add(c);
        //            amount += c.Amount;
        //        }
        //    }

        //    if (amount < withdrawal)
        //    {
        //        amount = 0;
        //        withdrawal = withdrawal - amount;
        //    }
        //    else
        //        amount -= withdrawal;

        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        Item item = list[i] as Item;
        //        item.Delete();
        //    }

        //    Treasury.AddItem(new Copper(amount));
        //    return withdrawal;
        //}

        public int WithdrawFromTreasury(int withdrawal)
        {
            if (Treasury == null || Treasury.Deleted)
                return 0;

            int amount = 0;

            //int count = Treasury.Items.Count;
            //for (int i = 0; i < count; i++) // converting silver and gold to copper
            //{
            //    if (Treasury.Items[i] is Silver)
            //    {
            //        amount += (Treasury.Items[i].Amount * 10);
            //        Treasury.Items[i].Delete();
            //    }
            //    if (Treasury.Items[i] is Gold)
            //    {
            //        amount += (Treasury.Items[i].Amount * 100);
            //        Treasury.Items[i].Delete();
            //    }
            //}
            //Treasury.DropItem(new Copper(amount)); // Dropping converted coins into the treasury.

            ArrayList addList = new ArrayList();
            ArrayList deleteList = new ArrayList();
  
            foreach (Item item in Treasury.Items)
            {
                if (item is Silver || item is Gold)
                {                
                    Copper c = new Copper();

                    if (item is Silver)
                        c.Amount = item.Amount * 10;
                    else if (item is Gold)
                        c.Amount = item.Amount * 100;

                    addList.Add(c);
                    deleteList.Add(item);
                }
            }
            foreach (Item copper in addList)
            {
                Treasury.AddItem(copper);
            }
            foreach (Item coin in deleteList)
            {
                Treasury.RemoveItem(coin);
            }

            //count = Treasury.Items.Count;
            amount = Treasury.GetAmount(typeof(Copper));

            if (amount < withdrawal)
                withdrawal = withdrawal - amount;

            Treasury.ConsumeUpTo(typeof(Copper), withdrawal);

            return withdrawal;
        }

        public bool HasTreasury(PlayerMobile m)
		{
			return HasTreasury( m, false );
		}
		
		public bool HasTreasury( PlayerMobile m, bool msg )
		{
			if( Treasury == null )
			{
				if( msg )
					m.SendMessage( "Your organization needs to assign a treasury before you can do that." );
				
				return false;
			}
			
			return true;
		}
		
		public override bool OnDragDrop( Mobile from, Item item )
		{
			if( !(from is PlayerMobile) || !IsGuildMember((PlayerMobile)from, this, true) || !HasTreasury((PlayerMobile)from, true) )
				return false;
			
			if( !(item is Copper) && !(item is Silver) && !(item is Gold) && !(item is ForgedCopper) && !(item is ForgedSilver) && !(item is ForgedGold))
			{
				from.SendMessage( "You can only deposit coins into your guild's treasury." );
				return false;
			}
			
			PlayerMobile m = from as PlayerMobile;
			m.CustomGuilds[this].Balance += item.Amount;
			Treasury.DropItem( item );
			
			return true;
		}
		
		public void TryToWithdraw( PlayerMobile m )
		{
			if( !IsGuildMember(m, this, true) || !HasTreasury(m, true) )
				return;
			
			if( m.CustomGuilds[this].Balance < 1 )
			{
				m.SendMessage( "You currently have no funds to withdraw." );
				return;
			}
			
			if( !m.InRange(this, 3) )
			{
				m.SendMessage( "You need to be near your organization's stone in order to do that." );
				return;
			}
			
			int amount = 0;
			ArrayList list = new ArrayList();
			
			foreach( Item item in Treasury.Items )
			{
				if( item is Copper )
				{
					amount += item.Amount;
					list.Add( item );
				}
			}
			
			for( int i = 0; i < list.Count; i++ )
			{
				Item item = list[i] as Item;
				item.Delete();
			}
			
			if( amount == 0 )
			{
				m.SendMessage( "Your organization currently has no funds." );
				return;
			}
			
			if( amount < m.CustomGuilds[this].Balance )
			{
				m.SendMessage( "You withdraw every single coin available, but it is still not enough." );
				m.Backpack.DropItem( new Copper(amount) );
				m.CustomGuilds[this].Balance -= amount;
			}
			
			else
			{
				m.SendMessage( "You withdraw all the copper you are entitled to." );
				m.Backpack.DropItem( new Copper(m.CustomGuilds[this].Balance) );
				amount -= m.CustomGuilds[this].Balance;
				m.CustomGuilds[this].Balance = 0;
				
				if( amount > 0 )
					Treasury.DropItem( new Copper(amount) );
			}
		}
		
		public static bool Outranks( PlayerMobile one, PlayerMobile two, CustomGuildStone g )
        {
        	if( one.AccessLevel > AccessLevel.Player )
        		return true;
        	
        	if( !IsGuildOwner(two, g) )
        	{
        		if( IsGuildOwner(one, g) )
        			return true;
        		
        		if( IsGuildMember(one, g) && IsGuildMember(two, g) )
        		{
        			if( one.CustomGuilds[g].RankID > two.CustomGuilds[g].RankID )
        				return true;
        		}
        	}
        	
        	if( one == two )
        		return true;
        	
        	one.SendMessage( "You cannot change the status of a member of equal or superior rank in an organization." );
        	return false;
        }

        public static bool IsGuildMember(PlayerMobile m, CustomGuildStone g)
        {
        	return IsGuildMember( m, g, false );
        }
        
        public static bool IsGuildMember( PlayerMobile m, CustomGuildStone g, bool msg )
        {
        	if( g.Members.Contains(m) && m.CustomGuilds.ContainsKey(g) )
        		return true;
        	
        	if( msg )
        		m.SendMessage( "You do not have access to that function because you are not a member this organization." );
        	
        	return false;
        }

        public static bool IsGuildOfficer( PlayerMobile m, CustomGuildStone g )
        {
        	return IsGuildOfficer( m, g, false );
        }
        
        public static bool IsGuildOfficer( PlayerMobile m, CustomGuildStone g, bool msg )
        {
        	if( m.AccessLevel > AccessLevel.Player )
        		return true;
        	
        	if( (IsGuildMember(m, g) && m.CustomGuilds[g].RankInfo != null && m.CustomGuilds[g].RankInfo.IsOfficer) || IsGuildOwner( m, g ) )
        		return true;

            if (IsGuildMilitary(m, g))
                return true;
        	
        	if( msg )
        		m.SendMessage( "You do not have access to that function because you are not an officer of this organization." );
        	
        	return false;
        }

        public static bool IsGuildMilitary(PlayerMobile m, CustomGuildStone g)
        {
            if (m.AccessLevel > AccessLevel.Player)
                return true;

            if( (IsGuildMember(m, g) && ((m.CustomGuilds[g].RankInfo != null && m.CustomGuilds[g].RankInfo.IsMilitary) || IsGuildOwner(m,g) || IsGuildLeader(m,g))))
                return true;

            return false;
        }

        public static bool IsGuildEconomic(PlayerMobile m, CustomGuildStone g)
        {
            if (m.AccessLevel > AccessLevel.Player)
                return true;

            if (IsGuildLeader(m, g))
                return true;

            if (IsGuildMember(m, g) && m.CustomGuilds[g].RankInfo != null && m.CustomGuilds[g].RankInfo.IsEconomic)
                return true;

            return false;
        }
        
        public static bool IsGuildLeader( PlayerMobile m, CustomGuildStone g )
        {
        	return IsGuildLeader( m, g, false );
        }
        
        public static bool IsGuildLeader( PlayerMobile m, CustomGuildStone g, bool msg )
        {
        	if( m.AccessLevel > AccessLevel.Player )
        		return true;
        	
        	if( IsGuildOwner(m, g) )
        		return true;
        	
        	if( IsGuildMember(m, g) && m.CustomGuilds[g].RankID == g.Ranks.Count )
        		return true;

        	if( msg )
        		m.SendMessage( "You do not have access to that function because you are not one of the leaders of this organization." );
        	
        	return false;
        }

        public static bool IsGuildOwner( PlayerMobile m, CustomGuildStone g )
        {
        	if( m.AccessLevel > AccessLevel.Player )
        		return true;

            if (g.Owner == null)
                return false;
        	
        	if( g.Owner == m )
        		return true;
        	
        	return false;
        }

        public static bool HasViewingRights( PlayerMobile m, CustomGuildStone g )
        {
            return HasViewingRights( m, g, false );
        }
        
        public static bool HasViewingRights( PlayerMobile m, CustomGuildStone g, bool msg )
        {
        	if( m.AccessLevel > AccessLevel.Player )
        		return true;

            if( IsGuildOwner( m, g ) )
                return true;

        	return IsGuildMember( m, g, msg );
        }
		
  		public CustomGuildStone( Serial serial ) : base( serial ) 
  		{ 
 		}
  		
  		public void TryToAddRank( int rank, GuildRankInfo info )
  		{
  			if( m_Ranks.ContainsKey(rank) )
  				Console.WriteLine( "Duplicated key \"" + rank.ToString() + "\" on Guildstone \"" + Serial.ToString() + "\"." );
  			
  			else
  				m_Ranks.Add( rank, info );
  		}

 		public override void Serialize( GenericWriter writer ) 
  		{
 			base.Serialize( writer );
     		writer.Write( (int) 0 ); // version
     		
     		writer.Write( (int) Ranks.Count );
     		
     		foreach( KeyValuePair<int, GuildRankInfo> kvp in Ranks )
     		{
     			writer.Write( (int) kvp.Key );
     			GuildRankInfo.Serialize( writer, kvp.Value );
     		}
     		
     		writer.Write( (List<Item>) m_AlliedGuilds );
     		writer.Write( (List<Item>) m_EnemyGuilds );
     		writer.Write( (int) m_PayCycleInRLDays );
     		writer.Write( (Item) m_Treasury );
     		writer.Write( (DateTime) m_NextPay );
     		writer.Write( (bool) m_OfficialGuild );
     		writer.Write( (int) m_ArmourHue );
     		writer.Write( (int) m_ClothingHue );
     		writer.Write( m_Owner );
     		writer.Write( (string) m_OwnersName );
     		writer.Write( m_Members );
     		writer.Write( m_Applicants );
  		} 

  		public override void Deserialize( GenericReader reader ) 
  		{
  			base.Deserialize( reader );
     		int version = reader.ReadInt();

     		int count = reader.ReadInt();
     		
     		for( int i = 0; i < count; i++ )
     			TryToAddRank( reader.ReadInt(), new GuildRankInfo(reader) );
     		
 			m_AlliedGuilds = (List<Item>)reader.ReadStrongItemList();
 			m_EnemyGuilds = (List<Item>)reader.ReadStrongItemList();
 			m_PayCycleInRLDays = reader.ReadInt();
 			m_Treasury = (Container)reader.ReadItem();
 			m_NextPay = reader.ReadDateTime();
 			m_OfficialGuild = reader.ReadBool();
     		m_ArmourHue = reader.ReadInt();
     		m_ClothingHue = reader.ReadInt();
     		m_Owner = (PlayerMobile)reader.ReadMobile();
     		m_OwnersName = reader.ReadString();
     		m_Members = reader.ReadStrongMobileList();
     		m_Applicants = reader.ReadStrongMobileList();
     		
     		Guilds.Add( this );
            CustomGuildStone.CheckMemberActivity(this);
  		}
  		
  		public static void Initialize()
		{
  			Timer.DelayCall( TimeSpan.FromMinutes( 10.0 ), new TimerCallback( CheckForPayment ) );
  		}
  		
  		public static void CheckForPayment()
  		{
  			foreach( CustomGuildStone guild in Guilds )
  			{
  				if( DateTime.Compare(DateTime.Now, guild.NextPay) > 0 )
  					guild.DoPayment();
  			}
  			
  			Timer.DelayCall( TimeSpan.FromMinutes( 10.0 ), new TimerCallback( CheckForPayment ) );
  		}

        public static void CheckMemberActivity(CustomGuildStone g)
        {
            List<PlayerMobile> inactiveList = new List<PlayerMobile>();

            foreach (Mobile m in g.Members)
            {
                if (m is PlayerMobile)
                {
                    PlayerMobile pm = m as PlayerMobile;

                    if (pm.LastOnline + TimeSpan.FromDays(45) < DateTime.Now)
                        inactiveList.Add(pm);
                }
            }

            int count = inactiveList.Count;
            for (int i = 0; i < count; i++)
                g.Members.Remove(inactiveList[i]);
        }
   	} 
} 
