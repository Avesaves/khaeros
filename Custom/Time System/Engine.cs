/*******************************************
 *               Time System
 * 
 * Version        : 2.0.6
 * By             : Morxeton
 * 
 * Date Created   : February 8, 2006
 * Date Modified  : March 18, 2007
 * 
 *******************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.TimeSystem
{
    public class Engine
    {
		private static bool m_Announce = false;
		
        #region Initialize

        public static void Initialize()
        {
            m_LoginQueue = new Queue();
            m_LogoutQueue = new Queue();

            EventSink.Login += new LoginEventHandler(OnLogin);
            EventSink.Disconnected += new DisconnectedEventHandler(OnDisconnected);
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);

            new DelayedInitializationTimer().Start();
        }
		
		public static bool Announcing { get { return m_Announce; } set { m_Announce = value; } }

        private static void InitializeSystem()
        {
            Data.Loading = true;

            Console.WriteLine(String.Format("Time System: Version {0} loading...", Data.Version));

            Support.OpenLogFile();

            SelfInstaller.Install();

            LightsEngine.PopulateLightsList();

            Data.Load();

            Support.CheckForceScriptSettings();

            Support.UpdateCodeChars();

            Start();

            Support.ConsoleWriteLine(String.Format("Time System: {0}", Formatting.GetTimeFormat(null, Format.Time)));

            Data.Loading = false;
        }

        #endregion

        #region Private Variables

        private static TimeSystemTimer m_TimeSystemTimer;
        private static LightsEngineTimer m_LightsEngineTimer;
        private static MobileObjectQueueTimer m_MobileObjectQueueTimer;

        private static Queue m_LoginQueue;
        private static Queue m_LogoutQueue;

        #endregion

        #region Event Sinks

        public static void OnLogin(LoginEventArgs args)
        {
            Mobile mobile = args.Mobile;

            MobileObject mo = new MobileObject();

            mo.Mobile = mobile;

            mo.IsNightSightOn = !mobile.CanBeginAction(typeof(LightCycle));

            m_LoginQueue.Enqueue(mo);
        }

        public static void OnDisconnected(DisconnectedEventArgs args)
        {
            Mobile mobile = args.Mobile;

            m_LogoutQueue.Enqueue(mobile);
        }

        public static void OnWorldSave(WorldSaveEventArgs e)
        {
            new DelayedSaveTimer().Start();
        }

        #endregion

        #region System Control

        public static void Stop()
        {
            Data.Enabled = false;

            if (m_TimeSystemTimer != null)
            {
                m_TimeSystemTimer.Stop();
            }

            if (m_LightsEngineTimer != null)
            {
                m_LightsEngineTimer.Stop();
            }

            if (m_MobileObjectQueueTimer != null)
            {
                m_MobileObjectQueueTimer.Stop();
            }
        }

        public static void Start()
        {
            Data.Enabled = true;

            m_TimeSystemTimer = new TimeSystemTimer();
            m_TimeSystemTimer.Start();

            m_LightsEngineTimer = new LightsEngineTimer();
            m_LightsEngineTimer.Start();

            m_MobileObjectQueueTimer = new MobileObjectQueueTimer();
            m_MobileObjectQueueTimer.Start();
        }

        public static void Restart()
        {
            if (Data.Enabled)
            {
                Stop();
                Start();
            }
        }

        #endregion

        #region Calculation Methods

        private static void OnMasterTick()
        {
            TimeEngine.CalculateBaseTime();
            EffectsEngine.CheckEvilSpawners();

            for (int i = 0; i < NetState.Instances.Count; i++)
            {
                Mobile mobile = ((NetState)NetState.Instances[i]).Mobile;

                if (mobile != null)
                {
                    mobile.CheckLightLevels(false);
                    
                    if( mobile is PlayerMobile && ((PlayerMobile)mobile).IsVampire )
                    	((PlayerMobile)mobile).HandleVampireStatOffsets();
                }
            }
        }

        private static void OnLightsEngineTick()
        {
            LightsEngine.CheckLights();
			
			if ((Announcing == false) && Data.Minute == 0)
			{
				if ((Data.Hour == 19) || (Data.Hour == 20) || (Data.Hour == 23) || (Data.Hour == 1) || (Data.Hour == 2) || (Data.Hour == 5) || (Data.Hour == 7) || (Data.Hour == 8) || (Data.Hour == 11) || (Data.Hour == 13) || (Data.Hour == 15) || (Data.Hour == 16) || (Data.Hour == 18)) {
				Announcing = true;			
            	foreach ( NetState state in NetState.Instances )
				{
					Mobile m = state.Mobile;
	
					if ( m != null )
					{
						string text = Formatting.GetDescriptiveTime(m);
						m.SendMessage(text);
					}
				}
				}
            } else if (Announcing && (Data.Minute > 0))
				Announcing = false;
        }

        #endregion

        #region Timers

        private class DelayedInitializationTimer : Timer
        {
            public DelayedInitializationTimer()
                : base(TimeSpan.FromSeconds(0.1))
            {
            }

            protected override void OnTick()
            {
                InitializeSystem();
            }
        }

        private class DelayedSaveTimer : Timer
        {
            public DelayedSaveTimer()
                : base(TimeSpan.FromSeconds(1.0))
            {
            }

            protected override void OnTick()
            {
                Data.Save();
            }
        }

        private class MobileObjectQueueTimer : Timer
        {
            public MobileObjectQueueTimer()
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(5.0))
            {
            }

            protected override void OnTick()
            {
                if (m_LoginQueue.Count > 0 || m_LogoutQueue.Count > 0)
                {
                    lock (Data.MobilesTable)
                    {
                        while (m_LogoutQueue.Count > 0)
                        {
                            Mobile mobile = (Mobile)m_LogoutQueue.Dequeue();

                            Data.MobilesTable.Remove(mobile);
                        }

                        while (m_LoginQueue.Count > 0)
                        {
                            MobileObject mo = (MobileObject)m_LoginQueue.Dequeue();

                            try
                            {
                                Data.MobilesTable.Add(mo.Mobile, mo);
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        #endregion

        #region Master Timer

        private class TimeSystemTimer : Timer
        {
            public TimeSystemTimer()
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(Data.TimerSpeed))
            {
            }

            protected override void OnTick()
            {
                OnMasterTick();
            }
        }

        #endregion

        #region Light Engine Timer

        private class LightsEngineTimer : Timer
        {
            public LightsEngineTimer()
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(Data.LightsEngineTimerSpeed))
            {
            }

            protected override void OnTick()
            {
                OnLightsEngineTick();
            }
        }

        #endregion
    }
}
