using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Commands
{
    public class ItemActivationCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register( "ActivateWeapon", AccessLevel.Player, new CommandEventHandler( ActivateWeapon_OnCommand ) );
            CommandSystem.Register( "ActivateShield", AccessLevel.Player, new CommandEventHandler( ActivateShield_OnCommand ) );
            CommandSystem.Register( "ActivateRing", AccessLevel.Player, new CommandEventHandler( ActivateRing_OnCommand ) );
            CommandSystem.Register( "ActivateBracelet", AccessLevel.Player, new CommandEventHandler( ActivateBracelet_OnCommand ) );
            CommandSystem.Register( "ActivateNecklace", AccessLevel.Player, new CommandEventHandler( ActivateNecklace_OnCommand ) );
        }

        [Usage( "ActivateWeapon" )]
        [Description( "Attempts to find and use XmlAttachments on your weapon." )]
        private static void ActivateWeapon_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;
            Item item = m.FindItemOnLayer( Layer.FirstValid );

            if( item != null && item is BaseWeapon )
                FindAndUseAttachmentsOn( m, item );

            else if( (item = m.FindItemOnLayer( Layer.TwoHanded )) != null && item is BaseWeapon )
                FindAndUseAttachmentsOn( m, item );
        }

        [Usage( "ActivateShield" )]
        [Description( "Attempts to find and use XmlAttachments on your shield." )]
        private static void ActivateShield_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;
            Item item = m.FindItemOnLayer( Layer.TwoHanded );

            if( item != null && item is BaseShield )
                FindAndUseAttachmentsOn( m, item );
        }

        [Usage( "ActivateRing" )]
        [Description( "Attempts to find and use XmlAttachments on your ring." )]
        private static void ActivateRing_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;
            Item item = m.FindItemOnLayer( Layer.Ring );

            if( item != null && item is BaseJewel )
                FindAndUseAttachmentsOn( m, item );
        }

        [Usage( "ActivateBracelet" )]
        [Description( "Attempts to find and use XmlAttachments on your bracelet." )]
        private static void ActivateBracelet_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;
            Item item = m.FindItemOnLayer( Layer.Bracelet );

            if( item != null && item is BaseJewel )
                FindAndUseAttachmentsOn( m, item );
        }

        [Usage( "ActivateNecklace" )]
        [Description( "Attempts to find and use XmlAttachments on your necklace." )]
        private static void ActivateNecklace_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;
            Item item = m.FindItemOnLayer( Layer.Neck );

            if( item != null && item is BaseJewel )
                FindAndUseAttachmentsOn( m, item );
        }

        public static void FindAndUseAttachmentsOn( Mobile user, Item item )
        {
            ArrayList list = XmlAttach.FindAttachments( item, typeof( XmlAttachment ) );

            if( list == null )
                return;

            for( int i = 0; i < list.Count; ++i )
                ((XmlAttachment)list[i]).OnActivatedBy( user );
        }
    }
}
