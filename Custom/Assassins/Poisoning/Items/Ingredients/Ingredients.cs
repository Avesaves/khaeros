using Server.Engines.Poisoning;
using System.Collections.Generic;

namespace Server.Items
{
	public class BushmasterVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StrengthDecrease, 50),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 10)
			}; 
		} }

		public override int SkillRequired { get { return 100; } }
		public override int Corrosivity { get { return 0; } }
		public override int ToxinActingSpeed { get { return 1; } }
		public override int ToxinDuration { get { return 360; } }

		[Constructable]
		public BushmasterVenom( int amount ) : base( 3173, amount )
		{
			Name = "Bushmaster Venom";
			Hue = 2642;
		}

		[Constructable]
		public BushmasterVenom() : this( 1 )
		{
		}

		public BushmasterVenom( Serial serial ) : base( serial )
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
	
	public class FunnelWebSpiderVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DexterityDecrease, 50),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 10)
			}; 
		} }

		public override int SkillRequired { get { return 150; } }
		public override int Corrosivity { get { return 0; } }
		public override int ToxinActingSpeed { get { return 1; } }
		public override int ToxinDuration { get { return 360; } }

		[Constructable]
		public FunnelWebSpiderVenom( int amount ) : base( 3982, amount )
		{
			Name = "Funnel Web Spider Venom";
			Hue = 0;
		}

		[Constructable]
		public FunnelWebSpiderVenom() : this( 1 )
		{
		}

		public FunnelWebSpiderVenom( Serial serial ) : base( serial )
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
	
	public class ScorpionVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StaminaDecrease, 50),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 10)
			}; 
		} }

		public override int SkillRequired { get { return 250; } }
		public override int Corrosivity { get { return 0; } }
		public override int ToxinActingSpeed { get { return 1; } }
		public override int ToxinDuration { get { return 360; } }

		[Constructable]
		public ScorpionVenom( int amount ) : base( 3979, amount )
		{
			Name = "Scorpion Venom";
			Hue = 2669;
		}

		[Constructable]
		public ScorpionVenom() : this( 1 )
		{
		}

		public ScorpionVenom( Serial serial ) : base( serial )
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
	
	public class CopperheadVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.ManaDecrease, 20),
                new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.IntelligenceDecrease, 20),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 10)
			}; 
		} }

		public override int SkillRequired { get { return 300; } }
		public override int Corrosivity { get { return 0; } }
		public override int ToxinActingSpeed { get { return 1; } }
		public override int ToxinDuration { get { return 360; } }

		[Constructable]
		public CopperheadVenom( int amount ) : base( 3967, amount )
		{
			Name = "Copperhead Venom";
			Hue = 2602;
		}

		[Constructable]
		public CopperheadVenom() : this( 1 )
		{
		}

		public CopperheadVenom( Serial serial ) : base( serial )
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
	
	public class AmbusherSpiderVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.IntelligenceDecrease, 50),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 10)
			}; 
		} }

		public override int SkillRequired { get { return 400; } }
		public override int Corrosivity { get { return 0; } }
		public override int ToxinActingSpeed { get { return 1; } }
		public override int ToxinDuration { get { return 360; } }

		[Constructable]
		public AmbusherSpiderVenom( int amount ) : base( 5927, amount )
		{
			Name = "Ambusher Spider Venom";
			Hue = 2626;
		}

		[Constructable]
		public AmbusherSpiderVenom() : this( 1 )
		{
		}

		public AmbusherSpiderVenom( Serial serial ) : base( serial )
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
	
	public class CobraVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 10),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StaminaDecrease, 20),
                new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DexterityDecrease, 20)
			}; 
		} }

		public override int SkillRequired { get { return 450; } }
		public override int Corrosivity { get { return 0; } }
		public override int ToxinActingSpeed { get { return 1; } }
		public override int ToxinDuration { get { return 360; } }

		[Constructable]
		public CobraVenom( int amount ) : base( 3985, amount )
		{
			Name = "Cobra Venom";
			Hue = 2669;
		}

		[Constructable]
		public CobraVenom() : this( 1 )
		{
		}

		public CobraVenom( Serial serial ) : base( serial )
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
	
	public class AssassinSpiderVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageStamina, 30),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 30)
			}; 
		} }

		public override int SkillRequired { get { return 550; } }
		public override int Corrosivity { get { return 1; } }
		public override int ToxinActingSpeed { get { return 2; } }
		public override int ToxinDuration { get { return 180; } }

		[Constructable]
		public AssassinSpiderVenom( int amount ) : base( 3193, amount )
		{
			Name = "Assassin Spider Venom";
			Hue = 2587;
		}

		[Constructable]
		public AssassinSpiderVenom() : this( 1 )
		{
		}

		public AssassinSpiderVenom( Serial serial ) : base( serial )
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
	
	public class WyvernVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageMana, 30),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 30)
			}; 
		} }

		public override int SkillRequired { get { return 600; } }
		public override int Corrosivity { get { return 1; } }
		public override int ToxinActingSpeed { get { return 2; } }
		public override int ToxinDuration { get { return 180; } }

		[Constructable]
		public WyvernVenom( int amount ) : base( 2452, amount )
		{
			Name = "Wyvern Venom";
			Hue = 2702;
		}

		[Constructable]
		public WyvernVenom() : this( 1 )
		{
		}

		public WyvernVenom( Serial serial ) : base( serial )
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
	
	public class BlackMambaVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 60)
			}; 
		} }

		public override int SkillRequired { get { return 700; } }
		public override int Corrosivity { get { return 1; } }
		public override int ToxinActingSpeed { get { return 2; } }
		public override int ToxinDuration { get { return 90; } }

		[Constructable]
		public BlackMambaVenom( int amount ) : base( 2428, amount )
		{
			Name = "Black Mamba Venom";
			Hue = 2685;
		}

		[Constructable]
		public BlackMambaVenom() : this( 1 )
		{
		}

		public BlackMambaVenom( Serial serial ) : base( serial )
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
	
	public class DireSpiderVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageStamina, 60)
			}; 
		} }

		public override int SkillRequired { get { return 750; } }
		public override int Corrosivity { get { return 1; } }
		public override int ToxinActingSpeed { get { return 2; } }
		public override int ToxinDuration { get { return 90; } }

		[Constructable]
		public DireSpiderVenom( int amount ) : base( 3969, amount )
		{
			Name = "Dire Spider Venom";
			Hue = 2963;
		}

		[Constructable]
		public DireSpiderVenom() : this( 1 )
		{
		}

		public DireSpiderVenom( Serial serial ) : base( serial )
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
	
	public class WolfSpiderVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageMana, 60)
			}; 
		} }

		public override int SkillRequired { get { return 850; } }
		public override int Corrosivity { get { return 1; } }
		public override int ToxinActingSpeed { get { return 2; } }
		public override int ToxinDuration { get { return 90; } }

		[Constructable]
		public WolfSpiderVenom( int amount ) : base( 3964, amount )
		{
			Name = "Wolf Spider Venom";
			Hue = 2935;
		}

		[Constructable]
		public WolfSpiderVenom() : this( 1 )
		{
		}

		public WolfSpiderVenom( Serial serial ) : base( serial )
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
	
	public class KingCobraVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 10)
			}; 
		} }

		public override int SkillRequired { get { return 1000; } }
		public override int Corrosivity { get { return 2; } }
		public override int ToxinActingSpeed { get { return 6; } }
		public override int ToxinDuration { get { return 90; } }

		[Constructable]
		public KingCobraVenom( int amount ) : base( 3967, amount )
		{
			Name = "King Cobra Venom";
			Hue = 2938;
		}

		[Constructable]
		public KingCobraVenom() : this( 1 )
		{
		}

		public KingCobraVenom( Serial serial ) : base( serial )
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
	
	public class XornVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageStamina, 10)
			}; 
		} }

		public override int SkillRequired { get { return 1000; } }
		public override int Corrosivity { get { return 2; } }
		public override int ToxinActingSpeed { get { return 6; } }
		public override int ToxinDuration { get { return 90; } }

		[Constructable]
		public XornVenom( int amount ) : base( 3984, amount )
		{
			Name = "Xorn Venom";
			Hue = 2784;
		}

		[Constructable]
		public XornVenom() : this( 1 )
		{
		}

		public XornVenom( Serial serial ) : base( serial )
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
	
	public class LargeScorpionVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StaminaDecrease, 80),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 40)
			}; 
		} }

		public override int SkillRequired { get { return 1000; } }
		public override int Corrosivity { get { return 1; } }
		public override int ToxinActingSpeed { get { return 2; } }
		public override int ToxinDuration { get { return 360; } }

		[Constructable]
		public LargeScorpionVenom( int amount ) : base( 5919, amount )
		{
			Name = "Large Scorpion Sting";
			Hue = 2581;
		}

		[Constructable]
		public LargeScorpionVenom() : this( 1 )
		{
		}

		public LargeScorpionVenom( Serial serial ) : base( serial )
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
	
	public class LargeSnakeVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 120)
			}; 
		} }

		public override int SkillRequired { get { return 1000; } }
		public override int Corrosivity { get { return 2; } }
		public override int ToxinActingSpeed { get { return 2; } }
		public override int ToxinDuration { get { return 10; } }

		[Constructable]
		public LargeSnakeVenom( int amount ) : base( 3189, amount )
		{
			Name = "Large Snake Venom";
			Hue = 2933;
		}

		[Constructable]
		public LargeSnakeVenom() : this( 1 )
		{
		}

		public LargeSnakeVenom( Serial serial ) : base( serial )
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
	
	public class LargeSpiderVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageStamina, 60),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 60)
			}; 
		} }

		public override int SkillRequired { get { return 1000; } }
		public override int Corrosivity { get { return 0; } }
		public override int ToxinActingSpeed { get { return 2; } }
		public override int ToxinDuration { get { return 300; } }

		[Constructable]
		public LargeSpiderVenom( int amount ) : base( 3977, amount )
		{
			Name = "Large Spider Venom";
			Hue = 2788;
		}

		[Constructable]
		public LargeSpiderVenom() : this( 1 )
		{
		}

		public LargeSpiderVenom( Serial serial ) : base( serial )
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
	
	public class LargeWyvernVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageMana, 60),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 60)
			}; 
		} }

		public override int SkillRequired { get { return 1000; } }
		public override int Corrosivity { get { return 0; } }
		public override int ToxinActingSpeed { get { return 2; } }
		public override int ToxinDuration { get { return 180; } }

		[Constructable]
		public LargeWyvernVenom( int amount ) : base( 2452, amount )
		{
			Name = "Large Wyvern Venom";
			Hue = 2964;
		}

		[Constructable]
		public LargeWyvernVenom() : this( 1 )
		{
		}

		public LargeWyvernVenom( Serial serial ) : base( serial )
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
	
	public class LargeInsectVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageStamina, 60)
			}; 
		} }

		public override int SkillRequired { get { return 1000; } }
		public override int Corrosivity { get { return 1; } }
		public override int ToxinActingSpeed { get { return 4; } }
		public override int ToxinDuration { get { return 300; } }

		[Constructable]
		public LargeInsectVenom( int amount ) : base( 3984, amount )
		{
			Name = "Large Insect Venom";
			Hue = 2964;
		}

		[Constructable]
		public LargeInsectVenom() : this( 1 )
		{
		}

		public LargeInsectVenom( Serial serial ) : base( serial )
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
	
	public class ToxicMarrowVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StrengthDecrease, 50),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DexterityDecrease, 50),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 20)
			}; 
		} }

		public override int SkillRequired { get { return 1000; } }
		public override int Corrosivity { get { return 0; } }
		public override int ToxinActingSpeed { get { return 2; } }
		public override int ToxinDuration { get { return 360; } }

		[Constructable]
		public ToxicMarrowVenom( int amount ) : base( 3972, amount )
		{
			Name = "Toxic Marrow Venom";
			Hue = 2966;
		}

		[Constructable]
		public ToxicMarrowVenom() : this( 1 )
		{
		}

		public ToxicMarrowVenom( Serial serial ) : base( serial )
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
	
	public class ToxicFleshVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageStamina, 120),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 120)
			}; 
		} }

		public override int SkillRequired { get { return 1000; } }
		public override int Corrosivity { get { return 4; } }
		public override int ToxinActingSpeed { get { return 1; } }
		public override int ToxinDuration { get { return 360; } }

		[Constructable]
		public ToxicFleshVenom( int amount ) : base( 2545, amount )
		{
			Name = "Toxic Flesh Venom";
			Hue = 2964;
		}

		[Constructable]
		public ToxicFleshVenom() : this( 1 )
		{
		}

		public ToxicFleshVenom( Serial serial ) : base( serial )
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
	
	public class RabidFleshVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.ManaDecrease, 60),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageMana, 60),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.IntelligenceDecrease, 60)
			}; 
		} }

		public override int SkillRequired { get { return 1000; } }
		public override int Corrosivity { get { return 0; } }
		public override int ToxinActingSpeed { get { return 2; } }
		public override int ToxinDuration { get { return 360; } }

		[Constructable]
		public RabidFleshVenom( int amount ) : base( 5641, amount )
		{
			Name = "Rabid Flesh Venom";
			Hue = 2886;
		}

		[Constructable]
		public RabidFleshVenom() : this( 1 )
		{
		}

		public RabidFleshVenom( Serial serial ) : base( serial )
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
	
	public class LargeHydraVenom : BaseToxinIngredient
	{
		public override KeyValuePair<PoisonEffectEnum, int>[] Effects { get { 
			return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.HealthDecrease, 120),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StaminaDecrease, 120),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.ManaDecrease, 120)
			}; 
		} }

		public override int SkillRequired { get { return 1000; } }
		public override int Corrosivity { get { return 6; } }
		public override int ToxinActingSpeed { get { return 1; } }
		public override int ToxinDuration { get { return 360; } }

		[Constructable]
		public LargeHydraVenom( int amount ) : base( 2515, amount )
		{
			Name = "Large Hydra Venom";
			Hue = 2680;
		}

		[Constructable]
		public LargeHydraVenom() : this( 1 )
		{
		}

		public LargeHydraVenom( Serial serial ) : base( serial )
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

    public class StoneFish : BaseToxinIngredient
    {
        public override KeyValuePair<PoisonEffectEnum, int>[] Effects
        {
            get
            {
                return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.HealthDecrease, 30),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StaminaDecrease, 30),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.ManaDecrease, 30)
			};
            }
        }

        public override int SkillRequired { get { return 1000; } }
        public override int Corrosivity { get { return 0; } }
        public override int ToxinActingSpeed { get { return 3; } }
        public override int ToxinDuration { get { return 240; } }

        [Constructable]
        public StoneFish( int amount )
            : base( 2508, amount )
        {
            Name = "Stone Fish";
            Hue = 2701;
        }

        [Constructable]
        public StoneFish()
            : this( 1 )
        {
        }

        public StoneFish( Serial serial )
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

    public class PufferFish : BaseToxinIngredient
    {
        public override KeyValuePair<PoisonEffectEnum, int>[] Effects
        {
            get
            {
                return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StrengthDecrease, 30),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DexterityDecrease, 30),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.IntelligenceDecrease, 30)
			};
            }
        }

        public override int SkillRequired { get { return 800; } }
        public override int Corrosivity { get { return 0; } }
        public override int ToxinActingSpeed { get { return 3; } }
        public override int ToxinDuration { get { return 180; } }

        [Constructable]
        public PufferFish( int amount )
            : base( 3542, amount )
        {
            Name = "Puffer Fish";
            Hue = 2702;
        }

        [Constructable]
        public PufferFish()
            : this( 1 )
        {
        }

        public PufferFish( Serial serial )
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

    public class ViperVenom : BaseToxinIngredient
    {
        public override KeyValuePair<PoisonEffectEnum, int>[] Effects
        {
            get
            {
                return new KeyValuePair<PoisonEffectEnum, int>[] { 
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DamageHealth, 10),
				new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.StrengthDecrease, 20),
                new KeyValuePair<PoisonEffectEnum, int>(PoisonEffectEnum.DexterityDecrease, 20)
			};
            }
        }

        public override int SkillRequired { get { return 450; } }
        public override int Corrosivity { get { return 0; } }
        public override int ToxinActingSpeed { get { return 1; } }
        public override int ToxinDuration { get { return 360; } }

        [Constructable]
        public ViperVenom(int amount)
            : base(3985, amount)
        {
            Name = "Viper Venom";
            Hue = 2669;
        }

        [Constructable]
        public ViperVenom()
            : this(1)
        {
        }

        public ViperVenom(Serial serial)
            : base(serial)
        {
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
