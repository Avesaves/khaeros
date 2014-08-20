using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Khaeros.Scripts.Khaeros.Spells
{
    public class QuestAltar : Item
    {


	public override bool HandlesOnSpeech{ get{ return true; } }


        Dictionary<Type, List<SpellScrollCost>> spellCosts;

        void AddSpellCosts()
        {
            List<SpellScrollCost> Quest1Costs = new List<SpellScrollCost>();
            Quest1Costs.Add(CreateSpellCost(typeof(Quest1Item), 1));

            List<SpellScrollCost> Quest2Costs = new List<SpellScrollCost>();
            Quest2Costs.Add(CreateSpellCost(typeof(Quest2Item), 1));

            List<SpellScrollCost> Quest3Costs = new List<SpellScrollCost>();
            Quest3Costs.Add(CreateSpellCost(typeof(Quest3Item), 1));

            List<SpellScrollCost> Quest4Costs = new List<SpellScrollCost>();
            Quest4Costs.Add(CreateSpellCost(typeof(Quest4Item), 1));

            List<SpellScrollCost> Quest5Costs = new List<SpellScrollCost>();
            Quest5Costs.Add(CreateSpellCost(typeof(Quest5Item), 1));

            List<SpellScrollCost> Quest6Costs = new List<SpellScrollCost>();
            Quest6Costs.Add(CreateSpellCost(typeof(Quest6Item), 1));

            List<SpellScrollCost> Quest7Costs = new List<SpellScrollCost>();
            Quest7Costs.Add(CreateSpellCost(typeof(Quest7Item), 1));


            AddToSpellCosts(typeof (Quest1pack), Quest1Costs);
            AddToSpellCosts(typeof(Quest2pack), Quest2Costs);
            AddToSpellCosts(typeof(Quest3pack), Quest3Costs);
            AddToSpellCosts(typeof(Quest4pack), Quest4Costs);
            AddToSpellCosts(typeof(Quest5pack), Quest5Costs);
            AddToSpellCosts(typeof(Quest6pack), Quest6Costs);
            AddToSpellCosts(typeof(Quest7pack), Quest7Costs);
        }

        void BuySpells(string speech, Container backpack, Mobile buyer)
        {
            if (speech.Contains("goblins") || speech.Contains("goblin"))
            {
                BuySpell<Quest1pack>(backpack, buyer);
                //buyer.Emote("*tries to exchange an item for a reward*");
            }
            if (speech.Contains("plants")||speech.Contains("plant"))
            {
                BuySpell<Quest2pack>(backpack, buyer);
                //buyer.Emote("*tries to exchange an item for a reward*");
            }
            if (speech.Contains("elementals")||speech.Contains("elemental"))
            {
                BuySpell<Quest3pack>(backpack, buyer);
                //buyer.Emote("*tries to exchange an item for a reward*");
            }
            if (speech.Contains("beasts")||speech.Contains("beast"))
            {
                BuySpell<Quest4pack>(backpack, buyer);
                //buyer.Emote("*tries to exchange an item for a reward*");
            }
            if (speech.Contains("devourer"))
            {
                BuySpell<Quest5pack>(backpack, buyer);
                //buyer.Emote("*tries to exchange an item for a reward*");
            }
            if (speech.Contains("star")||speech.Contains("creature"))
            {
                BuySpell<Quest6pack>(backpack, buyer);
                //buyer.Emote("*tries to exchange an item for a reward*");
            }
            if (speech.Contains("unknown") || speech.Contains("fearsome"))
            {
                BuySpell<Quest7pack>(backpack, buyer);
                //buyer.Emote("*tries to exchange an item for a reward*");
            }
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
			if (PlayerIsInRange(e))
			{
			Mobile buyer = e.Mobile;	
                //e.Mobile.SendMessage("Your words seem to echo back at you....");
			    string speech = e.Speech;
			   // e.Mobile.SendMessage(e.Speech);
			    
			    Container backpack = buyer.Backpack;

                Console.WriteLine(e.Speech);

                BuySpells(speech, backpack, buyer);
			}
        }

        void BuySpell<T>(Container backpack, Mobile buyer) where T : Item
        {
            if (PlayerHasEnoughGemsToPurchase(typeof(T), backpack))
            {
                ConsumeGemsByPurchasing(typeof(T), backpack);
                T scroll = Activator.CreateInstance<T>();
                buyer.AddToBackpack(scroll);
            }
        }

        void ConsumeGemsByPurchasing(Type spell, Container backpack)
        {
            List<SpellScrollCost> costs = spellCosts[spell];

            foreach (SpellScrollCost cost in costs)
            {
                backpack.ConsumeTotal(cost.Gem, cost.Amount);
            }
        }

        bool PlayerHasEnoughGemsToPurchase(Type spell, Container backpack)
        {
        if (spell == null || backpack == null) 
        	return false;
        
            if (!spellCosts.ContainsKey(spell))
                return false;

            List<SpellScrollCost> costs = spellCosts[spell];

            foreach (SpellScrollCost cost in costs)
            {
                if (backpack.GetAmount(cost.Gem) < cost.Amount)
                    return false;
            }
            return true;
        }

        void AddToSpellCosts(Type spell, List<SpellScrollCost> cost)
        {
            spellCosts.Add(spell, cost);
        }

        SpellScrollCost CreateSpellCost(Type gem, int amount)
        {
            return new SpellScrollCost(gem, amount);
        }

        bool PlayerIsInRange(SpeechEventArgs e)
        {
            return !e.Handled && e.Mobile.InRange(this.Location, 5);
        }

	void InitializeCosts()
	{
	 	spellCosts = new Dictionary<Type, List<SpellScrollCost>>();
            	AddSpellCosts();
	}

        [Constructable]
		public QuestAltar() : this( 1 )
		{
		}

		[Constructable]
		public QuestAltar( int amount ) : base( 0x3650 )
		{
			Amount = amount;
			InitializeCosts();
		}

        public QuestAltar(Serial serial)
            : base(serial)
		{
			InitializeCosts();
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

