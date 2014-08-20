using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a corpse" )]
    public class Quest6 : BaseCreature, IRegenerativeCreature, ILargePredator, IMhordulFavoredEnemy, IEnraged, ITroll
	{
		[Constructable]
        public Quest6() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{

            //Body = 130;
            BaseSoundID = 461;

            SetStr(180, 200);
            SetDex(66, 85);
            SetInt(35);

            SetHits(176, 193);

            SetDamage(14, 16);

            SetDamageType(ResistanceType.Blunt, 100);

            SetResistance(ResistanceType.Blunt, 30, 35);
            SetResistance(ResistanceType.Piercing, 30, 40);
            SetResistance(ResistanceType.Slashing, 30, 40);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.MagicResist, 50.1, 95.0);
            SetSkill(SkillName.Tactics, 60.1, 100.0);
            SetSkill(SkillName.UnarmedFighting, 60.1, 100.0);
            VirtualArmor = 34;
            Name = "a Cave Troll";
            WikiConfig = "frozenbehemoth";
            LoadWikiConfig = true; 
           

            
			Fame = 80000;
			Karma = -80000;
            PackItem( new RewardToken( 11 ) );
            PackItem(new Quest6Item());
            int rand = Utility.Random(40);
            if (rand > 39)
                PackItem(new StarmetalOre(2)); 
		}
		
	/*	public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			DragonHead head = new DragonHead();
			head.Hue = 2832;
            head.Name = "A Wyrm's head"; 
			bpc.DropItem( head );
            bpc.DropItem(new DragonEye()); 
		} */

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 2 );
			AddLoot( LootPack.Gems, 15 );
		}

		public Quest6( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
