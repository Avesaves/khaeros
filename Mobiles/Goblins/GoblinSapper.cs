using System;
using System.Collections;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a goblin sapper corpse")]
    public class GoblinSapper : BaseCreature, IMediumPredator, IHasReach
    {
        public override bool ParryDisabled { get { return true; } }
        [Constructable]
        public GoblinSapper()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a goblin sapper";
            Body = 220;
            Hue = Utility.RandomMinMax(1900,1908);

            SetStr(40, 50);
            SetDex(40, 50);
            SetInt(30, 60);

            SetHits(40, 50);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Blunt, 100);

            SetResistance(ResistanceType.Blunt, 5, 10);
            SetResistance(ResistanceType.Piercing, 5, 10);
            SetResistance(ResistanceType.Slashing, 5, 10);
            SetResistance(ResistanceType.Fire, 0);
            SetResistance(ResistanceType.Cold, 20);
            SetResistance(ResistanceType.Poison, 20);
            SetResistance(ResistanceType.Energy, 20);

            CombatSkills = 60;

            EquipItem(new Pickaxe());
            EquipItem(new Tunic());

            Fame = 900;
            Karma = -900;
        }

        public override void AddBodyParts(BodyPartsContainer bpc, Corpse corpse)
        {
            base.AddBodyParts(bpc, corpse);
            bpc.DropItem(new WeakBlackPowder());
            bpc.DropItem(new GoblinBrain());
            bpc.DropItem(new GoblinGonads()); 
        }

        public override int Meat { get { return 10; } }
        public override int Bones { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override int GetAngerSound()
        {
            return 0x1A1;
        }

        public override int GetIdleSound()
        {
            return 0x1A2;
        }

        public override int GetAttackSound()
        {
            return 0x1A3;
        }

        public override int GetHurtSound()
        {
            return 0x1A4;
        }

        public override int GetDeathSound()
        {
            return 0x1A5;
        }

        public void ExplodeItself(Mobile target)
        {
            Point3D loc = this.Location;
            Map map = this.Map;
            
            if (map == null)
                return;

            BombPotion pot = new BombPotion(1);

            pot.InstantExplosion = true;
            pot.ExplosionRange = 2;
            pot.AddEffect(CustomEffect.Explosion, 15);
            pot.AddEffect(CustomEffect.Fire, 3);
            pot.AddEffect(CustomEffect.Shrapnel, 10);
            pot.HeldBy = this;
            pot.PotionEffect = PotionEffect.ExplosionLesser;

            this.Say("Uh oh!");
            target.Emote("*You have triggered " + this.Name + "'s explosives!");
            this.PlaySound(519);

            pot.Explode(this, false, loc, map);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            double explodeChanceReduction = ((IKhaerosMobile)attacker).Feats.GetFeatLevel(FeatList.BlackPowder) / 10;

            if ((1.0 - explodeChanceReduction) >= Utility.RandomDouble())
                ExplodeItself(attacker);        
            
            base.OnGotMeleeAttack(attacker);
        }

    			public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor, 1 );
		}

        public GoblinSapper(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
