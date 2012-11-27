using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server.Gumps
{
	public class MasterworkEquipGump : Gump
	{
		
		public MasterworkEquipGump( PlayerMobile m )
			: base( 0, 0 )
		{
			m.CloseGump( typeof( MasterworkEquipGump ) );
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			
			this.AddPage(0);
			this.AddBackground(54, 31, 400, 383, 9270);
			this.AddBackground(71, 192, 364, 202, 3500);
			this.AddImage(4, 10, 10440);
			this.AddImage(423, 10, 10441);
            this.AddImage(183, 50, 29);
            //this.AddImage(215, 80, 9000);
			this.AddLabel(104, 48, 2010, @"Armour Points: " + m.Masterwork.ArmourPointsLeft + "          Weapon Points: " + m.Masterwork.WeaponPointsLeft );
			this.AddLabel(116, 82, 1149, @"Blunt Resist");
			this.AddLabel(116, 117, 1149, @"Slashing Resist");
			this.AddLabel(116, 152, 1149, @"Piercing Resist");
			this.AddLabel(297, 82, 1149, @"Weapon Damage");
			this.AddLabel(305, 117, 1149, @"Weapon Speed");
			this.AddLabel(287, 152, 1149, @"Weapon Accuracy");
			this.AddButton(404, 46, 1150, 1152, 0, GumpButtonType.Reply, 0);
			this.AddButton(96, 85, 5600, 5604, 1, GumpButtonType.Reply, 0);
			this.AddButton(96, 120, 5600, 5604, 2, GumpButtonType.Reply, 0);
			this.AddButton(96, 155, 5600, 5604, 3, GumpButtonType.Reply, 0);
			this.AddButton(415, 85, 5600, 5604, 4, GumpButtonType.Reply, 0);
			this.AddButton(415, 120, 5600, 5604, 5, GumpButtonType.Reply, 0);
			this.AddButton(415, 155, 5600, 5604, 6, GumpButtonType.Reply, 0);
			this.AddButton(75, 85, 9764, 9765, 7, GumpButtonType.Reply, 0);
			this.AddButton(75, 120, 9764, 9765, 8, GumpButtonType.Reply, 0);
			this.AddButton(75, 155, 9764, 9765, 9, GumpButtonType.Reply, 0);
			this.AddButton(395, 85, 9764, 9765, 10, GumpButtonType.Reply, 0);
			this.AddButton(395, 120, 9764, 9765, 11, GumpButtonType.Reply, 0);
			this.AddButton(395, 155, 9764, 9765, 12, GumpButtonType.Reply, 0);
			this.AddHtml( 99, 219, 307, 147, "Blunt Resist: " + m.Masterwork.BluntResist +
			             "<br>Slashing Resist: " + m.Masterwork.SlashingResist +
			             "<br>Piercing Resist: " + m.Masterwork.PiercingResist +
			             "<br>Weapon Damage: " + m.Masterwork.WeaponDamage +
			             "<br>Weapon Speed: " + m.Masterwork.WeaponSpeed +
			             "<br>Weapon Accuracy: " + m.Masterwork.WeaponAccuracy, (bool)true, (bool)true);
		}
		
		public override void OnResponse(NetState sender, RelayInfo info)
		{
			PlayerMobile m = sender.Mobile as PlayerMobile;
			
			if (m == null)
				return;

			switch ( info.ButtonID )
			{
					
				case 0:
				{
					break;
				}
					
				case 1: 
				{
					if( m.Masterwork.ArmourPointsLeft > 0 )
					{
						m.Masterwork.BluntResist ++;
						m.Masterwork.ArmourPointsLeft --;
					}
					break;
				}

				case 2: 
				{
					if( m.Masterwork.ArmourPointsLeft > 0 )
					{
						m.Masterwork.SlashingResist ++;
						m.Masterwork.ArmourPointsLeft --;
					}
					break;
				}

				case 3: 
				{
					if( m.Masterwork.ArmourPointsLeft > 0 )
					{
						m.Masterwork.PiercingResist ++;
						m.Masterwork.ArmourPointsLeft --;
					}
					break;
				}
					
				case 4: 
				{
					if( m.Masterwork.WeaponPointsLeft > 0 )
					{
						m.Masterwork.WeaponDamage ++;
						m.Masterwork.WeaponPointsLeft --;
					}
					break;
				}
					
				case 5: 
				{
					if( m.Masterwork.WeaponPointsLeft > 0 )
					{
						m.Masterwork.WeaponSpeed ++;
						m.Masterwork.WeaponPointsLeft --;
					}
					break;
				}
					
				case 6: 
				{
					if( m.Masterwork.WeaponPointsLeft > 0 )
					{
						m.Masterwork.WeaponAccuracy ++;
						m.Masterwork.WeaponPointsLeft --;
					}
					break;
				}
					
				case 7: 
				{
					if ( m.Masterwork.BluntResist > 0 )
					{
						m.Masterwork.BluntResist --;
						m.Masterwork.ArmourPointsLeft ++;
					}
					break;
				}
					
				case 8: 
				{
					if ( m.Masterwork.SlashingResist > 0 )
					{
						m.Masterwork.SlashingResist --;
						m.Masterwork.ArmourPointsLeft ++;
					}
					break;
				}
					
				case 9: 
				{
					if ( m.Masterwork.PiercingResist > 0 )
					{
						m.Masterwork.PiercingResist --;
						m.Masterwork.ArmourPointsLeft ++;
					}
					break;
				}
					
				case 10: 
				{
					if ( m.Masterwork.WeaponDamage > 0 )
					{
						m.Masterwork.WeaponDamage --;
						m.Masterwork.WeaponPointsLeft ++;
					}
					break;
				}
					
				case 11: 
				{
					if ( m.Masterwork.WeaponSpeed > 0 )
					{
						m.Masterwork.WeaponSpeed --;
						m.Masterwork.WeaponPointsLeft ++;
					}
					break;
				}
					
				case 12: 
				{
					if ( m.Masterwork.WeaponAccuracy > 0 )
					{
						m.Masterwork.WeaponAccuracy --;
						m.Masterwork.WeaponPointsLeft ++;
					}
					break;
				}
			}
			
			if ( info.ButtonID != 0 )
			{
				m.SendGump( new MasterworkEquipGump( m ) );	
			}
		}
	}
}
