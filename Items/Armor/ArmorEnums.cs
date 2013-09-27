using System;

namespace Server.Items
{
	public enum ArmorQuality
	{
		Low = 1,
		Regular = 3,
		Exceptional = 5,
		Masterwork = 9,
		Extraordinary = 7,
        Poor = 0,
        Inferior = 2,
        Superior = 4,
        Remarkable = 6,
        Antique = 8,
        Legendary = 10
	}

	public enum ArmorDurabilityLevel
	{
		Regular,
		Durable,
		Substantial,
		Massive,
		Fortified,
		Indestructible
	}

	public enum ArmorProtectionLevel
	{
		Regular,
		Defense,
		Guarding,
		Hardening,
		Fortification,
		Invulnerability,
	}

	public enum ArmorBodyType
	{
		Gorget,
		Gloves,
		Helmet,
		Arms,
		Legs, 
		Chest,
		Shield
	}

	public enum ArmorMaterialType
	{
		Cloth,
		Leather,
		Studded,
		Bone,
		Thick,
		Beast,
		Scaled,
		Ringmail,
		Chainmail,
		Plate,
		Dragon	// On OSI, Dragon is seen and considered its own type.
	}

	public enum ArmorMeditationAllowance
	{
		All,
		Half,
		None
	}
}
