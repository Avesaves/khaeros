using System;
using System.Collections.Generic;
using Server;

namespace Server.Items
{
	public enum CustomEffect
	{
		HitPointRestoration = 1,
		Cure = 2,
		HitPointRegeneration = 3,
		StaminaRegeneration = 4,
		ManaRegeneration = 5,
		Dexterity = 6,
		Strength = 7,
		Intelligence = 8,
		HitPoints = 9,
		Mana = 10,
		Stamina = 11,
		StaminaRestoration = 12,
		ManaRestoration = 13,
		Explosion = 14,
		Smoke = 15,
		StickyGoo = 16,
		Fire = 17,
		Thirst = 18,
		Flash = 19,
		Paralysis = 20,
		Hunger = 21,
		Rust = 22,
		Adhesive = 23,
		Shrapnel = 24,
        ImprovedVision = 25,
        Ointment = 26, // Leprosy Cure; Chaulmoogra Seeds; Chaulmoogra Plant
        Madness = 27, // Tyrean Disease Cure; Mercury; Cinnabar Gemstone
        Confusion = 28, // Bile Cure; Camphor Wax; Camphora Trees
		InfluenzaCure = 29, //Flu cure; fairy wings; Echinacea
	}

	public abstract class CustomPotionEffect
	{
		private static Dictionary<CustomEffect, CustomPotionEffect> m_Registry = new Dictionary<CustomEffect, CustomPotionEffect>();
		public virtual string Name{ get{ return ""; } }

		public virtual void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource ) // effect caused by either drink, bomb or oil (called for each player)
		{
		}

		public virtual void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map ) // what happens when a bomb explodes (only called once per explosion)
		{
		}

		public virtual bool CanDrink( Mobile mobile )
		{
			return true;
		}

		
		public static void Register( CustomPotionEffect instance, CustomEffect id )
		{
			if ( m_Registry.ContainsKey( id ) )
				Console.WriteLine( "WARNING: CustomPotionEffect found with duplicate ID (Name: {0}, ID: {1}) -- The effect was NOT registered.", instance.Name, id );
			else
				m_Registry[id] = instance;
		}

		public static CustomPotionEffect GetEffect( CustomEffect id )
		{
			return m_Registry[id];
		}
	}
	public class CircleHelper 
	{
		public static List<Point3D> GetCirclePoints(int cx, int cy, int cz, int x, int y)
		{
			List<Point3D> ret = new List<Point3D>();
			if (x == 0) 
			{
				ret.Add( new Point3D(cx, cy + y, cz) );
				ret.Add( new Point3D(cx, cy - y, cz) );
				ret.Add( new Point3D(cx + y, cy, cz) );
				ret.Add( new Point3D(cx - y, cy, cz) );
			} 
			else if (x == y) 
			{
				ret.Add( new Point3D(cx + x, cy + y, cz) );
				ret.Add( new Point3D(cx - x, cy + y, cz) );
				ret.Add( new Point3D(cx + x, cy - y, cz) );
				ret.Add( new Point3D(cx - x, cy - y, cz) );
			} 
			else if (x < y) 
			{
				ret.Add( new Point3D(cx + x, cy + y, cz) );
				ret.Add( new Point3D(cx - x, cy + y, cz) );
				ret.Add( new Point3D(cx + x, cy - y, cz) );
				ret.Add( new Point3D(cx - x, cy - y, cz) );
				ret.Add( new Point3D(cx + y, cy + x, cz) );
				ret.Add( new Point3D(cx - y, cy + x, cz) );
				ret.Add( new Point3D(cx + y, cy - x, cz) );
				ret.Add( new Point3D(cx - y, cy - x, cz) );
			}
			
			return ret;
		}

		public static List<Point3D> CircleMidpoint(int cx, int cy, int cz, int radius, bool filled)
		{
			List<Point3D> ret = new List<Point3D>();
			Dictionary<Point3D, bool> dict = new Dictionary<Point3D, bool>();
			List<Point3D> temp;
			int[] xMax = new int[radius+1];
			int x = 0;
			int y = radius;
			int p = (5 - radius*4)/4;

			temp = GetCirclePoints(cx, cy, cz, x, y);
			foreach (Point3D point in temp)
			{
				if ( filled )
				{
					int l = point.Y-cy;
					if ( l >= 0 && xMax[l] < point.X-cx)
						xMax[l] = point.X-cx;
				}
				
				dict[point] = true;
			}

			while (x < y) {
				x++;
				if (p < 0) {
					p += 2*x+1;
				} else {
					y--;
					p += 2*(x-y)+1;
				}
				temp = GetCirclePoints(cx, cy, cz, x, y);
				foreach (Point3D point in temp)
				{
					if ( filled )
					{
						int l = point.Y-cy;
						if ( l >= 0 && xMax[l] < point.X-cx)
							xMax[l] = point.X-cx;
					}
					
					dict[point] = true;
				}
			}
			
			if ( filled )
			{
				for (int i=-radius+1; i<radius; i++)
					for (int j=-xMax[Math.Abs(i)]+1; j<xMax[Math.Abs(i)]; j++)
						dict[new Point3D(cx+j, cy+i, cz)] = true;
			}
			
			foreach ( KeyValuePair<Point3D, bool> kvp in dict )
				ret.Add( kvp.Key );
			return ret;
		}
	}
}
