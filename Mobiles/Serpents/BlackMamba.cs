using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Poisoning;
using System.Collections.Generic;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName("a black mamba corpse")]
	public class BlackMamba : Serpent, IJungleCreature
	{
		[Constructable]
		public BlackMamba() : base()
		{
            NewBreed = "Black Mamba";
            Hue = 2991;
		}

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new BlackMamba());
        }
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			bpc.DropItem( new BlackMambaVenom() );
		}

        public override void incSerpentPoison()
        {

            int dmgLevelUp = Utility.Random(XPScale) + 1;

            this.poisonHealthDamage += dmgLevelUp;

            if(this.ControlMaster != null)
                this.ControlMaster.SendMessage(this.Name + "'s venom is more deadly!");
        }

        public override KeyValuePair<PoisonEffectEnum, int>[] HitPoison
        {
            get
            {
                return new KeyValuePair<PoisonEffectEnum, int>[] 
                    { 
                        new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, this.poisonHealthDamage),
                    };
            }
        }

        public override int PoisonDuration { get { return this.poisonDuration; } }
        public override int PoisonActingSpeed { get { return this.poisonSpeed; } }

		public BlackMamba(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
