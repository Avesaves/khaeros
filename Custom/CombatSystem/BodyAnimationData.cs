using System;
using Server;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.BodyAnimationData
{
	public sealed class Initializer
	{
		public static void Initialize()
		{ // humans do not use these, just for monsters/animals/etc
			// Format: BodyID, ActionType, AnimationID, AnimationFrame
			
			// Body number 1 Orge
			BAData.Register( 1, AttackType.Overhead, 5 );
			BAData.Register( 1, AttackType.Swing, 12 );
			// Thrust NA

			// Body number 2 Double headed Orge
			BAData.Register( 2, AttackType.Overhead, 5 );
			BAData.Register( 2, AttackType.Swing, 12 );
			// Thrust NA

			// Body number 3 Zombie
			BAData.Register( 3, AttackType.Overhead, 6 );
			BAData.Register( 3, AttackType.Swing, 12 );
			// Thrust NA


			// Body number 4 Yuan-Ti Boss REALLY IFFY
			BAData.Register( 4, AttackType.Overhead, 6 );
			BAData.Register( 4, AttackType.Swing, 4 );
			// Thrust NA

            // Body number 5 Eagle animation
            BAData.Register( 5, AttackType.Swing, 21 );
            BAData.Register( 5, AttackType.Thrust, 4 );
            BAData.Register( 5, AttackType.Overhead, 20) ;

			// Body number 7 Blue Woman with 4 arms and blue hair beastly thing
			BAData.Register( 7, AttackType.Overhead, 12 );
			// Thrust NA, Swing NA

			// Body number 8 Vines, three of them
			BAData.Register( 8, AttackType.Swing, 4 );
			// Thrust NA, Overhead NA

			// Body number 9 Balron
			BAData.Register( 9, AttackType.Overhead, 5 );
			BAData.Register( 9, AttackType.Thrust, 12 );
			BAData.Register( 9, AttackType.Swing, 4 );


			// Body number 10 Flaming Spirit
			BAData.Register( 10, AttackType.Overhead, 5 );
			BAData.Register( 10, AttackType.Swing, 4 );
			//Thrust NA

			// Body number 11 Blue ass spider, Funnel? 
			BAData.Register( 11, AttackType.Overhead, 4 );
			BAData.Register( 11, AttackType.Thrust, 10 );
			// Swing NA

			// Body number 12 Dragon
			BAData.Register( 12, AttackType.Thrust, 4 );
			BAData.Register( 12, AttackType.Swing, 5 );
			//Overhead NA

			// Body number 13 Air Element
			BAData.Register( 13, AttackType.Overhead, 6 );
			BAData.Register( 13, AttackType.Swing, 4 );
			//Thrust NA and Animation 12 is just frickin crazy, like the wind gives birth to the element

			// Body number 14 Earth Element
			BAData.Register( 14, AttackType.Overhead, 4 );
			BAData.Register( 14, AttackType.Swing, 5 );
			//Thrust NA 

			// Body number 15 Fire Element
			BAData.Register( 15, AttackType.Overhead, 6 );
			BAData.Register( 15, AttackType.Swing, 4 );
			//Thrust NA

			// Body number 24 Big Beastman
			BAData.Register( 24, AttackType.Overhead, 4 );
			BAData.Register( 24, AttackType.Swing, 5 );
			BAData.Register( 24, AttackType.Thrust, 6 );
			
			// Body number 16 Water Elemental
			BAData.Register( 16, AttackType.Swing, 4 );
			// This body has 2 swings and 2 overheads, but no thrust!

			// Body number 17 Orc
			BAData.Register( 17, AttackType.Overhead, 6 );
			BAData.Register( 17, AttackType.Swing, 4 );
			BAData.Register( 17, AttackType.Thrust, 5 );

			// Body number 18 Ettin
			BAData.Register( 18, AttackType.Overhead, 5 );
			BAData.Register( 18, AttackType.Swing, 4 );

			// Body number 19 Giant Spider of some Kind
			BAData.Register( 19, AttackType.Thrust, 4 );
			// Body Number 20 has the same animations.

			// Body number 21 Giant Snake
			BAData.Register( 21, AttackType.Thrust, 4 );

			// Body number 22 Gazer has no usable animations (Just flashy light things)

			// Body number 23 Wolf
			BAData.Register( 23, AttackType.Overhead, 6 );
			BAData.Register( 23, AttackType.Thrust, 5 );
			
			// Body number 25 Wolf
			BAData.Register( 25, AttackType.Overhead, 6 );
			BAData.Register( 25, AttackType.Thrust, 5 );

			// Body number 26 Wraith
			BAData.Register( 26, AttackType.Overhead, 6 );
			BAData.Register( 26, AttackType.Swing, 5 );
			BAData.Register( 26, AttackType.Thrust, 4 );


			// Body number 27 Wolf
			BAData.Register( 27, AttackType.Overhead, 6 );
			BAData.Register( 27, AttackType.Thrust, 5 );

			// Body number 28 Giant Wolf spider?
			BAData.Register( 28, AttackType.Overhead, 4 );
			BAData.Register( 28, AttackType.Swing, 5 );
			BAData.Register( 28, AttackType.Thrust, 6 );

			// Body number 29 Gorilla
			BAData.Register( 29, AttackType.Swing, 5 );

			// Body number 30 Beastman
			BAData.Register( 30, AttackType.Overhead, 5 );
			BAData.Register( 30, AttackType.Swing, 4 );
			
			// Body number 31 Worm with legs?
			BAData.Register( 31, AttackType.Overhead, 4 );
			BAData.Register( 31, AttackType.Swing, 5 );

			// Body number 33 Crystal Golem ?
			BAData.Register( 33, AttackType.Overhead, 4 );

			// Body number 34 Grey wolf 
			BAData.Register( 34, AttackType.Thrust, 5 );

			// Body number 35 Japanese Demon Samurai
			BAData.Register( 35, AttackType.Overhead, 4 );
			BAData.Register( 35, AttackType.Swing, 5 );

			// Body number 36 Storm Giant 
			BAData.Register( 36, AttackType.Overhead, 4 );

			// Body number 37 Wolf again
			BAData.Register( 37, AttackType.Thrust, 5 );

			// Body number 38 Mhordul Troll Guard
			BAData.Register( 38, AttackType.Overhead, 4 );
			BAData.Register( 38, AttackType.Swing, 5 );

			// Body number 39 The Viking God Odin Himself?
			BAData.Register( 39, AttackType.Overhead, 4 );
			BAData.Register( 39, AttackType.Swing, 5 );

			// Body number 40 Some ghostly girl thing, i think it's a magic user
			BAData.Register( 40, AttackType.Swing, 4 );
			BAData.Register( 40, AttackType.Thrust, 5 );

			// Body number 41 Giant White blob with eyes
			BAData.Register( 41, AttackType.Thrust, 4 );

			// Body number 42 Archer Goatman no melee

			// Body number 43 Giant Horned bettle?
			BAData.Register( 43, AttackType.Thrust, 4 );

			// Body number 44 Goat Macer
			BAData.Register( 44, AttackType.Overhead, 4 );
			BAData.Register( 44, AttackType.Swing, 5 );

			// Body number 45 I Don't Know, a Yuan-Ti Wanna be spear blue guy?
			BAData.Register( 45, AttackType.Thrust, 4 );
			BAData.Register( 45, AttackType.Swing, 5 );

			// Body number 46 Ancient Dragon
			BAData.Register( 46, AttackType.Thrust, 4 );
			BAData.Register( 46, AttackType.Overhead, 5 );

			// Body number 47 Haunted Tree
			BAData.Register( 47, AttackType.Swing, 4 );
			BAData.Register( 47, AttackType.Overhead, 5 ); // 5 is a different animation then 6
			// Body number 48 Scorpion 
			BAData.Register( 48, AttackType.Overhead, 4 );
			BAData.Register( 48, AttackType.Swing, 5 );

			// Body number 49 Dragon 
			BAData.Register( 49, AttackType.Thrust, 4 );
			BAData.Register( 49, AttackType.Swing, 5 );

			// Body number 50 Skeleton
			BAData.Register( 50, AttackType.Overhead, 4 );
			BAData.Register( 50, AttackType.Swing, 5 );
			
			// Body number 51 Slime
			BAData.Register( 51, AttackType.Thrust, 4 );

			// Body number 52 Small Snake
			BAData.Register( 52, AttackType.Thrust, 5 );

			// Body number 53 Troll with axe
			BAData.Register( 53, AttackType.Overhead, 5 );
			BAData.Register( 53, AttackType.Swing, 4 );

			// Body number 54 Troll no axe
			BAData.Register( 54, AttackType.Overhead, 5 );
			BAData.Register( 54, AttackType.Swing, 4 );

			// Body number 55 Ice Troll, with axe
			BAData.Register( 55, AttackType.Overhead, 5 );
			BAData.Register( 55, AttackType.Swing, 4 );

			// Body number 56 Skeleton with axe
			BAData.Register( 56, AttackType.Overhead, 4 );
			BAData.Register( 56, AttackType.Swing, 5 );

			// Body number 57 Skeleton with sword and shield
			BAData.Register( 57, AttackType.Overhead, 4 );
			BAData.Register( 57, AttackType.Swing, 5 );

			// Body number 58 Wisp
			//All it does is expand, probably for magic

			// Body number 59 Dragon
			BAData.Register( 59, AttackType.Thrust, 4 );
			BAData.Register( 59, AttackType.Swing, 5 );

			// Body number 60 Drake
			BAData.Register( 60, AttackType.Thrust, 4 );
			BAData.Register( 60, AttackType.Swing, 5 );

			// Body number 61 Red Drake
			BAData.Register( 61, AttackType.Thrust, 4 );
			BAData.Register( 61, AttackType.Swing, 5 );

			// Body number 62 Wyvern 
			BAData.Register( 62, AttackType.Swing, 4 );

			// Body number 63 Panther
			BAData.Register( 63, AttackType.Thrust, 5 );
			BAData.Register( 63, AttackType.Overhead, 6 );

			// Body number 64 Panther
			BAData.Register( 64, AttackType.Thrust, 5 );
			BAData.Register( 64, AttackType.Overhead, 6 ); 

			// Body number 65 Panther
			BAData.Register( 65, AttackType.Thrust, 5 );
			BAData.Register( 65, AttackType.Overhead, 6 ); 

			// Body number 66 Vines
			BAData.Register( 66, AttackType.Swing, 4 );

			// Body number 67 Umberhulk
			BAData.Register( 67, AttackType.Thrust, 4 );
			BAData.Register( 67, AttackType.Swing, 5 );

			// Body number 68 Beholder
			// Lots of flashy animations for magic

			// Body number 69 Beholder
			// Lots of flashy animations for magic

			// Body number 70 Green Spider/Ant thing forgot what its called
			BAData.Register( 70, AttackType.Thrust, 6 );
			BAData.Register( 70, AttackType.Swing, 4 );
			BAData.Register( 70, AttackType.Overhead, 11 );

			// Body number 71 Same as 70 different colour and size
			BAData.Register( 71, AttackType.Thrust, 6 );
			BAData.Register( 71, AttackType.Swing, 4 );
			BAData.Register( 71, AttackType.Overhead, 11 );

			// Body number 72 Same as 71
			BAData.Register( 72, AttackType.Thrust, 6 );
			BAData.Register( 72, AttackType.Swing, 4 );
			BAData.Register( 72, AttackType.Overhead, 11 );

			// Body number 73 Rock Troll sword creature
			BAData.Register( 73, AttackType.Swing, 4 );

			// Body number 74 Gremlin 
			//Just stands there and breathes 

			// Body number 75 Cyclops with hammer
			BAData.Register( 75, AttackType.Swing, 4 );
			BAData.Register( 75, AttackType.Overhead, 5 );
			
			// Body number 76 Titan
			BAData.Register( 76, AttackType.Overhead, 4 );


			// Body number 77 Kraken 
			BAData.Register( 77, AttackType.Overhead, 4 ); //Overhead attack or casting animation


			// Body number 78 Rock Troll with sword, bigger than 73
			BAData.Register( 78, AttackType.Overhead, 4 );
			BAData.Register( 78, AttackType.Swing, 5 );

			//Body number 79 = 78


			// Body number 80 Toad
			BAData.Register( 80, AttackType.Thrust, 4 );
			BAData.Register( 80, AttackType.Overhead, 5 ); 

			// Body number 81 Baby Toad
			BAData.Register( 81, AttackType.Thrust, 5 );

			// Body number 82 = body value 79

			//Body number 83 = Body number 1

			//Body number 84 = Body number 1

			// Body number 85 Yuan-Ti Mage
			BAData.Register( 85, AttackType.Swing, 4 );
			BAData.Register( 85, AttackType.Thrust, 5 );
			BAData.Register( 85, AttackType.Overhead, 6 );

			// Body number 86 Yuan-Ti Soldier 
			BAData.Register( 86, AttackType.Swing, 4 );
			BAData.Register( 86, AttackType.Thrust, 5 );
			BAData.Register( 86, AttackType.Overhead, 6 );

			// Body number 87 Yuan-Ti no weapon
			BAData.Register( 87, AttackType.Overhead, 4 );
			BAData.Register( 87, AttackType.Thrust, 5 );
			BAData.Register( 87, AttackType.Swing, 6 );

			// Body number 88 Goat
			BAData.Register( 88, AttackType.Thrust, 5 );

			// Body number 89 Giant Ice Snake, same as 21 but there is an extra animation
			BAData.Register( 89, AttackType.Thrust, 4 );

			//Body numbers 90,91,92 and 93 are all the same snake but different colour

			//Body number 94 = Body number 51

			//Body number 95 = Nothing

			//Body number 96 = Body number 94

			//Body number 100,99,98 and 97 are all wolf animations of the body value 34

			// Body number 101 Centaur 
			BAData.Register( 101, AttackType.Swing, 4 );
			BAData.Register( 101, AttackType.Thrust, 5 ); 

			// Body number 102 Gnoll
			BAData.Register( 102, AttackType.Swing, 4 );

			// Body number 103 Beholder
			BAData.Register( 103, AttackType.Overhead, 5 );
			BAData.Register( 103, AttackType.Thrust, 6 );

			// Body number 104 Skeleton Drake
			BAData.Register( 104, AttackType.Thrust, 4 );

			//Body Number 105 and 106 = 59 Body value of a dragon

			//Body number 107,108,109,110,111,112 and 113 = Body Value 14, Earth Element just different colours

			//Body number 114 Horse
			BAData.Register( 114, AttackType.Overhead, 5 );
			BAData.Register( 114, AttackType.Thrust, 6 ); 
			
			//Body number 115 Horse
			BAData.Register( 115, AttackType.Overhead, 5 );
			BAData.Register( 115, AttackType.Thrust, 6 );
			
			//Body number 116 Horse
			BAData.Register( 116, AttackType.Overhead, 5 );
			BAData.Register( 116, AttackType.Thrust, 6 );
			
			//Body number 117 Horse
			BAData.Register( 117, AttackType.Overhead, 5 );
			BAData.Register( 117, AttackType.Thrust, 6 );
			
			//Body number 118 Horse
			BAData.Register( 118, AttackType.Overhead, 5 );
			BAData.Register( 118, AttackType.Thrust, 6 );
			
			//Body number 119 Horse
			BAData.Register( 119, AttackType.Overhead, 5 );
			BAData.Register( 119, AttackType.Thrust, 6 );
			
			//Body number 120 Horse
			BAData.Register( 120, AttackType.Overhead, 5 );
			BAData.Register( 120, AttackType.Thrust, 6 );
			
			//Body number 121 Horse
			BAData.Register( 121, AttackType.Overhead, 5 );
			BAData.Register( 121, AttackType.Thrust, 6 );

			//Body number 122 Unicorn
			BAData.Register( 122, AttackType.Thrust, 4 );

			//Body number 123 Angel Guardian thing
			BAData.Register( 123, AttackType.Swing, 4 );

			//Body number 124 Yeti ???
			BAData.Register( 124, AttackType.Overhead, 4 );
			BAData.Register( 124, AttackType.Swing, 5 );

			//Body number 125 Jungle Tiger
			BAData.Register( 125, AttackType.Swing, 4 ); //It's either a parry thrust or a swing attack depending on the angle you look at it from
			BAData.Register( 125, AttackType.Overhead, 5 );
			BAData.Register( 125, AttackType.Thrust, 6 );
			
			// Body number 126 Sabre Tooth
			BAData.Register( 126, AttackType.Overhead, 5 );
			BAData.Register( 126, AttackType.Swing, 4 );
			BAData.Register( 126, AttackType.Thrust, 6 ); 

			// Body number 127 Panther
			BAData.Register( 127, AttackType.Overhead, 6 );
			BAData.Register( 127, AttackType.Swing, 7 );
			BAData.Register( 127, AttackType.Thrust, 5 ); 

			// Body number 128 Faerie
			BAData.Register( 128, AttackType.Overhead, 5 );

			// Body number 129 Vine Thing 
			// This is a REALLY weird graphic. All the animations are just attacks in different directions, but they appear to be the same attack. I don't think they are usable..

			// Body number 130 Cave Troll 
			BAData.Register( 130, AttackType.Overhead, 4 );
			BAData.Register( 130, AttackType.Swing, 5 );

			// Body number 131 Air Elemental
			BAData.Register( 131, AttackType.Overhead, 6 );
			BAData.Register( 131, AttackType.Swing, 4 );

			// Body number 132 Eyestalk
			BAData.Register( 132, AttackType.Overhead, 5 );
			BAData.Register( 132, AttackType.Swing, 6 );
			BAData.Register( 132, AttackType.Thrust, 4 ); 

			// Body number 133 Aligator
			BAData.Register( 133, AttackType.Swing, 4 );
			BAData.Register( 133, AttackType.Thrust, 5 ); 

			// Body number 134 Glitchy Lizard
			// Animations can't be used, they clip all kinds.

			// Body number 135 Ogre
			BAData.Register( 135, AttackType.Overhead, 5 );
			BAData.Register( 135, AttackType.Swing, 4 );
			BAData.Register( 135, AttackType.Thrust, 10 ); 

			// Body number 136 Yuan-ti Magi
			BAData.Register( 136, AttackType.Overhead, 6 );
			BAData.Register( 136, AttackType.Swing, 4 );
			BAData.Register( 136, AttackType.Thrust, 5 ); 

			// Body number 137 Yuan-ti Warrior
			BAData.Register( 137, AttackType.Overhead, 6 );
			BAData.Register( 137, AttackType.Swing, 4 );
			BAData.Register( 137, AttackType.Thrust, 5 );

			// Body number 138 Andariel
			BAData.Register( 138, AttackType.Overhead, 4 );

			// Body number 139 Andariel
			BAData.Register( 139, AttackType.Overhead, 4 );

			// Body number 140 Orc
			BAData.Register( 140, AttackType.Overhead, 6 );
			BAData.Register( 140, AttackType.Swing, 4 );

			// Body number 141 Human?
			BAData.Register( 141, AttackType.Swing, 4 );
			// Not a usable mobile. All animations 1-10 are the same one handed swing.

			// Body number 142 Yuan-ti Spear Warrior
			BAData.Register( 142, AttackType.Swing, 5 );
			BAData.Register( 142, AttackType.Thrust, 4 );

			// Body number 143 Yuan-ti Warrior
			BAData.Register( 143, AttackType.Swing, 5 );
			BAData.Register( 143, AttackType.Thrust, 4 );

			// Body number 144 Horse
			BAData.Register( 144, AttackType.Overhead, 5 );

			// Body number 145 Giant Sea Serpent
			BAData.Register( 145, AttackType.Overhead, 5 );
			BAData.Register( 145, AttackType.Swing, 7 );
			
			// Body number 146 Harrower
			BAData.Register( 146, AttackType.Swing, 6 );

			// Body number 147 Skeleton
			BAData.Register( 147, AttackType.Overhead, 4 );
			BAData.Register( 147, AttackType.Swing, 6 );

			// Body number 148 Skeleton Lord
			BAData.Register( 148, AttackType.Swing, 6 );

			// Body number 149 Succubus
			BAData.Register( 149, AttackType.Overhead, 5 );

			// Body number 150 Sandworm
			BAData.Register( 150, AttackType.Overhead, 5 );
			BAData.Register( 150, AttackType.Swing, 7 );

			// Body number 151 Dolphin
			//No usable animations

			// Body number 152 Insect Lady
			BAData.Register( 152, AttackType.Overhead, 4 );

			// Body number 153 Ghoul
			BAData.Register( 153, AttackType.Overhead, 6 );
			BAData.Register( 153, AttackType.Swing, 5 );
			BAData.Register( 153, AttackType.Thrust, 4 ); 

			// Body number 154 Mummy
			BAData.Register( 154, AttackType.Overhead, 4 );

			// Body number 155 Green Zombie
			BAData.Register( 155, AttackType.Overhead, 6 );
			BAData.Register( 155, AttackType.Swing, 5 );

			// Body number 156 NOTHING

			// Body number 157 Spider
			BAData.Register( 157, AttackType.Thrust, 4 ); 

			// Body number 158 Acid Elemental
			BAData.Register( 158, AttackType.Overhead, 7 );
			BAData.Register( 158, AttackType.Swing, 6 );

			// Body number 159 Red Elemental
			BAData.Register( 159, AttackType.Overhead, 7 );
			BAData.Register( 159, AttackType.Swing, 6 );

			// Body number 160 Red-Orange Elemental
			BAData.Register( 160, AttackType.Overhead, 7 );
			BAData.Register( 160, AttackType.Swing, 6 );

			// Body number 161 Ice Elemental
			BAData.Register( 161, AttackType.Overhead, 6 );
			BAData.Register( 161, AttackType.Swing, 5 );

			// Body number 162 Green Elemental
			BAData.Register( 162, AttackType.Overhead, 7 );
			BAData.Register( 162, AttackType.Swing, 6 );

			// Body number 163 White Elemental
			BAData.Register( 163, AttackType.Overhead, 7 );
			BAData.Register( 163, AttackType.Swing, 6 );

			// Body number 164 Purple Elemental
			BAData.Register( 164, AttackType.Overhead, 7 );
			BAData.Register( 164, AttackType.Swing, 6 );

			// Body number 165 Twinkle
			//No usable animations

			// Body number 166 Gold Golem
			BAData.Register( 166, AttackType.Overhead, 6 );
			BAData.Register( 166, AttackType.Swing, 5 );

			// Body number 167 Brown Bear
			BAData.Register( 167, AttackType.Overhead, 5 );
			BAData.Register( 167, AttackType.Swing, 7 );
			BAData.Register( 167, AttackType.Thrust, 6 ); 

			// Body number 168 Shadow Elemental
			BAData.Register( 168, AttackType.Overhead, 7 );
			BAData.Register( 168, AttackType.Swing, 4 );

			// Body number 169 Light Blue Beetle
			BAData.Register( 169, AttackType.Overhead, 6 );
			BAData.Register( 169, AttackType.Thrust, 5 ); 

			// Body number 170 Etheral Tiny Goblin
			BAData.Register( 170, AttackType.Overhead, 5 );
			BAData.Register( 170, AttackType.Swing, 6 );

			// Body number 171 Etheral Ostard
			BAData.Register( 171, AttackType.Swing, 5 );
			BAData.Register( 171, AttackType.Thrust, 6 ); 

			// Body number 172 Wyrm
			BAData.Register( 172, AttackType.Overhead, 6 );
			BAData.Register( 172, AttackType.Swing, 4 );

			// Body number 173 Dire Spider
			BAData.Register( 173, AttackType.Overhead, 5 );

			// Body number 174 Big Succubus
			BAData.Register( 174, AttackType.Overhead, 4 );

			// Body number 175 Angel
			BAData.Register( 175, AttackType.Overhead, 4 );
			
			// Body number 176 Great Faerie
			BAData.Register( 176, AttackType.Overhead, 5 );

			// Body number 177 Horse
			BAData.Register( 177, AttackType.Overhead, 5 );

			// Body number 178 Horse
			BAData.Register( 178, AttackType.Overhead, 5 );

			// Body number 179 Horse
			BAData.Register( 179, AttackType.Overhead, 5 );

			// Body number 180 Dragon
			BAData.Register( 180, AttackType.Overhead, 6 );
			BAData.Register( 180, AttackType.Swing, 5 );
			BAData.Register( 180, AttackType.Thrust, 4 );

			// Body number 181 Tribal Orc
			BAData.Register( 181, AttackType.Overhead, 4 );

			// Body number 182 Orc Bomber
			BAData.Register( 182, AttackType.Overhead, 5 );

			// Body number 183 Human male
			// Body number 184 Human female
			// Body number 185 Human male
			// Body number 186 Human female

			// Body number 187 Ridgeback
			BAData.Register( 187, AttackType.Thrust, 6 );

			// Body number 188 Ridgeback
			BAData.Register( 188, AttackType.Thrust, 6 );

			// Body number 189 Sabre Tooth
			BAData.Register( 189, AttackType.Overhead, 5 );

			// Body number 190 Horse
			BAData.Register( 190, AttackType.Overhead, 5 );

			// Body number 191 Eyestalk
			BAData.Register( 191, AttackType.Overhead, 5 );
			BAData.Register( 191, AttackType.Swing, 6 );
			BAData.Register( 191, AttackType.Thrust, 4 );

			// Body number 192 Male Unicorn
			BAData.Register( 192, AttackType.Overhead, 5 );
			BAData.Register( 192, AttackType.Thrust, 3 );

			// Body number 193 Ridgeback
			BAData.Register( 193, AttackType.Thrust, 6 );

			// Body number 194 Greater Water Elemental
			BAData.Register( 194, AttackType.Overhead, 6 );
			BAData.Register( 194, AttackType.Swing, 5 );

			// Body number 195 Eyestalk
			BAData.Register( 195, AttackType.Overhead, 6 );
			BAData.Register( 195, AttackType.Thrust, 5 );

			// Body number 196 Lesser Air Elemental
			BAData.Register( 196, AttackType.Overhead, 6 );
			BAData.Register( 196, AttackType.Swing, 5 );

			//Body number 197 No animation
			//Body number 198 No animation

			// Body number 199 Badass Crystal Elemental
			BAData.Register( 199, AttackType.Overhead, 6 );
			BAData.Register( 199, AttackType.Swing, 4 );

			// Body number 200 Horse
			BAData.Register( 200, AttackType.Overhead, 5 );

			// Body number 201 Kitty
			BAData.Register( 201, AttackType.Swing, 6 );
			BAData.Register( 201, AttackType.Thrust, 5 );

			// Body number 202 Gator
			BAData.Register( 202, AttackType.Swing, 6 );
			BAData.Register( 202, AttackType.Thrust, 5 );

			// Body number 203 Pig
			BAData.Register( 203, AttackType.Thrust, 6 );

			// Body number 204 Horse
			BAData.Register( 204, AttackType.Overhead, 5 );

			// Body number 205 Bunny
			BAData.Register( 205, AttackType.Thrust, 6 );

			// Body number 206 Non-glitchy Lizard
			BAData.Register( 206, AttackType.Swing, 5 );
			BAData.Register( 206, AttackType.Thrust, 6 );
			//Same as body 134 only it actually works here.

			// Body number 207 Sheep
			BAData.Register( 207, AttackType.Thrust, 5 );

			// Body number 208 Chicken
			BAData.Register( 208, AttackType.Swing, 6 );
			BAData.Register( 208, AttackType.Thrust, 5 );

			// Body number 209 Goat
			BAData.Register( 209, AttackType.Thrust, 6 );

			// Body number 210 Butcher
			BAData.Register( 210, AttackType.Overhead, 5 );
			
			// Body number 211 Bear
			BAData.Register( 211, AttackType.Overhead, 5 );
			BAData.Register( 211, AttackType.Swing, 7 );
			BAData.Register( 211, AttackType.Thrust, 6 ); 

			// Body number 212 Grizzly Bear
			BAData.Register( 212, AttackType.Overhead, 5 );
			BAData.Register( 212, AttackType.Swing, 7 );
			BAData.Register( 212, AttackType.Thrust, 6 ); 

			// Body number 213 Polar Bear
			BAData.Register( 213, AttackType.Overhead, 5 );
			BAData.Register( 213, AttackType.Swing, 7 );
			BAData.Register( 213, AttackType.Thrust, 6 ); 

			// Body number 214 Cougar
			BAData.Register( 214, AttackType.Overhead, 6 );
			BAData.Register( 214, AttackType.Swing, 7 );
			BAData.Register( 214, AttackType.Thrust, 5 ); 

			// Body number 215 Large Rat
			BAData.Register( 215, AttackType.Overhead, 5 );
			BAData.Register( 215, AttackType.Thrust, 6 ); 

			// Body number 216 Cow
			//No usable animations

			// Body number 217 Dog
			BAData.Register( 217, AttackType.Overhead, 6 );

			// Body number 218 Frenzied Ostard
			BAData.Register( 218, AttackType.Swing, 5 );
			BAData.Register( 218, AttackType.Thrust, 6 ); 

			// Body number 219 Desert Ostard
			BAData.Register( 219, AttackType.Swing, 5 );
			BAData.Register( 219, AttackType.Thrust, 6 ); 

			// Body number 220 Goblin
			BAData.Register( 220, AttackType.Overhead, 5 );
			BAData.Register( 220, AttackType.Swing, 6 );

			// Body number 221 Walrus
			BAData.Register( 221, AttackType.Overhead, 6 );
			BAData.Register( 221, AttackType.Swing, 7 );

			// Body number 222 Big Red Demon -Standing animation might be messed up
			BAData.Register( 222, AttackType.Overhead, 4 );
			BAData.Register( 222, AttackType.Swing, 5 );

			// Body number 223 Sheared Sheep
			//Has no usable animations

			// Body number 224 Nothing

			// Body number 225 Wolf
			BAData.Register( 225, AttackType.Overhead, 6 );

			// Body number 226 Horse
			BAData.Register( 226, AttackType.Overhead, 5 );

			// Body number 227 Nothing

			// Body number 228 Horse
			BAData.Register( 228, AttackType.Overhead, 5 );

			// Body number 229 Nothing

			// Body number 230 Bugged out Tree
			//No usable animations
			
			// Body number 231 Cow
			//No usable animations

			// Body number 232 Bull
			//No usable animations

			// Body number 233 Bull
			//No usable animations

			// Body number 234 Elk
			BAData.Register( 234, AttackType.Overhead, 5 );
			BAData.Register( 234, AttackType.Swing, 7 );
			BAData.Register( 234, AttackType.Thrust, 6 ); 

			// Body number 235 Bugged out Ogre
			//No usable animations

			// Body number 236 Skeleton
			BAData.Register( 236, AttackType.Swing, 5 );
			BAData.Register( 236, AttackType.Thrust, 4 ); 

			// Body number 237 Deer
			BAData.Register( 237, AttackType.Swing, 6 );
			BAData.Register( 237, AttackType.Thrust, 5 ); 

			// Body number 238 Rat
			BAData.Register( 238, AttackType.Swing, 6 );
			BAData.Register( 238, AttackType.Thrust, 5 ); 

			// Body number 239 Nothing

			// Body number 240 Kobold
			BAData.Register( 240, AttackType.Swing, 4 );

			// Body number 241 Forest Troll
			BAData.Register( 241, AttackType.Overhead, 4 );
			BAData.Register( 241, AttackType.Swing, 5 );

			// Body number 242 Rust Monster
			BAData.Register( 242, AttackType.Overhead, 4 );
			BAData.Register( 242, AttackType.Thrust, 5 ); 

			// Body number 243 Roquine
			BAData.Register( 243, AttackType.Overhead, 5 );
			BAData.Register( 243, AttackType.Swing, 7 );
			BAData.Register( 243, AttackType.Thrust, 6 ); 

			// Body number 244 Giant Beetle
			BAData.Register( 244, AttackType.Overhead, 5 );
			BAData.Register( 244, AttackType.Thrust, 4 ); 

			// Body number 245 Kobold
			BAData.Register( 245, AttackType.Overhead, 4 );
			BAData.Register( 245, AttackType.Swing, 5 );
			BAData.Register( 245, AttackType.Thrust, 6 ); 

			// Body number 246 Displacer Beast
			BAData.Register( 246, AttackType.Overhead, 6 );
			BAData.Register( 246, AttackType.Swing, 7 );
			BAData.Register( 246, AttackType.Thrust, 5 ); 

			// Body number 247 Goat Demon
			BAData.Register( 247, AttackType.Swing, 6 );
			BAData.Register( 247, AttackType.Thrust, 5 ); 

			// Body number 248 Drox
			BAData.Register( 248, AttackType.Overhead, 6 );
			BAData.Register( 248, AttackType.Swing, 7 );
			BAData.Register( 248, AttackType.Thrust, 5 ); 

			// Body number 249 Minotaur Slug
			BAData.Register( 249, AttackType.Swing, 5 );
			BAData.Register( 249, AttackType.Thrust, 4 ); 

			// Body number 250 Wraith
			BAData.Register( 250, AttackType.Overhead, 4 );
			BAData.Register( 250, AttackType.Swing, 5 );

			// Body number 251 Sabertooth
			BAData.Register( 251, AttackType.Overhead, 6 );
			BAData.Register( 251, AttackType.Swing, 5 );
			BAData.Register( 251, AttackType.Thrust, 4 ); 

			// Body number 252 Floating Witch
			BAData.Register( 252, AttackType.Overhead, 6 );
			BAData.Register( 252, AttackType.Swing, 5 );
			BAData.Register( 252, AttackType.Thrust, 6 ); 

			// Body number 253 Kobold Lord
			BAData.Register( 253, AttackType.Overhead, 6 );
			BAData.Register( 253, AttackType.Swing, 5 );

			// Body number 254 Blue Flamingo
			//No usable animations

			// Body number 255 Old Man Kobold
			BAData.Register( 255, AttackType.Overhead, 4 );
			BAData.Register( 255, AttackType.Swing, 6 );

			// Body number 256 Famine Spirit
			BAData.Register( 256, AttackType.Overhead, 5 );
			BAData.Register( 256, AttackType.Swing, 4 );
			BAData.Register( 256, AttackType.Thrust, 6 ); 

			// Body number 257 Unicorn
			BAData.Register( 257, AttackType.Overhead, 4 );
			BAData.Register( 257, AttackType.Swing, 5 );

			// Body number 258 Dryad
			BAData.Register( 258, AttackType.Overhead, 5 );
			BAData.Register( 258, AttackType.Swing, 6 );

			// Body number 259 Minotaur Zombie
			BAData.Register( 259, AttackType.Overhead, 6 );
			BAData.Register( 259, AttackType.Swing, 5 );

			// Body number 260 Blue Blob
			//No usable animations

			// Body number 261 Blue Blob
			//No usable animations

			// Body number 262 Minotaur Cleric
			BAData.Register( 262, AttackType.Overhead, 6 );
			BAData.Register( 262, AttackType.Swing, 4 );

			// Body number 263 Minotaur
			BAData.Register( 263, AttackType.Swing, 5 );
			BAData.Register( 263, AttackType.Thrust, 4 ); 

			// Body number 264 Petal
			BAData.Register( 264, AttackType.Thrust, 4 ); 

			// Body number 265 Hydra
			BAData.Register( 265, AttackType.Overhead, 5 );
			BAData.Register( 265, AttackType.Thrust, 4 ); 

			// Body number 266 Dryad
			BAData.Register( 266, AttackType.Overhead, 5 );
			BAData.Register( 266, AttackType.Swing, 6 );

			// Body number 267 Mountain Troll
			BAData.Register( 267, AttackType.Overhead, 6 );
			BAData.Register( 267, AttackType.Swing, 5 );

			// Body number 268 Nothing

			// Body number 269 Satyr Bugged
			//No usable animations

			// Body number 270 Satyr Bugged
			//No usable animations

			// Body number 271 Satyr
			BAData.Register( 271, AttackType.Swing, 6 );
			BAData.Register( 271, AttackType.Thrust, 5 ); 

			// Body number 272 Green Blob
			//No usable animations

			// Body number 273 Green Blob
			//No usable animations

			// Body number 274 Nothing

			// Body number 275 Nothing

			// Body number 276 Chymera
			BAData.Register( 276, AttackType.Overhead, 5 );
			BAData.Register( 276, AttackType.Thrust, 6 ); 

			// Body number 277 White Dire Wolf
			BAData.Register( 277, AttackType.Overhead, 6 );
			BAData.Register( 277, AttackType.Thrust, 5 ); 

			// Body number 278 Squirrel
			BAData.Register( 278, AttackType.Thrust, 6 ); 

			// Body number 279 Racoon
			BAData.Register( 279, AttackType.Thrust, 5 ); 

			// Body number 280 Armoured Minotaur 1
			BAData.Register( 280, AttackType.Overhead, 5 );
			BAData.Register( 280, AttackType.Swing, 4 );

			// Body number 281 Armoured Minotaur 2
			BAData.Register( 281, AttackType.Overhead, 6 );
			BAData.Register( 281, AttackType.Swing, 5 );

			// Body number 282 Butcher
			BAData.Register( 282, AttackType.Overhead, 5 );

			// Body number 283 Earth Elemental
			BAData.Register( 283, AttackType.Overhead, 5 );
			BAData.Register( 283, AttackType.Swing, 6 );

			// Body number 284 Armoured Horse
			BAData.Register( 284, AttackType.Overhead, 5 );

			// Body number 285 Tree Ent
			BAData.Register( 285, AttackType.Overhead, 6 );
			BAData.Register( 285, AttackType.Swing, 4 );
			BAData.Register( 285, AttackType.Thrust, 5 ); 

			// Body number 286 Goblin
			BAData.Register( 286, AttackType.Swing, 5 );

			// Body number 287 Goblin Archer
			BAData.RegisterRanged( 287, 5 );

			// Body number 288 Nothing
			BAData.Register( 288, AttackType.Overhead, 5 );
			BAData.Register( 288, AttackType.Swing, 7 );
			BAData.Register( 288, AttackType.Thrust, 6 ); 

			// Body number 289 Nothing

			// Body number 290 Pig
			BAData.Register( 290, AttackType.Thrust, 5 ); 

			// Body number 291 Pack Horse
			BAData.Register( 291, AttackType.Overhead, 5 );

			// Body number 292 Goblin Archer
			BAData.RegisterRanged( 292, 5 );

			// Body number 293 Nothing 

			// Body number 294 Nothing

			// Body number 295 Dolphin
			//No usable animations

			// Body number 296 Nothing

			// Body number 297 Nothing

			// Body number 298 Nothing

			// Body number 299 Nothing

			// Body number 300 Crystal Elemental
			BAData.Register( 300, AttackType.Swing, 5 );
			BAData.Register( 300, AttackType.Thrust, 4 ); 

			// Body number 301 Wood Fellow
			BAData.Register( 301, AttackType.Swing, 5 );
			BAData.Register( 301, AttackType.Thrust, 4 ); 

			// Body number 302 Skeleton King
			BAData.Register( 302, AttackType.Overhead, 4 );

			// Body number 303 Chest Mouth Monster
			BAData.Register( 303, AttackType.Overhead, 5 );
			BAData.Register( 303, AttackType.Swing, 4 );

			// Body number 304 Flesh Golem
			BAData.Register( 304, AttackType.Overhead, 4 );
			BAData.Register( 304, AttackType.Swing, 5 );

			// Body number 305 Long Neck Abomination
			BAData.Register( 305, AttackType.Overhead, 5 );
			BAData.Register( 305, AttackType.Swing, 6 );
			BAData.Register( 305, AttackType.Thrust, 4 ); 

			// Body number 306 Hook Horror
			BAData.Register( 306, AttackType.Overhead, 6 );
			BAData.Register( 306, AttackType.Swing, 5 );

			// Body number 307 Gibberling
			BAData.Register( 307, AttackType.Overhead, 4 );
			BAData.Register( 307, AttackType.Swing, 5 );

			// Body number 308 Giant Bone Golem
			BAData.Register( 308, AttackType.Swing, 4 );
			BAData.Register( 308, AttackType.Thrust, 5 ); 

			// Body number 309 Patchwork Skeleton
			BAData.Register( 309, AttackType.Overhead, 6 );
			BAData.Register( 309, AttackType.Swing, 5 );

			// Body number 310 Witch
			BAData.Register( 310, AttackType.Overhead, 5 );
			BAData.Register( 310, AttackType.Thrust, 4 ); 

			// Body number 311 Stone Giant
			BAData.Register( 311, AttackType.Overhead, 4 );
			BAData.Register( 311, AttackType.Swing, 5 );

			// Body number 312 Octopod
			BAData.Register( 312, AttackType.Swing, 4 );

			// Body number 313 Quadraphon
			BAData.Register( 313, AttackType.Swing, 4 );

			// Body number 314 Ettercap
			BAData.Register( 314, AttackType.Overhead, 5 );

			// Body number 315 Large Bug
			BAData.Register( 315, AttackType.Thrust, 4 ); 

			// Body number 316 Seahorse of DEATH
			BAData.Register( 316, AttackType.Swing, 5 );
			BAData.Register( 316, AttackType.Thrust, 4 ); 

			// Body number 317 Vampire Bat
			BAData.Register( 317, AttackType.Thrust, 4 ); 

			// Body number 318 Choas Lord
			BAData.Register( 318, AttackType.Swing, 5 );
			BAData.Register( 318, AttackType.Thrust, 4 ); 

			// Body number 319 Maggot Mound
			BAData.Register( 319, AttackType.Swing, 5 );
			BAData.Register( 319, AttackType.Thrust, 4 ); 

			// Body number 320 Nothing
			
			// Body number 321 through 325 Nothing

			// Body number 326 Bugged Cat that turns to a Horse

			// Body number 327 Pig
			//No usable animations

			// Body number 328 Alligator (looks like pet/zoo animations)
			BAData.Register( 328, AttackType.Swing, 4 );

			// Body number 329 Bugged Chicken that turns into a Goat

			// Body number 330 Bugged Bear into a Grizzly

			// Body number 331 Cougar (looks like pet/zoo)
			//No usable animations

			// Body number 332 Bugged Dog that turns into a Cow

			// Body number 333 Bugged Ostard that turns into a Goblin

			// Body number 334 Nothing

			// Body number 335 Bugged Dog

			// Body number 336 Bugged Horse

			// Body number 337 Bugged Cow

			// Body number 338 Bull
			//No usable animations

			// Body number 339 Nothing

			// Body number 340 Bugged out Rat

			// Body number 341 through 357 Nothing

			// Body number 358 Bugged out Goblin Archer that turns into a Grizzly Bear

			// Body number 359 Bugged out Pig

			// Body number 360 Bugged out Goblin Archer

			// Body number 361 through 399 Nothing
			
			// Body number 775 Nasty Mound
			BAData.Register( 775, AttackType.Thrust, 6 ); 

			// Body number 787 Dune Digger
			BAData.Register( 787, AttackType.Overhead, 6 );
			BAData.Register( 787, AttackType.Swing, 4 );
			BAData.Register( 787, AttackType.Thrust, 5 ); 

			// Body number 789 Nasty Mound
			BAData.Register( 789, AttackType.Thrust, 4 ); 

			// Body number 791 Beetle
			BAData.Register( 791, AttackType.Overhead, 6 );
			BAData.Register( 791, AttackType.Thrust, 5 ); 

			// Body number 793 Worg Rider 
			BAData.Register( 793, AttackType.Overhead, 3 );
			BAData.Register( 793, AttackType.Thrust, 5 ); 

			// Body number 804 Ant Queen
			BAData.Register( 804, AttackType.Thrust, 4 );
			BAData.Register( 804, AttackType.Swing, 6 );

			// Body number 805 Ant Worker
			BAData.Register( 805, AttackType.Overhead, 4 );
			BAData.Register( 805, AttackType.Swing, 6 );

			// Body number 806 Ant Warrior
			BAData.Register( 806, AttackType.Thrust, 4 ); 

			// Body number 807 Ant Queen
			BAData.Register( 807, AttackType.Thrust, 4 );
			BAData.Register( 807, AttackType.Swing, 6 );

			// Body number 808 Ant Queen
			BAData.Register( 808, AttackType.Thrust, 4 );
			// Has a different animation.
		}
	}
	
	public class BAData
	{
		private static Dictionary<int, Dictionary<int, int[]>> m_Database = new Dictionary<int, Dictionary<int, int[]>>();
		public static void Register( int BodyValue, AttackType atktype, int[] animation )
		{
			Register( BodyValue, ConvertToID( atktype ), animation );
		}
		
		public static void Register( int BodyValue, AttackType atktype, int animation )
		{
			Register( BodyValue, ConvertToID( atktype ), new int[]{ animation, 7 } );
		}
		
		public static void RegisterRanged( int BodyValue, int animation )
		{
			RegisterRanged( BodyValue, new int[]{ animation, 7 } );
		}
		
		public static void Register( int BodyValue, DefenseType deftype, int[] animation )
		{
			Register( BodyValue, ConvertToID( deftype ), animation );
		}
		
		public static void RegisterRanged( int BodyValue, int[] animation )
		{
			Register( BodyValue, 999, animation );
		}
		
		public static void Register( int BodyValue, int actionType, int[] animation )
		{
			if ( m_Database.ContainsKey( BodyValue ) )
			{
				Dictionary<int, int[]> subDict = m_Database[BodyValue];
				if ( subDict.ContainsKey( actionType ) )
					Console.WriteLine( "WARNING: Attempting to override animation ID in Initializer for Body {0}", BodyValue );
				else
					subDict[actionType] = animation;
			}
			else
			{
				m_Database[BodyValue] = new Dictionary<int, int[]>();
				m_Database[BodyValue][actionType] = animation;
			}
		}
		
		public static int[] GetAnimation( Mobile mob, AttackType atktype )
		{
			return GetAnimation( mob, ConvertToID( atktype ) );
		}
		
		public static int[] GetAnimation( Mobile mob, DefenseType deftype )
		{
			return GetAnimation( mob, ConvertToID( deftype ) );
		}
		
		public static int[] GetRangedAnimation( Mobile mob )
		{
			return GetAnimation( mob, 999 );
		}
		
		public static int[] GetAnimation( Mobile mob, int actionType )
		{
			int BodyValue = mob.BodyValue;
			if ( m_Database.ContainsKey( BodyValue ) )
			{
				Dictionary<int, int[]> subDict = m_Database[BodyValue];
				if ( subDict.ContainsKey( actionType ) )
					return subDict[actionType];
				else
					return null;
			}
			else
				return null;
		}
		
		public static int ConvertToID( AttackType atktype )
		{
			return (int)atktype;
		}
		
		public static int ConvertToID( DefenseType deftype )
		{
			return 100 + (int)deftype;
		}
	}
}
