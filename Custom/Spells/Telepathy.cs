using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Engines.XmlSpawner2;


namespace Server.Misc
{
	public class Telepathy : BaseCustomSpell
	{

public override bool IsMageSpell { get { return true; } }
public override bool CanTargetSelf { get { return true; } }
        public override bool AffectsItems { get { return false; } }
public override bool AffectsMobiles { get { return true; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return true; } }
public override FeatList Feat{ get{ return FeatList.CustomMageSpell; } }

        public override int TotalCost { get { return 10; } }

		
		public override SkillName GetSkillName{ get{ return SkillName.Magery; } }
		
		public Telepathy( Mobile Caster, int featLevel ) : base( Caster, featLevel )
		{
		}
		
		public override bool CanBeCast
        {
        	
        	 get
            {
            	PlayerMobile l = Caster as PlayerMobile;
                XmlData awe = XmlAttach.FindAttachment( l, typeof( XmlData ), "Telepathy" ) as XmlData;
                if (awe != null)
                    return base.CanBeCast;
                else
                    return false;
            }
        }
        public string nam { get { Caster.Name; } }
        public string mess;
		public override void Effect( )
		{
            PlayerMobile l = Caster as PlayerMobile;
			if( TargetCanBeAffected && CasterHasEnoughMana )
			{

                TargetMobile.SendMessage(2660, nam + ":" + " " + mess); 
				Success = true;
			}
            Success = false; 

			}

                        /*
protected override void OnTarget( Mobile from, object targeted )
{
if( from == null || targeted == null || !(from is PlayerMobile) || !(targeted is PlayerMobile) )
return;

PlayerMobile target = targeted as PlayerMobile;

if( m_Setting )
{
target.Backgrounds.BackgroundDictionary[m_Background].Level = m_Level;
from.SendMessage( "Property has been set." );
}

else
from.SendMessage( m_Background.ToString() + " = " + target.Backgrounds.BackgroundDictionary[m_Background].Level.ToString() );
}
        }
*/
		
		public static void Initialize()
		{
			CommandSystem.Register( "Telepathy", AccessLevel.Player, new CommandEventHandler( Telepathy_OnCommand ) );
		}
		
		[Usage( "Telepathy" )]
        [Description( "Casts Telepathy." )]
        private static void Telepathy_OnCommand( CommandEventArgs e )
        {
           
            if (e.Mobile == null || !(e.Mobile is PlayerMobile) || e.Length < 1 || e.Arguments[0].Trim().Length < 1)
                return;
            mess = e.ArgString;
        	if( e.Mobile != null )
        		SpellInitiator( new Telepathy( e.Mobile, GetSpellPower( "3", ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.MindI) ) ) );
        }
	}
}
