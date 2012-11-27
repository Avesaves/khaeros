using System;
using Server.Items;
using Server.Mobiles;
using Server.Prompts;

namespace Server.Items
{
    public class RingOfTheFaceless : BaseRing
    {
        [Constructable]
        public RingOfTheFaceless()
            : base( 0x108a )
        {
            Weight = 0.1;
            Name = "Ring of the Faceless";
            Quality = WeaponQuality.Masterwork;
            GemType = GemType.Diamond;
            Description = "A flawless ring of ancient and meticulous design, whose length is entirely encrusted with small diamonds that blend into its shape seamlessly.";
        }

        public RingOfTheFaceless( Serial serial )
            : base( serial )
        {
        }

        public override void OnDoubleClick( Mobile from )
        {
            if( RootParentEntity == from )
            {
                from.SendMessage( "Please enter the name of your new identity." );
                from.Prompt = new NewIdentity();
            }
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }

        private class NewIdentity : Prompt
        {
            public NewIdentity()
            {
            }

            public override void OnResponse( Mobile from, string text )
            {
                if( from == null || from.Deleted )
                    return;

                if( !String.IsNullOrEmpty( text ) )
                {
                    from.SendMessage( "You assume your new identity, your facial features changing accordingly." );
                    from.Name = text;
                }

                else
                    from.SendMessage( "Invalid name." );
            }
        }
    }
}
