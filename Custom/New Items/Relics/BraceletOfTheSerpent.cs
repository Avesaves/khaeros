using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class BraceletOfTheSerpent : GoldBracelet
    {
        private int m_OriginalBody;
        private int m_OriginalHue;

        public int OriginalBody { get { return m_OriginalBody; } set { m_OriginalBody = value; } }
        public int OriginalHue { get { return m_OriginalHue; } set { m_OriginalHue = value; } }

        private string m_SerpentName;
        [CommandProperty( AccessLevel.GameMaster )]
        public string SerpentName { get { return m_SerpentName; } set { m_SerpentName = value; } }

        [Constructable]
        public BraceletOfTheSerpent()
        {
            Name = "Bracelet of the Serpent";
        }

        public BraceletOfTheSerpent( Serial serial )
            : base( serial )
        {
        }

        public void SerpentFormTo( Mobile from )
        {
            from.Emote( "*changes {0} form into a large serpent-man*", from.Female == true ? "her" : "his" );
            OriginalBody = from.BodyValue;
            OriginalHue = from.Hue;
            from.BodyValue = 86;
            from.Hue = 2932;
            from.AddStatMod( new StatMod( StatType.HitsMax, "Bracelet of the Serpent", 20, TimeSpan.Zero ) );

            if( !String.IsNullOrEmpty( SerpentName ) )
                from.NameMod = SerpentName;
        }

        public void RevertFormOn( Mobile from )
        {
            from.BodyValue = OriginalBody;
            from.Hue = OriginalHue;
            from.Emote( "*reverts to {0} original form*", from.Female == true ? "her" : "his" );
            OriginalHue = 0;
            OriginalBody = 0;
            from.RemoveStatMod( "Bracelet of the Serpent" );
            from.NameMod = null;
        }

        public override void OnDoubleClick( Mobile from )
        {
            if( OriginalBody > 0 )
                RevertFormOn( from );

            else if( this.ParentEntity == from )
                SerpentFormTo( from );

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

            writer.Write( (int)1 ); // version
            writer.Write( (int)m_OriginalBody );
            writer.Write( (int)m_OriginalHue );
            writer.Write( (string)m_SerpentName );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
            m_OriginalBody = reader.ReadInt();
            m_OriginalHue = reader.ReadInt();

            if( version > 0 )
                m_SerpentName = reader.ReadString();
        }
    }
}
