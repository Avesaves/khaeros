using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Poisoning;
using System.Collections.Generic;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
	[CorpseName( "a grass snake corpse" )]
	public class GrassSnake : Serpent, IPlainsCreature
	{
        private bool constrictAbility;

		[Constructable]
		public GrassSnake() : base()
		{
            NewBreed = "Grass Snake";
            Hue = 2003;
            constrictAbility = false;

            SetDamageType(ResistanceType.Blunt, 100);
            SetDamageType(ResistanceType.Piercing, 0);
            SetSkill(SkillName.Poisoning, 0);

            SetStr(25, 35);
            SetDex(35, 45);
            SetHits(25, 35);
		}

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new GrassSnake());
        }

        public void SetConstrict(bool x)
        {
            constrictAbility = x;
        }

        public bool GetConstrict()
        {
            return constrictAbility;
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            if (this.GetConstrict())
            {
                int constrictChance = Utility.Random(100);
                TimeSpan constrictDuration = TimeSpan.FromSeconds(XPScale);
                double defenderConstrictResistance = (defender.RawDex / 100);
                int constrictCheck = 80;

                if (this.Level >= 50)
                    constrictCheck -= 20;
                else if (this.Level >= 40)
                    constrictCheck -= 10;
                else if (this.Level >= 30)
                    constrictCheck -= 5;

                if (constrictChance > constrictCheck && !defender.Paralyzed && this.CanUseSpecial)
                {
                    this.Emote("*attempts to coil itself around " + defender.Name + "!*");

                    if (this.Combatant is PlayerMobile && ((PlayerMobile)this.Combatant).Evaded())
                        return;
                    else if ((defenderConstrictResistance >= this.XPScale) && (constrictChance < 100))
                    {
                        defender.Emote("*escapes being constricted by " + this.Name + "!*");
                        return;
                    }
                    else
                    {
                        this.Emote("*constricts " + defender.Name + "!*");

                        if (defender is PlayerMobile)
                            ((PlayerMobile)defender).Paralyze(constrictDuration);

                        if (defender is BaseCreature)
                            ((BaseCreature)defender).Paralyze(constrictDuration);
                    }
                }
            }

            base.OnGaveMeleeAttack(defender);
        }

		public GrassSnake(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

            writer.Write((bool)constrictAbility);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

            constrictAbility = reader.ReadBool();

			int version = reader.ReadInt();
		}
	}
}
