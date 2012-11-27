using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class BraceletOfTheShapeshifter : GoldBracelet
    {
        private int m_OriginalBody;
        private int m_OriginalHue;

        public int OriginalBody { get { return m_OriginalBody; } set { m_OriginalBody = value; } }
        public int OriginalHue { get { return m_OriginalHue; } set { m_OriginalHue = value; } }

        [Constructable]
        public BraceletOfTheShapeshifter()
        {
            Name = "Bracelet of the Shapeshifter";
        }

        public BraceletOfTheShapeshifter( Serial serial )
            : base( serial )
        {
        }

        public void RevertFormOn( Mobile from )
        {
            from.BodyValue = OriginalBody;
            from.Hue = OriginalHue;
            from.Emote( "*reverts to {0} original form*", from.Female == true ? "her" : "his" );
            OriginalHue = 0;
            OriginalBody = 0;
            from.NameMod = null;
        }

        public override void OnDoubleClick( Mobile from )
        {
            if( OriginalBody > 0 )
                RevertFormOn( from );

            else if( this.ParentEntity == from )
                from.Target = new ShapeShiftingTarget( this );
                
            base.OnDoubleClick( from );
        }

        public override void OnRemoved( object parent )
        {
            if( OriginalBody > 0 && parent != null && parent is Mobile )
                RevertFormOn( (Mobile)parent );

            base.OnRemoved( parent );
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
            writer.Write( (int)m_OriginalBody );
            writer.Write( (int)m_OriginalHue );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
            m_OriginalBody = reader.ReadInt();
            m_OriginalHue = reader.ReadInt();
        }

        private class ShapeShiftingTarget : Target
        {
            private BraceletOfTheShapeshifter bracelet;

            public ShapeShiftingTarget( BraceletOfTheShapeshifter source )
                : base( 15, false, TargetFlags.None )
            {
                bracelet = source;
            }

            protected override void OnTarget( Mobile from, object obj )
            {
                if( from == null || from.Deleted || obj == null || bracelet == null || bracelet.Deleted )
                    return;

                if( bracelet.ParentEntity != from )
                    from.SendMessage( "You must be wearing the bracelet to use it." );

                else if( obj is Mobile )
                {
                    Mobile target = obj as Mobile;
                    from.Emote( "*changes {0} form to copy {1}*", from.Female == true ? "her" : "his", target.Name );
                    bracelet.OriginalBody = from.BodyValue;
                    bracelet.OriginalHue = from.Hue;
                    from.BodyValue = target.BodyValue;
                    from.Hue = target.Hue;
                    from.NameMod = target.Name;
                }

                else
                    from.SendMessage( "Invalid target." );
            }
        }
    }
}
