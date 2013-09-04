using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class FenceNPC: BaseVendor, INorthern
    {
        private ArrayList m_SBInfos = new ArrayList();

        [Constructable]
        public FenceNPC()
            : base("the Shady Merchant")
        {
                   
        }

        public FenceNPC(Serial serial)
            : base(serial)
        {
            
        }

        public override TimeSpan RestockDelay
        {
            get
            {
                return TimeSpan.FromDays(7);
            }
        }

        public override void InitBody()
        {
            Hue = BaseKhaerosMobile.AssignRacialHue(Mobiles.Nation.Northern);
            HairItemID = BaseKhaerosMobile.AssignRacialHair(Mobiles.Nation.Northern, this.Female);
            int hairhue = BaseKhaerosMobile.AssignRacialHairHue(Mobiles.Nation.Northern);
            HairHue = hairhue;

            Body = 0x190;
            Name = BaseKhaerosMobile.RandomName(Mobiles.Nation.Northern, false) + BaseKhaerosMobile.RandomSurname(Mobiles.Nation.Northern, false);
        }

        public override void InitOutfit()
        {
            BaseKhaerosMobile.RandomCrafterClothes(this, Mobiles.Nation.Northern);
            PackGold(1000, 2000);
        }

        protected override ArrayList SBInfos
        {
            get { return m_SBInfos; }
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFence());
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }        

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }
        
    }
}
