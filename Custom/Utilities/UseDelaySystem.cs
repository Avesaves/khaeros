using Server;
using System;
using System.Collections.Generic;
using System.IO;
using Server.Commands;
using Server.Mobiles;

namespace Server.Items
{

	//to make an item/ability use this system simply add another entry in this list:
	
	public enum DelayContextType
	{
	LootCommand,
	WoundCommand
	};



	public class UseDelaySystem
	{
		/*
		Functions:
		AddContext(Mobile m, DelayContextType type, TimeSpan delay)    			//set a use delay		
		RemoveContext(Mobile m,DelayContextType type) 					  //prematurely remove a delay  (returns true if it existed already)
		CheckContext(Mobile m, DelayContextType type)						 //check if a delay exists for specified abiliy (returns true if it exists)
		*/
		
		//----START SYSTEM----
		
		
		public class DelayContextArray
		{
			private Dictionary<DelayContextType,DateTime> Contexts = new Dictionary<DelayContextType,DateTime>();
		
			public DelayContextArray()
			{
			}
			
			public bool Contains(DelayContextType dct)
			{
				DateTime context;
		
				if(Contexts.TryGetValue(dct,out context))
				{
					//Console.WriteLine("context: {0}",context);//debug
					if(context>DateTime.Now)return true;
					else Contexts.Remove(dct);				
				}
				//Console.WriteLine("-context: {0}",context);//debug
				return false;
			}
			
			public bool Remove(DelayContextType dct)
			{
				DateTime context;
			
				if(Contexts.TryGetValue(dct,out context))
				{
					if(context>DateTime.Now)
					{
						Contexts.Remove(dct);
						return true;		
					}
				}
				return false;			
			}
			
			public void Add(DelayContextType dct,TimeSpan delay)
			{
				DateTime context;
				
				if(Contexts.TryGetValue(dct,out context))
				{
					//Console.WriteLine("contains {0}, removing... {1}",dct,context);//debug
					Contexts.Remove(dct);
				}
				
				//Console.WriteLine("Adding...{0} {1}",dct,delay);//debug
				Contexts[dct] = DateTime.Now+delay;
			}
			
			public bool IsEmpty()
			{
			return (Contexts.Count <=0);
			}
			
			public bool Cleanup()
			{
		       for(int i=Contexts.Count;i>0;i--)
			   {
					foreach( KeyValuePair<DelayContextType, DateTime> kvp in Contexts )
			        {
						if(kvp.Value<DateTime.Now)
						{
						Contexts.Remove(kvp.Key);
						break;
						}
			        }
				
				}
				
			return IsEmpty();
			}
			
			
			public void DumpInfo(StreamWriter op)
			{
		        foreach( KeyValuePair<DelayContextType, DateTime> kvp in Contexts )
		        {
		            string expired="[active]";
					if(kvp.Value<DateTime.Now)expired="[expired]";
					
					op.WriteLine("{0} ==> {1} {2}", kvp.Key,kvp.Value,expired);
		        }
				op.WriteLine();
			}
			
			
		}
		
		
		private static Dictionary<Mobile, DelayContextArray> UseDelayMain = new Dictionary<Mobile, DelayContextArray>();
	
		//Main functions
		//============

		//checks if m is still being delayed by "type" context (ex: need to wait again to use that ability type)
		public static bool CheckContext(Mobile m, DelayContextType type)
		{
			DelayContextArray array;
		
			if(UseDelayMain.TryGetValue(m,out array))
			{
			if(array==null)return false;
			bool ans = array.Contains(type);			
			if(!ans && array.IsEmpty())UseDelayMain.Remove(m);
			return ans;
			}
			
		return false;
		}
		
		
		//removes context.  Returns true if it did exist, or false if it did not
		public static bool RemoveContext(Mobile m,DelayContextType type)
		{
			DelayContextArray array;
		
			if(UseDelayMain.TryGetValue(m,out array))
			{
				if(array.Remove(type))	return true;							
			}		
			return false;
		}
		
		
		
		
		//tries to add a context with a delay
		public static void AddContext(Mobile m, DelayContextType type,TimeSpan delay)
		{
			DelayContextArray array;
		
			if(!UseDelayMain.TryGetValue(m,out array))
			{
				//Console.WriteLine("Cant find array, adding...");//debug
				array = new DelayContextArray();
				UseDelayMain[m] = array;
			}
			
			array.Add(type,delay);	
		}
		
		
		
		
		//Management functions:
		//======================
		
		public static void Cleanup()
		{
		    for(int i=UseDelayMain.Count;i>0;i--)
			{
				
				foreach( KeyValuePair<Mobile, DelayContextArray> kvp in UseDelayMain )
		        {					
					DelayContextArray dca = UseDelayMain[kvp.Key];
					
					if(dca.Cleanup())
					{
					UseDelayMain.Remove(kvp.Key);
					break;
					}
		        }
			}
		}
		
		
		
		
		public static void DumpStats()
		{	
			using ( StreamWriter op = new StreamWriter( "UseDelaySystem.txt" ) )
			{		
			op.WriteLine("UseDelaySystem usage status: Generated on {0}",DateTime.Now);
			op.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
			op.WriteLine();
			
		        foreach( KeyValuePair<Mobile, DelayContextArray> kvp in UseDelayMain )
		        {
		            op.WriteLine("{0}", kvp.Key);
					op.WriteLine("---------");
					
					DelayContextArray dca = UseDelayMain[kvp.Key];
					
					dca.DumpInfo(op);	
		        }			
			
			}			
		}
		
		
	}
	
	
	public class udsdump
	{
		public static void Initialize()
		{
			CommandSystem.Register( "udsdump", AccessLevel.GameMaster, new CommandEventHandler( udsdump_OnCommand ) );
		}

		[Usage( "udsdump" )]
		[Description( "Dumps Use Delay System information" )]
		private static void udsdump_OnCommand( CommandEventArgs e )
		{		
		PlayerMobile pm = e.Mobile as PlayerMobile;
		
		UseDelaySystem.DumpStats();
		pm.SendMessage("Dumped UseDelaySystem info to UseDelaySystem.txt");
		}
		
	}
	
	public class udscleanup
	{
		public static void Initialize()
		{
			CommandSystem.Register( "udscleanup", AccessLevel.GameMaster, new CommandEventHandler( udscleanup_OnCommand ) );
		}

		[Usage( "udscleanup" )]
		[Description( "Cleans up Use Delay System" )]
		private static void udscleanup_OnCommand( CommandEventArgs e )
		{		
		PlayerMobile pm = e.Mobile as PlayerMobile;
		
		UseDelaySystem.Cleanup();
		pm.SendMessage("Use Delay System Cleanup Complete");
		}
		
	}	
	
	
}