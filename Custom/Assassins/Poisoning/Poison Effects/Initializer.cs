using System;
using Server;
using Server.Items;

namespace Server.Engines.Poisoning
{
	public sealed class Initializer
	{
		public static void Initialize() // All poison effects must be registered here
		{
			PoisonEffect.Register( new StrengthDecreaseEffect(), PoisonEffectEnum.StrengthDecrease );
			PoisonEffect.Register( new DexterityDecreaseEffect(), PoisonEffectEnum.DexterityDecrease );
			PoisonEffect.Register( new IntelligenceDecreaseEffect(), PoisonEffectEnum.IntelligenceDecrease );
			PoisonEffect.Register( new HealthDecreaseEffect(), PoisonEffectEnum.HealthDecrease );
			PoisonEffect.Register( new StaminaDecreaseEffect(), PoisonEffectEnum.StaminaDecrease );
			PoisonEffect.Register( new ManaDecreaseEffect(), PoisonEffectEnum.ManaDecrease );
			
			PoisonEffect.Register( new DamageHealthEffect(), PoisonEffectEnum.DamageHealth );
			PoisonEffect.Register( new DamageStaminaEffect(), PoisonEffectEnum.DamageStamina );
			PoisonEffect.Register( new DamageManaEffect(), PoisonEffectEnum.DamageMana );
		}
	}
}
