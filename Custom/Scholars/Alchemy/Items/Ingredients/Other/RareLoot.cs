using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class DragonBlood : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, 100),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, 150),
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRegeneration, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DragonBlood( int amount ) : base( 3620, amount )
		{
			Name = "dragon blood";
			Hue = 2886;
		}

		[Constructable]
		public DragonBlood() : this( 1 )
		{
		}

		public DragonBlood( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class DragonHeart : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Cure, 150),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRestoration, 100),
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRegeneration, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public DragonHeart( int amount ) : base( 3184, amount )
		{
			Name = "dragon heart";
			Hue = 2771;
		}

		[Constructable]
		public DragonHeart() : this( 1 )
		{
		}

		public DragonHeart( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class DragonTalon : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Shrapnel, 100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public DragonTalon( int amount ) : base( 5919, amount )
		{
			Name = "dragon talon";
			Hue = 2932;
		}

		[Constructable]
		public DragonTalon() : this( 1 )
		{
		}

		public DragonTalon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class DragonPancreas : BaseIngredient, IBombIngredient, IOilIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Fire, 150)
			}; 
		} }
		
		public override int SkillRequired { get { return 900; } }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 15; } }
		int IOilIngredient.Duration { get { return 0; } }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public DragonPancreas( int amount ) : base( 3195, amount )
		{
			Name = "dragon pancreas";
			Hue = 2778;
		}

		[Constructable]
		public DragonPancreas() : this( 1 )
		{
		}

		public DragonPancreas( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class NecroticExcrement : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.StickyGoo, 150)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 2; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public NecroticExcrement( int amount ) : base( 3186, amount )
		{
			Name = "necrotic excrement";
			Hue = 2964;
		}

		[Constructable]
		public NecroticExcrement() : this( 1 )
		{
		}

		public NecroticExcrement( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class GorgonEyes : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Paralysis, 150)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public GorgonEyes( int amount ) : base( 2485, amount )
		{
			Name = "gorgon eyes";
			Hue = 2969;
		}

		[Constructable]
		public GorgonEyes() : this( 1 )
		{
		}

		public GorgonEyes( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class GorgonBrain : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Intelligence, 150),
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRegeneration, 100),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Stamina, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public GorgonBrain( int amount ) : base( 3184, amount )
		{
			Name = "gorgon brain";
			Hue = 1061;
		}

		[Constructable]
		public GorgonBrain() : this( 1 )
		{
		}

		public GorgonBrain( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class GazerIris : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Paralysis, 100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 2; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public GazerIris( int amount ) : base( 3189, amount )
		{
			Name = "gazer iris";
			Hue = 2805;
		}

		[Constructable]
		public GazerIris() : this( 1 )
		{
		}

		public GazerIris( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class GiantTesticle : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, 150),
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRegeneration, 100),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Intelligence, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public GiantTesticle( int amount ) : base( 3164, amount )
		{
			Name = "giant testicle";
			Hue = 2804;
		}

		[Constructable]
		public GiantTesticle() : this( 1 )
		{
		}

		public GiantTesticle( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class GiantLiver : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, 200),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Dexterity, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public GiantLiver( int amount ) : base( 2505, amount )
		{
			Name = "Giant Liver";
			Hue = 2723;
		}

		[Constructable]
		public GiantLiver() : this( 1 )
		{
		}

		public GiantLiver( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class TrollNail : BaseIngredient, IBombIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Shrapnel, 40)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IBombIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IBombIngredient.Range { get { return 0; } }
		bool IBombIngredient.InstantEffect { get { return false; } }

		[Constructable]
		public TrollNail( int amount ) : base( 3977, amount )
		{
			Name = "Troll Nail";
			Hue = 2983;
		}

		[Constructable]
		public TrollNail() : this( 1 )
		{
		}

		public TrollNail( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class TrollSpleen : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Dexterity, 100),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRestoration, 150),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Hunger, -100),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Thirst, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public TrollSpleen( int amount ) : base( 2512, amount )
		{
			Name = "Troll Spleen";
			Hue = 2723;
		}

		[Constructable]
		public TrollSpleen() : this( 1 )
		{
		}

		public TrollSpleen( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class BeastmanLung : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Stamina, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRegeneration, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRestoration, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Intelligence, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public BeastmanLung( int amount ) : base( 2513, amount )
		{
			Name = "Beastman Lung";
			Hue = 2844;
		}

		[Constructable]
		public BeastmanLung() : this( 1 )
		{
		}

		public BeastmanLung( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class GoatmanHorns : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, 20),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, 20),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRestoration, 20),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Mana, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public GoatmanHorns( int amount ) : base( 3553, amount )
		{
			Name = "Goatman Horns";
			Hue = 2301;
		}

		[Constructable]
		public GoatmanHorns() : this( 1 )
		{
		}

		public GoatmanHorns( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class TroglinTongue : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRestoration, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Hunger, -100),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Thirst, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public TroglinTongue( int amount ) : base( 3882, amount )
		{
			Name = "Troglin Tongue";
			Hue = 2778;
		}

		[Constructable]
		public TroglinTongue() : this( 1 )
		{
		}

		public TroglinTongue( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class OgreTendons : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Dexterity, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, 40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Hunger, -100),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Thirst, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public OgreTendons( int amount ) : base( 3181, amount )
		{
			Name = "Ogre Tendons";
			Hue = 2600;
		}

		[Constructable]
		public OgreTendons() : this( 1 )
		{
		}

		public OgreTendons( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class OgreLard : BaseIngredient, IOilIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] {
			}; 
		} }
		
		public override int SkillRequired { get { return 900; } }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 0; } }
		int IOilIngredient.Duration { get { return 24000; } }

		[Constructable]
		public OgreLard( int amount ) : base( 12677, amount )
		{
			Name = "Ogre Lard";
			Hue = 2932;
		}

		[Constructable]
		public OgreLard() : this( 1 )
		{
		}

		public OgreLard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class ThickNecroplasm : BaseIngredient, IOilIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRegeneration, -40)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 2; } }
		int IOilIngredient.Duration { get { return 600; } }

		[Constructable]
		public ThickNecroplasm( int amount ) : base( 3195, amount )
		{
			Name = "Thick Necroplasm";
			Hue = 2688;
		}

		[Constructable]
		public ThickNecroplasm() : this( 1 )
		{
		}

		public ThickNecroplasm( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class GnollBlood : BaseIngredient, IOilIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, -40),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, -40)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IOilIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IOilIngredient.Corrosivity { get { return 0; } }
		int IOilIngredient.Duration { get { return 600; } }

		[Constructable]
		public GnollBlood( int amount ) : base( 3620, amount )
		{
			Name = "Thick Necroplasm";
			Hue = 2656;
		}

		[Constructable]
		public GnollBlood() : this( 1 )
		{
		}

		public GnollBlood( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class HagFingers : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRegeneration, 150),
				new KeyValuePair<CustomEffect, int>(CustomEffect.ManaRestoration, 100),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public HagFingers( int amount ) : base( 3553, amount )
		{
			Name = "Hag Fingers";
			Hue = 2985;
		}

		[Constructable]
		public HagFingers() : this( 1 )
		{
		}

		public HagFingers( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class MinotaurHeart : BaseIngredient, IDrinkIngredient
	{
		public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Strength, 150),
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPoints, 100),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Stamina, -100)
			}; 
		} }

		public override int SkillRequired { get { return 900; } }

		bool IDrinkIngredient.CanUse( Mobile mobile )
		{
			return true;
		}

		int IDrinkIngredient.PotionBooster { get { return 0; } }

		[Constructable]
		public MinotaurHeart( int amount ) : base( 2513, amount )
		{
			Name = "Minotaur Heart";
			Hue = 2600;
		}

		[Constructable]
		public MinotaurHeart() : this( 1 )
		{
		}

		public MinotaurHeart( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

    public class ShimmeringBass : BaseIngredient, IBombIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Flash, 40),
                new KeyValuePair<CustomEffect, int>(CustomEffect.Adhesive, 40)
			};
            }
        }

        bool IBombIngredient.CanUse( Mobile mobile )
        {
            return true;
        }

        int IBombIngredient.Range { get { return 2; } }
        bool IBombIngredient.InstantEffect { get { return true; } }

        public override int SkillRequired { get { return 1000; } }

        [Constructable]
        public ShimmeringBass( int amount )
            : base( 2509, amount )
        {
            Name = "Shimmering Bass";
            Hue = 2823;
        }

        [Constructable]
        public ShimmeringBass()
            : this( 1 )
        {
        }

        public ShimmeringBass( Serial serial )
            : base( serial )
        {
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
    }

    public class MurrelFish : BaseIngredient, IDrinkIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.HitPointRestoration, 30),
				new KeyValuePair<CustomEffect, int>(CustomEffect.StaminaRestoration, 30),
				new KeyValuePair<CustomEffect, int>(CustomEffect.Hunger, 10)
			};
            }
        }

        public override int SkillRequired { get { return 900; } }

        bool IDrinkIngredient.CanUse( Mobile mobile )
        {
            return true;
        }

        int IDrinkIngredient.PotionBooster { get { return 0; } }

        [Constructable]
        public MurrelFish( int amount )
            : base( 3542, amount )
        {
            Name = "Murrel Fish";
            Hue = 2828;
        }

        [Constructable]
        public MurrelFish()
            : this( 1 )
        {
        }

        public MurrelFish( Serial serial )
            : base( serial )
        {
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
    }

    public class BarbedFish : BaseIngredient, IBombIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects { get { 
			return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Shrapnel, 20),
				new KeyValuePair<CustomEffect, int>(CustomEffect.StickyGoo, 10)
			}; 
		} }

        public override int SkillRequired { get { return 1000; } }

        bool IBombIngredient.CanUse( Mobile mobile )
        {
            return true;
        }

        int IBombIngredient.Range { get { return -1; } }
        bool IBombIngredient.InstantEffect { get { return false; } }

        [Constructable]
        public BarbedFish( int amount )
            : base( 3542, amount )
        {
            Name = "Barbed Fish";
            Hue = 2705;
        }

        [Constructable]
        public BarbedFish()
            : this( 1 )
        {
        }

        public BarbedFish( Serial serial )
            : base( serial )
        {
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
    }

    public class DragonFish : BaseIngredient, IBombIngredient, IOilIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[] { 
				new KeyValuePair<CustomEffect, int>(CustomEffect.Fire, 50)
			};
            }
        }

        public override int SkillRequired { get { return 1000; } }

        bool IOilIngredient.CanUse( Mobile mobile )
        {
            return true;
        }

        int IOilIngredient.Corrosivity { get { return 5; } }
        int IOilIngredient.Duration { get { return 0; } }

        bool IBombIngredient.CanUse( Mobile mobile )
        {
            return true;
        }

        int IBombIngredient.Range { get { return 0; } }
        bool IBombIngredient.InstantEffect { get { return false; } }

        [Constructable]
        public DragonFish( int amount )
            : base( 3542, amount )
        {
            Name = "Dragon Fish";
            Hue = 2712;
        }

        [Constructable]
        public DragonFish()
            : this( 1 )
        {
        }

        public DragonFish( Serial serial )
            : base( serial )
        {
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
    }
}
