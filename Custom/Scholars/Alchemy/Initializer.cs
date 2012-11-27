using System;
using Server;
using Server.Items;

namespace Server.Engines.Alchemy
{
	public sealed class Initializer
	{
		public static void Initialize() // All custom potion effects must be registered here
		{
			CustomPotionEffect.Register( new HitPointRestorationEffect(), CustomEffect.HitPointRestoration );
			CustomPotionEffect.Register( new StaminaRestorationEffect(), CustomEffect.StaminaRestoration );
			CustomPotionEffect.Register( new ManaRestorationEffect(), CustomEffect.ManaRestoration );
			CustomPotionEffect.Register( new CureEffect(), CustomEffect.Cure );
			CustomPotionEffect.Register( new HitPointRegenerationEffect(), CustomEffect.HitPointRegeneration );
			CustomPotionEffect.Register( new StaminaRegenerationEffect(), CustomEffect.StaminaRegeneration );
			CustomPotionEffect.Register( new ManaRegenerationEffect(), CustomEffect.ManaRegeneration );
			CustomPotionEffect.Register( new DexterityEffect(), CustomEffect.Dexterity );
			CustomPotionEffect.Register( new StrengthEffect(), CustomEffect.Strength );
			CustomPotionEffect.Register( new HitPointsEffect(), CustomEffect.HitPoints );
			CustomPotionEffect.Register( new IntelligenceEffect(), CustomEffect.Intelligence );
			CustomPotionEffect.Register( new ManaEffect(), CustomEffect.Mana );
			CustomPotionEffect.Register( new StaminaEffect(), CustomEffect.Stamina );
			CustomPotionEffect.Register( new ExplosionEffect(), CustomEffect.Explosion );
			CustomPotionEffect.Register( new SmokeEffect(), CustomEffect.Smoke );
			CustomPotionEffect.Register( new StickyGooEffect(), CustomEffect.StickyGoo );
			CustomPotionEffect.Register( new FireEffect(), CustomEffect.Fire );
			CustomPotionEffect.Register( new ThirstEffect(), CustomEffect.Thirst );
			CustomPotionEffect.Register( new FlashEffect(), CustomEffect.Flash );
			CustomPotionEffect.Register( new ParalysisEffect(), CustomEffect.Paralysis );
			CustomPotionEffect.Register( new HungerEffect(), CustomEffect.Hunger );
			CustomPotionEffect.Register( new RustEffect(), CustomEffect.Rust );
			CustomPotionEffect.Register( new AdhesiveEffect(), CustomEffect.Adhesive );
			CustomPotionEffect.Register( new ShrapnelEffect(), CustomEffect.Shrapnel );
            CustomPotionEffect.Register( new ImprovedVisionEffect(), CustomEffect.ImprovedVision );
            CustomPotionEffect.Register( new OintmentEffect(), CustomEffect.Ointment );
            CustomPotionEffect.Register( new MadnessEffect(), CustomEffect.Madness );
            CustomPotionEffect.Register( new ConfusionEffect(), CustomEffect.Confusion );
            CustomPotionEffect.Register(new InfluenzaCureEffect(), CustomEffect.InfluenzaCure);

		}
	}
}
